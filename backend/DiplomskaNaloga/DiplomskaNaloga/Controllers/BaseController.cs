
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DiplomskaNaloga.Controllers
{
    [Route("api/[controller]/[action]"), ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid? UserId
        {
            get
            {
                var id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);                
                if (id != null && Guid.TryParse(id.Value, out Guid userId)) return userId;
                return Guid.Empty;
            }
        }

        protected string Role
        {
            get
            {
                return User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role).Value;
            }
        }
    }
}

