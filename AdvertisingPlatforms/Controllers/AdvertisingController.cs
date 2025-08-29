using AdvertisingPlatforms.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvertisingController : ControllerBase
{
    private readonly IAdvertisingService _advertisingService;

    public AdvertisingController(IAdvertisingService advertisingService)
    {
        _advertisingService = advertisingService;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAdvertiser(IFormFile file)
    {
        if (file == null)
            return BadRequest("File not found");

        using var stream = file.OpenReadStream();
        var result = await _advertisingService.LoadFromStreamAsync(stream);

        return Ok(result);
    }

    [HttpGet("search")]
    public IActionResult SearchAdvertisers([FromQuery] string location)
    {
        if (string.IsNullOrEmpty(location))
        {
            return BadRequest("Location not found");
        }
        
        var advertisers = _advertisingService.GetAdvertisersForLocation(location);
        
        return Ok(advertisers);
    }
}