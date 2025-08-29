namespace AdvertisingPlatforms.Services;

public interface IAdvertisingService
{
    Task<Models.LoadResult> LoadFromStreamAsync(Stream stream, CancellationToken cancellationToken = default);
    IReadOnlyList<string> GetAdvertisersForLocation(string location);
}