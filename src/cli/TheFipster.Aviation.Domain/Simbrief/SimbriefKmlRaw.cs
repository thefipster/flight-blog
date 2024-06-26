﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'System.Text.Json' then do:
//
//    using TheFipster.Aviation.Domain.Simbrief.Kml;
//
//    var simbriefKmlRaw = SimbriefKmlRaw.FromJson(jsonString);
#nullable enable
#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8603

namespace TheFipster.Aviation.Domain.Simbrief.Kml
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json.Serialization;

    public partial class SimbriefKmlRaw
    {
        [JsonPropertyName("?xml")]
        public Xml Xml { get; set; }

        [JsonPropertyName("kml")]
        public Kml Kml { get; set; }
    }

    public partial class Kml
    {
        [JsonPropertyName("@xmlns")]
        public Uri Xmlns { get; set; }

        [JsonPropertyName("Document")]
        public Document Document { get; set; }
    }

    public partial class Document
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("Style")]
        public List<Style> Style { get; set; }

        [JsonPropertyName("Placemark")]
        public List<Placemark> Placemark { get; set; }
    }

    public partial class Placemark
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("styleUrl")]
        public string StyleUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LineString")]
        public LineString LineString { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Point")]
        public Point Point { get; set; }
    }

    public partial class LineString
    {
        [JsonPropertyName("tessellate")]
        public string Tessellate { get; set; }

        [JsonPropertyName("extrude")]
        public string Extrude { get; set; }

        [JsonPropertyName("altitudeMode")]
        public string AltitudeMode { get; set; }

        [JsonPropertyName("coordinates")]
        public string Coordinates { get; set; }
    }

    public partial class Point
    {
        [JsonPropertyName("altitudeMode")]
        public string AltitudeMode { get; set; }

        [JsonPropertyName("coordinates")]
        public string Coordinates { get; set; }
    }

    public partial class Style
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LineStyle")]
        public LineStyle LineStyle { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("PolyStyle")]
        public PolyStyle PolyStyle { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("IconStyle")]
        public IconStyle IconStyle { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("color")]
        public string Color { get; set; }
    }

    public partial class IconStyle
    {
        [JsonPropertyName("Icon")]
        public Icon Icon { get; set; }
    }

    public partial class Icon
    {
        [JsonPropertyName("href")]
        public Uri Href { get; set; }
    }

    public partial class LineStyle
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }
    }

    public partial class PolyStyle
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }
    }

    public partial class Xml
    {
        [JsonPropertyName("@version")]
        public string Version { get; set; }

        [JsonPropertyName("@encoding")]
        public string Encoding { get; set; }
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8601
#pragma warning restore CS8603
