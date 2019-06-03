using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using RcepcionApi.EntityModels;

namespace RcepcionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<UsuarioEntity> _signInManager;
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly RoleManager<UsuarioRoleEntity> _roleManager;

        public TokenController(
            IOptions<IdentityOptions> identityOptions,
            SignInManager<UsuarioEntity> signInManager,
            UserManager<UsuarioEntity> userManager,
            RoleManager<UsuarioRoleEntity> roleManager)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost(Name = nameof(TokenExchange))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> TokenExchange(OpenIdConnectRequest request)
        {
            if (!request.IsPasswordGrantType())
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "El tipo de concesión especificado no es compatible."
                });
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "El usuario o contraseña es invalido."
                });
            }

            // Ensure the user is allowed to sign in
            if (!await _signInManager.CanSignInAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "El usuario no tiene permitido ingresar."
                });
            }

            // Ensure the user is not already locked out
            if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "El usuario o contraseña es invalido."
                });
            }

            // Ensure the password is valid
            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                if (_userManager.SupportsUserLockout)
                {
                    await _userManager.AccessFailedAsync(user);
                }

                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "El usuario o contraseña es invalido."
                });
            }

            // Reset the lockout count
            if (_userManager.SupportsUserLockout)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
            }

            // Look up the user's roles (if any)
            var roles = new string[0];
            if (_userManager.SupportsUserRole)
            {
                roles = (await _userManager.GetRolesAsync(user)).ToArray();
            }

            // Create a new authentication ticket w/ the user identity
            var ticket = await CreateTicketAsync(request, user, roles);

            var signInResult = SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            return signInResult;// SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, UsuarioEntity user, string[] roles)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            AddRolesToPrincipal(principal, roles);

            var ticket = new AuthenticationTicket(principal,
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme);

            //ticket.SetScopes(OpenIddictConstants.Scopes.Roles);
            if (!request.IsRefreshTokenGrantType())
            {
                ticket.SetScopes(new[] {OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles}.Intersect(request.GetScopes()));
            }
            // Explicitly specify which claims should be included in the access token
            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp (it's a secret value)
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType) continue;

                // TODO: If there are any other private/secret claims on the user that should
                // not be exposed publicly, handle them here!
                // The token is encoded but not encrypted, so it is effectively plaintext.

                claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken);
            }

            return ticket;
        }

        private static void AddRolesToPrincipal(ClaimsPrincipal principal, string[] roles)
        {
            var identity = principal.Identity as ClaimsIdentity;

            var alreadyHasRolesClaim = identity.Claims.Any(c => c.Type == "role");
            if (!alreadyHasRolesClaim && roles.Any())
            {
                identity.AddClaims(roles.Select(r => new Claim("role", r)));
            }

            var newPrincipal = new System.Security.Claims.ClaimsPrincipal(identity);
        }
    }
}