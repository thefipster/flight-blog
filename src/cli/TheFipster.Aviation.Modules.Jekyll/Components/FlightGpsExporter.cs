﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.Simbrief.Kml;
using TheFipster.Aviation.Modules.Jekyll.Model;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class FlightGpsExporter
    {
        private int flightNumber;

        public FlightGeo GenerateGpsApiData(FlightImport flight)
        {
            flightNumber = int.Parse(flight.FlightNumber);

            var geo = new FlightGeo();
            geo.Events = generateLocation(flight.Events);
            geo.Waypoints = generateLocation(flight.SimbriefKml.Kml.Document.Placemark);
            geo.Images = generateLocation(flight.Geotags);
            geo.Track = generateTrack(flight.Track);

            return geo;
        }

        private IEnumerable<Location> generateLocation(List<Placemark> placemark)
        {
            foreach (var place in placemark)
            {
                if (place.Point != null)
                    yield return new Location(place);
            }
        }

        private IEnumerable<Location> generateLocation(IEnumerable<Waypoint> collection)
        {
            foreach (var item in collection ?? [])
                yield return new Location(item);
        }

        private IEnumerable<UriLocation> generateLocation(IEnumerable<GeoTag> collection)
        {

            foreach (var item in collection ?? [])
                yield return new UriLocation(item, flightNumber);
        }

        private IEnumerable<IEnumerable<decimal>> generateTrack(IEnumerable<Coordinate> collection)
        {
            foreach (var item in collection ?? [])
                yield return [
                    item.Latitude.RoundToSignificantDigits(5),
                    item.Longitude.RoundToSignificantDigits(5)
                ];
        }
    }
}
