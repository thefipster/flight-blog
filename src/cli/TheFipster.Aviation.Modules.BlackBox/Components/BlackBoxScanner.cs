﻿using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.BlackBox;
using System.ComponentModel.DataAnnotations;

namespace TheFipster.Aviation.Modules.BlackBox.Components
{
    public class BlackBoxScanner
    {
        public BlackBoxStats GenerateStatsFromBlackbox(ICollection<Record> records)
        {
            var items = records.ToList();
            var stats = new BlackBoxStats();

            extractRecords(items, stats);
            // must run after extractRecords since it uses the record values to determine where they happened
            // on the first pass we don't know when we encounter a maximum
            extractWaypoints(items, stats);

            return stats;
        }

        private static void extractWaypoints(List<Record> records, BlackBoxStats stats)
        {
            bool tocTrigger = true;
            bool gsTrigger = true;
            bool climbTrigger = true;
            bool descentTrigger = true;
            bool windTrigger = true;

            for (int i = 0; i < records.Count; i++)
            {
                var rec = records[i];
                if (tocTrigger && stats.MaxAltitudeM * 0.999 < rec.GpsAltitudeMeters)
                {
                    var waypoint = new Waypoint(FlightEvents.RealToc, rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    tocTrigger = false;
                    stats.TimeTable.Add(new TimeTable(rec.Timestamp, TimeEvents.TopOfClimb));
                }

                if (gsTrigger && rec.GroundSpeedMps == stats.MaxGroundSpeedMps)
                {
                    var waypoint = new Waypoint(FlightEvents.MaxSpeed, rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    gsTrigger = false;
                }

                if (climbTrigger && rec.VerticalSpeedMps == stats.MaxClimbMps)
                {
                    var waypoint = new Waypoint(FlightEvents.MaxClimb, rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    climbTrigger = false;
                }

                if (descentTrigger && rec.VerticalSpeedMps == stats.MaxDescentMps)
                {
                    var waypoint = new Waypoint(FlightEvents.MaxDescent, rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    descentTrigger = false;
                }

                var windspeed = UnitConverter.KtsToMps(rec.WindSpeedKnots);
                if (windTrigger && windspeed == stats.MaxWindspeedMps)
                {
                    var waypoint = new Waypoint(FlightEvents.MaxWind, rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    windTrigger = false;
                }
            }
        }

        private static void extractRecords(List<Record> records, BlackBoxStats stats)
        {
            bool above = true;
            bool below = true;
            bool engineStart = true;
            bool leftRamp = true;
            bool reachedRamp = true;

            var firstRec = records.First();
            var lastRec = records.Last();

            for (int i = 1; i < records.Count; i++)
            {
                var last = records[i - 1];
                var cur = records[i];

                if (last.FlapsConfig != cur.FlapsConfig)
                {
                    var waypoint = new Waypoint($"{FlightEvents.Flaps} {last.FlapsConfig}->{cur.FlapsConfig}", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                }

                if (engineStart && cur.Engine2N1Percent > 0)
                {
                    engineStart = false;
                    stats.TimeTable.Add(new TimeTable(cur.Timestamp, TimeEvents.EngineStart));
                }

                var kmFromStart = GpsCalculator.GetHaversineDistance(firstRec.LatitudeDecimals, firstRec.LongitudeDecimals, cur.LatitudeDecimals, cur.LongitudeDecimals);
                if (leftRamp && kmFromStart > 0.05)
                {
                    leftRamp = false;
                    stats.TimeTable.Add(new TimeTable(cur.Timestamp, TimeEvents.StartRolling));
                }

                var kmFromParking = GpsCalculator.GetHaversineDistance(lastRec.LatitudeDecimals, lastRec.LongitudeDecimals, cur.LatitudeDecimals, cur.LongitudeDecimals);
                if (reachedRamp && kmFromParking < 0.05)
                {
                    reachedRamp = false;
                    stats.TimeTable.Add(new TimeTable(cur.Timestamp, TimeEvents.ParkPosition));
                }

                if (last.GearPosition != cur.GearPosition)
                {
                    var waypoint = new Waypoint($"{FlightEvents.Gear} {cur.GearPosition}", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                }

                if (last.OnGroundFlag != cur.OnGroundFlag)
                {
                    if (cur.OnGroundFlag)
                    {
                        // Landing
                        var waypoint = new Waypoint(FlightEvents.Landing, cur.LatitudeDecimals, cur.LongitudeDecimals);
                        stats.Waypoints.Add(waypoint);
                        stats.TouchdownTime = cur.Timestamp;
                        stats.TimeTable.Add(new TimeTable(cur.Timestamp, TimeEvents.Touchdown));
                    }
                    else
                    {
                        // Takeoff
                        var waypoint = new Waypoint(FlightEvents.Takeoff, cur.LatitudeDecimals, cur.LongitudeDecimals);
                        stats.Waypoints.Add(waypoint);
                        stats.TakeoffTime = cur.Timestamp;
                        stats.TimeTable.Add(new TimeTable(cur.Timestamp, TimeEvents.Takeoff));
                    }
                }

                if (above && last.AltimeterFeet < 10000 && cur.AltimeterFeet >= 10000)
                {
                    var waypoint = new Waypoint(FlightEvents.Above10000, cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    above = false;
                }

                if (below && last.AltimeterFeet >= 10000 && cur.AltimeterFeet < 10000)
                {
                    var waypoint = new Waypoint(FlightEvents.Below10000, cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    below = false;
                }

                var trip = GpsCalculator.GetHaversineDistance(last.LatitudeDecimals, last.LongitudeDecimals, cur.LatitudeDecimals, cur.LongitudeDecimals);
                if (last.OnGroundFlag)
                    stats.DistanceGround += trip;
                else
                    stats.DistanceAir += trip;

                if (cur.AltimeterFeet < 10000 && cur.IndicatedAirSpeedKnots > 250)
                    stats.Below10000SpeedWarning = true;

                if (cur.GpsAltitudeMeters > stats.MaxAltitudeM)
                    stats.MaxAltitudeM = cur.GpsAltitudeMeters;

                if (cur.GroundSpeedMps > stats.MaxGroundSpeedMps)
                    stats.MaxGroundSpeedMps = cur.GroundSpeedMps;

                if (cur.VerticalSpeedMps > stats.MaxClimbMps)
                    stats.MaxClimbMps = cur.VerticalSpeedMps;

                if (cur.VerticalSpeedMps < stats.MaxDescentMps)
                    stats.MaxDescentMps = cur.VerticalSpeedMps;

                if (cur.FuelLiters > stats.MaxFuel)
                    stats.MaxFuel = cur.FuelLiters;

                stats.ShutdownFuel = cur.FuelLiters;

                var windspeed = UnitConverter.KtsToMps(cur.WindSpeedKnots);
                if (windspeed > stats.MaxWindspeedMps)
                {
                    stats.MaxWindspeedMps = windspeed;
                    stats.WindDirectionRad = cur.WindDirectionRadians;
                }
            }

            stats.MaxFuel = (int)Math.Round(UnitConverter.JetA1LToKg(stats.MaxFuel));
            stats.ShutdownFuel = (int)Math.Round(UnitConverter.JetA1LToKg(stats.ShutdownFuel));
        }
    }
}
