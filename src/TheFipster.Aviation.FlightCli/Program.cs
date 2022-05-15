﻿using CommandLine;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;


//args = new [] { "simbrief", "-f", @"C:\Users\felix\Aviation\flight\KDENKTEX_XML_1652208542.xml" };
//args = new [] { "stkp", "-f", @"C:\Users\felix\Aviation\data\export.json" };
//args = new [] { "rec", "-d", "KLAS", "-a", "KLAX" };
//args = new [] { "airpt", "-i", "KTEX" };
//args = new [] { "woop" };
args = new [] { "--help" };
//args = new [] { "toolkit" };
//args = new[]
//{
//    "merge",
//    "-a", @"C:\Users\felix\Aviation\flight\KDEN - Airport.json",
//          @"C:\Users\felix\Aviation\flight\KTEX - Airport.json",
//          @"C:\Users\felix\Aviation\flight\KCOS - Airport.json",
//    "-b", @"C:\Users\felix\Aviation\flight\KDEN - KTEX - BlackBox.json",
//    "-s", @"C:\Users\felix\Aviation\flight\KDEN - KTEX - Simbrief.json",
//    "-t", @"C:\Users\felix\Aviation\flight\KDEN - KTEX - SimToolkitPro.json",
//};

try
{
    run();
    Console.WriteLine($"Finished");
}
catch (ApplicationException ex)
{
    Console.WriteLine();
    Console.WriteLine($"Whoopsie. {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine($"Exception");
    Console.WriteLine();
    Console.WriteLine(ex.GetType().Name);
    Console.WriteLine(ex.Message);
    Console.Write(ex.StackTrace);
    Console.WriteLine();
}

void run()
{
    var config = new Configuration().Load();

    Parser.Default.ParseArguments<
            AirportDetailsOptions,
            MergeOptions,
            RecorderOptions,
            SimbriefImportOptions,
            SimToolkitProImportOptions>(args)
        .WithParsed<AirportDetailsOptions>(o => { new AirportDetailsCommand().Run(o); })
        .WithParsed<MergeOptions>(o => { new MergeCommand().Run(o); })
        .WithParsed<RecorderOptions>(o => { new RecorderCommand().Run(o); })
        .WithParsed<SimbriefImportOptions>(o => { new SimbriefImportCommand().Run(o); })
        .WithParsed<SimToolkitProImportOptions>(o => { new SimToolkitProImportCommand(config).Run(o); })
        .WithNotParsed(_ => Console.Write(string.Empty));
}

