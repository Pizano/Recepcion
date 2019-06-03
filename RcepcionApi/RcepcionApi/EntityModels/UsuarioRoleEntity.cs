using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.EntityModels
{
    public class UsuarioRoleEntity : IdentityRole
    {
        public UsuarioRoleEntity()
            : base()
        {
        }

        public UsuarioRoleEntity(string roleName)
            : base(roleName)
        {
        }
    }
}
