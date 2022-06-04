using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CSharpCachePoc.Controllers;

[ApiController]
[Route("/")]
public class ApiController : ControllerBase
{
    private const string RandomNumberCacheKey = "randomNumber";
    private readonly ILogger<ApiController> _logger;
    private readonly IMemoryCache _cache;

    public ApiController(
        ILogger<ApiController> logger,
        IMemoryCache cache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    [HttpGet]
    public int Get()
    {
        // If not found, process and store in cache
        if (_cache.TryGetValue(RandomNumberCacheKey, out int randomNumber))
        {
            return randomNumber;
        }
        
        _logger.Log(LogLevel.Information, "Random number not in cache, processing...");

        // Simulate slow process that should be cached 
        Thread.Sleep(3000);
        randomNumber = new Random().Next(1000);

        _cache.Set(RandomNumberCacheKey, randomNumber, new MemoryCacheEntryOptions());

        return randomNumber;
    }
}
