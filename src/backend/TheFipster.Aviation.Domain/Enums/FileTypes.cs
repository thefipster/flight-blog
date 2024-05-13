﻿using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FileTypes
    {
        Empty,
        OfpPdf,
        Chart,
        BlackBoxCsv,
        RouteKml,
        PathKml,
        MsfsFlightPlan,
        MergedFlightJson,
        AirportJson,
        SimToolkitProJson,
        BlackBoxJson,
        SimbriefJson,
        SimbriefXml,
        Unknown,
        Error,
        LandingJson,
        TrackJson,
        ChartAirport,
        ChartParking,
        ChartDeparture,
        ChartArrival,
        ChartApproach,
        ChartTaxi,
        NotamJson,
        LogbookJson,
        OfpHtml,
        RouteJson,
        Screenshot,
        WaypointsJson,
        BlackBoxTrimmedJson,
        StatsJson,
        Thumbnail,
        ChartImage,
        ChartThumbnail,
        GeoTagsJson,
        BlackBoxCompressedJson
    }
}
