using Xunit;
using StatlerWaldorfCorp.EventProcessor.Location;
using System;

namespace StatlerWaldorfCorp.EventProcessor.Tests.Location
{
    public class GpsUtilityTests
    {
        [Fact]
        public void ProducesAccurateDistanceMeasurements()
        {
            var gpsUtility = new GpsUtility();
            var losAngelesCoordinate = new GpsCoordinate{
                Latitude = 34.0522222,
                Longitude = -118.2427778
            };

            var newYorkCoordinate = new GpsCoordinate{
                Latitude = 40.7141667,
                Longitude = -74.0063889
            };

            var distance = gpsUtility.DistanceBetweenPoints(losAngelesCoordinate, newYorkCoordinate);

            Assert.Equal(3933, Math.Round(distance));
            Assert.Equal(0, gpsUtility.DistanceBetweenPoints(losAngelesCoordinate, losAngelesCoordinate));
        }
    }
}