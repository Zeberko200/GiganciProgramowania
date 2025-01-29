using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    protected string GetIpAddress()
    {
        var ip = HttpContext?.Connection?.RemoteIpAddress?.ToString();
        if (string.IsNullOrWhiteSpace(ip))
        {
            throw new Exception("IP address not found.");
        }
        
        return ip;
    }
}