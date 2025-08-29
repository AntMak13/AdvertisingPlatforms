namespace AdvertisingPlatforms.Models;

public class LoadResult
{
    public int LinesProcessed { get; init; }
    public int AdvertisersLoaded { get; init; }
    public int Errors { get; init; }
}