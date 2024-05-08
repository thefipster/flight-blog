﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class TrackCommand
    {
        private HardcodedConfig config;

        internal TrackCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(TrackOptions options)
        {
            Console.WriteLine($"Extracting tracks from SimToolkitPro");
            var flightFolders = Directory.GetDirectories(config.FlightsFolder);
            foreach (var folder in flightFolders)
                extractTrack(folder);
        }

        private void extractTrack(string folder)
        {
            Console.Write($"\t {Path.GetFileName(folder)}");
            SimToolkitProFlight? flight = getFlight(folder);
            if (flight == null)
            {
                Console.WriteLine(" - no file.");
                return;
            }

            Track? track = extractTrack(flight);
            Console.WriteLine($" - {track.Features.First().Geometry.Coordinates.Count} coordinates.");
            new JsonWriter<Track>().Write(folder, track, "Track", track.Departure, track.Arrival);
        }

        private SimToolkitProFlight? getFlight(string folder)
        {
            var files = new ScanCommand(config).Scan(folder);
            foreach (var file in files)
                if (file.Value == Domain.Enums.FileTypes.SimToolkitProJson)
                    return new JsonReader<SimToolkitProFlight>().FromFile(file.Key);

            return null;
        }

        private Track extractTrack(SimToolkitProFlight? flight)
        {
            Track? track = new JsonReader<Track>().FromText(flight.Logbook.TrackedGeoJson);
            track.Departure = flight.Logbook.Dep;
            track.Arrival = flight.Logbook.Arr;
            return track;
        }
    }
}
