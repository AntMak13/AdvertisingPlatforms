using System.Reflection;
using AdvertisingPlatforms.Services;

namespace AdvertisingPlatforms.Tests;

public class AdvertisingServiceTests
{
    [Fact]
    public void GetAdvertisersForLocation_ReturnsEmptyList_WhenNoData()
    {
        var service = new AdvertisingService();
        var result = service.GetAdvertisersForLocation("Moscow");
        
        Assert.Empty(result);
    }

    [Fact]
    public void GetAdvertisersForLocation_ReturnsAdvertiser_WhenDataIsCorrect()
    {
        var service = new AdvertisingService();
        service.LoadFromLines(new[] { "Ревдинский рабочий: /ru/svrd/revda" });

        var result = service.GetAdvertisersForLocation("/ru/svrd/revda");

        Assert.Single(result);
        Assert.Equal("Ревдинский рабочий", result[0]); 
    }

    [Fact]
    public void GetAdvertisersForLocation_ReturnsError_WhenNoLocation()
    {
        var service = new AdvertisingService();
        var lines = new[] { "Яндекс.Директ:" };

        var result = service.LoadFromLines(lines);

        Assert.Equal(1, result.LinesProcessed);   
        Assert.Equal(0, result.AdvertisersLoaded);
        Assert.Equal(1, result.Errors);  
    }

    [Fact]
    public void GetAdvertisersForLocation_WithoutColon_ReturnsError()
    {
        var service = new AdvertisingService();
        var lines = new[] { "Газета уральских москвичей" };
        
        var result = service.LoadFromLines(lines);
        
        Assert.Equal(1, result.LinesProcessed);   
        Assert.Equal(0, result.AdvertisersLoaded);
        Assert.Equal(1, result.Errors);
    }
}