﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class ScanCommand
    {
        private HardcodedConfig config;

        public ScanCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(ScanOptions _)
        {
            var flights = Scan();
            print(flights);
        }

        public Dictionary<string, Dictionary<string, FileTypes>> Scan()
        {
            var flights = new Dictionary<string, Dictionary<string, FileTypes>>();
            var flightFolders = Directory.GetDirectories(config.FlightsFolder).OrderBy(x => x);

            foreach (var flightFolder in flightFolders)
            {
                var flightName = Path.GetFileName(flightFolder);
                var flight = new FileSystemFinder().GetFiles(flightFolder);
                flights.Add(flightName, flight);
            }

            return flights;
        }

        private void print(Dictionary<string, Dictionary<string, FileTypes>> flights)
        {
            foreach (var flight in flights)
            {
                var folder = Path.GetFileName(flight.Key);
                Console.WriteLine(folder);
                Console.WriteLine();
                foreach (var file in flight.Value.OrderBy(x => x.Key))
                {
                    switch (file.Value)
                    {
                        case FileTypes.Unknown:
                        case FileTypes.Error:
                        case FileTypes.Chart:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case FileTypes.SimbriefXml:
                        case FileTypes.SimbriefJson:
                        case FileTypes.OfpPdf:
                        case FileTypes.RouteKml:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case FileTypes.NotamJson:
                        case FileTypes.WaypointsJson:
                        case FileTypes.RouteJson:
                        case FileTypes.OfpHtml:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case FileTypes.SimToolkitProJson:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case FileTypes.TrackJson:
                        case FileTypes.LandingJson:
                        case FileTypes.LogbookJson:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case FileTypes.ChartAirport:
                        case FileTypes.ChartApproach:
                        case FileTypes.ChartArrival:
                        case FileTypes.ChartDeparture:
                        case FileTypes.ChartParking:
                        case FileTypes.ChartTaxi:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case FileTypes.MsfsFlightPlan:
                        case FileTypes.PathKml:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case FileTypes.Screenshot:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case FileTypes.AirportJson:
                        case FileTypes.MergedFlightJson:
                        case FileTypes.BlackBoxCsv:
                        case FileTypes.BlackBoxJson:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.WriteLine($"\t{file.Value.ToString().PadRight(20)} - {file.Key}");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
