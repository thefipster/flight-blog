﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class CreateSimbriefFlightCommand : ICommand<CreateSimbriefFlightOptions, Leg>
    {
        private readonly SimbriefDownloader downloader;
        private readonly FileOperations fileOperations;
        private readonly JsonWriter<SimbriefImport> simbriefWriter;

        public CreateSimbriefFlightCommand()
        {
            downloader = new SimbriefDownloader();
            fileOperations = new FileOperations();
            simbriefWriter = new JsonWriter<SimbriefImport>();
        }

        public Leg Run(CreateSimbriefFlightOptions options, IConfig config)
        {
            Console.WriteLine(CreateSimbriefFlightOptions.Welcome);
            Guard.EnsureConfig(config);

            SimbriefImport flightPlan;
            while (true)
            {
                flightPlan = downloader.DownloadJson(config.SimbriefPilotId);
                var msg = $"Found flight: {flightPlan.Origin.IcaoCode} - {flightPlan.Destination.IcaoCode}. Is this correct?";
                var confirmation = StdOut.YesNoDecision(msg, false);
                if (confirmation)
                    break;

                Console.WriteLine("Please refile the flight plan and press ENTER when it is created.");
                Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine($"Your chosen flight plan will be from {flightPlan.Origin.IcaoCode} to {flightPlan.Destination.IcaoCode}");

            var flightNo = Directory.GetDirectories(config.FlightsFolder).Count() + 1;

            Leg leg = new Leg(flightNo, flightPlan.Origin.IcaoCode, flightPlan.Destination.IcaoCode);

            var flightPath = fileOperations.CreateFlightFolder(config.FlightsFolder, leg);
            var newFlightPlan = Path.Combine(flightPath, $"{leg.From} - {leg.To} - SimbriefImport.json");
            simbriefWriter.Write(newFlightPlan, flightPlan, true);

            downloader.DownloadXml(flightPath, config.SimbriefPilotId);
            downloader.DownloadKml(flightPath);
            downloader.DownloadMaps(flightPath);
            downloader.DownloadPdf(flightPath);

            return leg;
        }
    }
}
