using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Utilities;

public static class WktConverter
{
    public static string ToWkt(double latitude, double longitude)
    {
        return $"POINT({longitude} {latitude})";
    }

    public static Location FromWkt(string wkt)
    {
        var parts = wkt.Replace("POINT(", "").Replace(")", "").Split(' ');
        return new Location
        {
            Latitude = double.Parse(parts[1]),
            Longitude = double.Parse(parts[0])
        };
    }
}
