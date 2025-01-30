using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    protected async Task<string> GetIpAddress()
    {
        var host = await Dns.GetHostAddressesAsync(Dns.GetHostName());

        return host.First().MapToIPv4().ToString();
    }
}