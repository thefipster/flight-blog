﻿using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Domain.Datahub
{
    public class Airport : JsonBase
    {
        public Airport() { }

        [JsonPropertyName("continent")]
        public string? Continent { get; set; }

        /// <summary>
        /// WARNING the format of this coordinate is "LON, LAT"
        /// </summary>
        [JsonPropertyName("coordinates")]
        public string? Coordinates { get; set; }

        [JsonPropertyName("elevation_ft")]
        public string? ElevationFt { get; set; }

        [JsonPropertyName("gps_code")]
        public string? GpsCode { get; set; }

        [JsonPropertyName("iata_code")]
        public string? IataCode { get; set; }

        [JsonPropertyName("ident")]
        public string? Ident { get; set; }

        [JsonPropertyName("iso_country")]
        public string? IsoCountry { get; set; }

        [JsonPropertyName("iso_region")]
        public string? IsoRegion { get; set; }

        [JsonPropertyName("local_code")]
        public string? LocalCode { get; set; }

        [JsonPropertyName("municipality")]
        public string? Municipality { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
