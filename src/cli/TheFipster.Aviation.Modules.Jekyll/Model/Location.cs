﻿using System.Text.Json.Serialization;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.Simbrief.Kml;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class Location
    {
        public Location(Waypoint item)
        {
            Name = item.Name;
            LatLon = [
                item.Latitude.RoundToSignificantDigits(4),
                item.Longitude.RoundToSignificantDigits(4)
            ];
        }

        public Location(GeoTag item)
        {
            Name = item.Screenshot.Replace(" ", string.Empty);
            LatLon = [
                item.Latitude.RoundToSignificantDigits(4),
                item.Longitude.RoundToSignificantDigits(4)
            ];
        }

        public Location(OurAirport airport)
        {
            Name = airport.Ident;
            LatLon = [
                airport.Latitude.RoundToSignificantDigits(4),
                airport.Longitude.RoundToSignificantDigits(4)
            ];
        }

        public Location(Placemark place)
        {
            Name = place.Name;
            var coord = Coordinate.FromAirportCoordinateString(place.Point.Coordinates);
            LatLon = [
                coord.Latitude.RoundToSignificantDigits(4), 
                coord.Longitude.RoundToSignificantDigits(4)
            ];
        }

        public Location(string name, List<decimal> latlon)
        {
            Name = name;
            LatLon = latlon;
        }

        public Location(string name, double latitude, double longitude)
        {
            Name = name;
            LatLon = [
                latitude.RoundToSignificantDigits(4),
                longitude.RoundToSignificantDigits(4)
            ];
        }

        public Location(string name, decimal latitude, decimal longitude)
        {
            Name = name;
            LatLon = [latitude, longitude];
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("latlon")]
        public List<decimal> LatLon { get; set; }
    }
}
