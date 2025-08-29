using System.Collections.Immutable;
using AdvertisingPlatforms.Models;

namespace AdvertisingPlatforms.Services;

public class AdvertisingService : IAdvertisingService
{
    private ImmutableDictionary<string, ImmutableHashSet<string>> _map = 
        ImmutableDictionary<string, ImmutableHashSet<string>>.Empty.WithComparers(StringComparer.OrdinalIgnoreCase);
    
    public async Task<LoadResult> LoadFromStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(stream);
        var lines = new List<string>();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line is not null) lines.Add(line);
        }

        return LoadFromLines(lines);
    }

    public LoadResult LoadFromLines(IEnumerable<string> lines)
    {
        var tmp = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        int linesProcessed = 0;
        int errors = 0;
        int advertisers = 0;

        foreach (var rawLine in lines)
        {
            linesProcessed++;

            if (string.IsNullOrEmpty(rawLine))
            {
                continue;
            }
            var line = rawLine.Trim();

            var parts = line.Split(':', 2);
            if (parts.Length != 2)
            {
                errors++;
                continue;
            }

            var name = parts[0].Trim();
            if (string.IsNullOrEmpty(name))
            {
                errors++;
                continue;
            }

            var locsPart = parts[1].Trim();
            if (string.IsNullOrEmpty(locsPart))
            {
                errors++;
                continue;
            }

            var locs = locsPart.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => NormalizeLocation(l.Trim()))
                .Where(l => !string.IsNullOrEmpty(l));

            bool any = false;
            foreach (var loc in locs)
            {
                any = true;
                if (!tmp.TryGetValue(loc, out var set))
                {
                    set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    tmp[loc] = set;
                }

                set.Add(name);
            }


            if (any) advertisers++;
        }
        
        var immutable = tmp.ToImmutableDictionary(kv => kv.Key, kv => kv.Value.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
            _map = immutable;

        return new LoadResult { LinesProcessed = linesProcessed, AdvertisersLoaded = advertisers, Errors = errors };
    }
    
    private static string NormalizeLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location)) return string.Empty;
        location = location.Trim();
        if (!location.StartsWith('/')) location = "/" + location;
        while (location.Length > 1 && location.EndsWith('/')) location = location.TrimEnd('/');
        return location;
    }
    
    public IReadOnlyList<string> GetAdvertisersForLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location)) return Array.Empty<string>();
        location = NormalizeLocation(location);

        var segments = location.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var prefixes = new List<string>();
        string cur = string.Empty;

        foreach (var seg in segments)
        {
            cur = cur + "/" + seg;
            prefixes.Add(cur);
        }

        var matches = new List<(string advertiser, int prefixLength)>();

        foreach (var pref in prefixes)
        {
            if (_map.TryGetValue(pref, out var set))
            {
                foreach (var adv in set)
                {
                    matches.Add((adv, pref.Length));
                }
            }

        }

        // уникальные рекламодатели, сортируем по наиболее специфичному совпадению (длина префикса), затем по имени
        var result = matches
            .GroupBy(m => m.advertiser, StringComparer.OrdinalIgnoreCase)
            .Select(g => new { name = g.Key, maxPrefix = g.Max(x => x.prefixLength) })
            .OrderByDescending(x => x.maxPrefix)
            .ThenBy(x => x.name, StringComparer.OrdinalIgnoreCase)
            .Select(x => x.name)
            .ToList();

        return result;
    }
}