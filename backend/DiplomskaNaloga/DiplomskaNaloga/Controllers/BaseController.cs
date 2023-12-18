
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
                string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (Guid.TryParse(id, out Guid userId)) return userId;
                return null;
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

