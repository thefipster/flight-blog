﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.Modules.Simbrief
{
    public class SimbriefImporter
    {
        public SimBriefFlight Import(string flightFolder)
        {
            var split = new SimbriefXmlSplitter().Split(flightFolder);

            var departure = split.Flight.Departure.Icao;
            var arrival = split.Flight.Arrival.Icao;

            var route = new SimbriefKmlLoader().Load(flightFolder);

            split.Waypoints.FileType = FileTypes.WaypointsJson;
            new JsonWriter<SimbriefWaypoints>().Write(flightFolder, split.Waypoints, FileTypes.WaypointsJson, departure, arrival);

            split.Notams.FileType = FileTypes.NotamJson;
            new JsonWriter<NotamReport>().Write(flightFolder, split.Notams, FileTypes.NotamJson, departure, arrival);

            split.Flight.FileType = FileTypes.SimbriefJson;
            new JsonWriter<SimBriefFlight>().Write(flightFolder, split.Flight, FileTypes.SimbriefJson, departure, arrival);

            route.FileType = FileTypes.RouteJson;
            new JsonWriter<Route>().Write(flightFolder, route, FileTypes.RouteJson, departure, arrival);

            new PlainWriter().Write(flightFolder, split.Ofp, FileTypes.OfpHtml, departure, arrival);

            return split.Flight;
        }
    }
}
