﻿using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Uses the chart pdf files to generate png images.
    /// </summary>
    internal class ChartCommand
    {
        private HardcodedConfig config;

        public ChartCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(ChartOptions options)
        {
            Console.WriteLine("Converting chart pdfs into pngs.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                var charts = new FlightFileScanner().GetFiles(folder, Domain.Enums.FileTypes.Chart);
                foreach (var chart in charts)
                {
                    Console.WriteLine($"\t\t {Path.GetFileName(chart)}");
                    new PdfConverter().ToImage(chart);
                }
            }
        }
    }
}