using System.Linq;
using System.Collections.Generic;
using StatlerWaldorfCorp.EventProcessor.Location;
using System;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    public class ProximityDetector
    {
        public ICollection<ProximityDetectedEvent> DetectProximityEvents(
            MemberLocationRecordedEvent memberLocationEvent,
            ICollection<MemberLocation> memberLocations,
            double distanceThreshold)
            {
                var gpsUtility = new GpsUtility();
                var sourceCoordinate = new GpsCoordinate{
                    Latitude = memberLocationEvent.Latitude,
                    Longitude = memberLocationEvent.Longitude
                };

                return memberLocations.Where(ml => ml.MemberId != memberLocationEvent.MemberId
                                        && gpsUtility.DistanceBetweenPoints(sourceCoordinate, ml.Location) < distanceThreshold)
                                        .Select(ml => new ProximityDetectedEvent{
                                            SourceMemberId = memberLocationEvent.MemberId,
                                            TargetMemberId = ml.MemberId,
                                            DetectionTime = DateTime.UtcNow.Ticks,
                                            SourceMemberLocation = sourceCoordinate,
                                            TargetMemberLocation = ml.Location,
                                            MemberDistance = gpsUtility.DistanceBetweenPoints(sourceCoordinate, ml.Location)
                                        }).ToList();
            }
    }


}