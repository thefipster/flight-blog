﻿using System.Globalization;
using System.Text;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;

namespace TheFipster.Aviation.Modules.BlackBox.Components
{
    public class BlackBoxCsvWriter
    {
        public void Write(string flightFolder, BlackBoxFlight flight, FileTypes filetype, bool overwrite = false)
        {
            string filetypeName = filetype switch
            {
                FileTypes.BlackBoxCsv => "BlackBox",
                _ => throw new UnknownFileTypeException($"Unknown csv file type {filetype}.")
            };

            var file = $"{flight.Origin} - {flight.Destination} - {filetypeName}.csv";

            var path = Path.Combine(flightFolder, file);

            if (File.Exists(path) && !overwrite)
                return;

            var sb = new StringBuilder();
            sb.AppendLine(Const.BlackBoxHeader);

            foreach (var record in flight.Records)
            {
                var line = makeLine(record);
                sb.AppendLine(line);
            }

            var text = sb.ToString();
            File.WriteAllText(path, text);
        }

        private string makeLine(Record record)
        {
            var sb = new StringBuilder();

            sb.Append(record.Timestamp);
            sb.Append(";");
            sb.Append(record.AltimeterFeet);
            sb.Append(";");
            sb.Append(record.BankAngle.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.BrakesActivated);
            sb.Append(";");
            sb.Append(record.CompassHeadingRadians);
            sb.Append(";");
            sb.Append(record.ElevationMeters);
            sb.Append(";");
            sb.Append(record.Engine1N1Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.Engine1N2Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.Engine2N1Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.Engine2N2Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.FlapsConfig);
            sb.Append(";");
            sb.Append(record.FuelLiters);
            sb.Append(";");
            sb.Append(record.GearPosition);
            sb.Append(";");
            sb.Append(record.GpsAltitudeMeters);
            sb.Append(";");
            sb.Append(record.GroundSpeedMps);
            sb.Append(";");
            sb.Append(record.IndicatedAirSpeedKnots);
            sb.Append(";");
            sb.Append(record.LatitudeDecimals.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.LongitudeDecimals.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.OnGroundFlag);
            sb.Append(";");
            sb.Append(record.OutsideAirTemperatureCelsius);
            sb.Append(";");
            sb.Append(record.PitchAngle.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.RadioAltimeterMeters);
            sb.Append(";");
            sb.Append(record.TotalAirTemperatureCelsius);
            sb.Append(";");
            sb.Append(record.TrueAirSpeedKnots);
            sb.Append(";");
            sb.Append(record.VerticalSpeedMps);
            sb.Append(";");
            sb.Append(record.WindDirectionRadians);
            sb.Append(";");
            sb.Append(record.WindSpeedKnots);

            return sb.ToString();
        }
    }
}
