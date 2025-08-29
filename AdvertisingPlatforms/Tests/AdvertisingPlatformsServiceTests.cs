using System.Text;
using AdvertisingPlatforms.Services;
using Xunit;

namespace AdvertisingPlatforms.Tests;

public class AdvertisingPlatformsServiceTests
{
    [Fact]
    public void LoadAndSearch_BasicScenario()
    {
        var text = new StringBuilder()
            .AppendLine("Яндекс.Директ:/ru")
            .AppendLine("Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik")
            .AppendLine("Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl")
            .AppendLine("Крутая реклама:/ru/svrd")
            .ToString();

        var advertisingService = new AdvertisingService();
        var res = advertisingService.LoadFromLines(text.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries));
        Assert.Equal(4, res.LinesProcessed);
        Assert.Equal(4, res.AdvertisersLoaded);

        var r1 = advertisingService.GetAdvertisersForLocation("/ru/msk");
        Assert.Contains("Яндекс.Директ", r1);
        Assert.Contains("Газета уральских москвичей", r1);

        var r2 = advertisingService.GetAdvertisersForLocation("/ru/svrd/revda");
        Assert.Contains("Яндекс.Директ", r2);
        Assert.Contains("Ревдинский рабочий", r2);
        Assert.Contains("Крутая реклама", r2);
    }
}