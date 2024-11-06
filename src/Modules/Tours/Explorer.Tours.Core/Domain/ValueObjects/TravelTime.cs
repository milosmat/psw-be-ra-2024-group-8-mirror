using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.ValueObjects
{
    public class TravelTime : ValueObject<TravelTime>
    {
        public int Time { get; set; }
        public TransportType TransportType { get; set; }

        [JsonConstructor]
        public TravelTime(int time, TransportType transportType) {
            this.Time = time;
            this.TransportType = transportType;
        }
        protected override bool EqualsCore(TravelTime other)
        {
            throw new NotImplementedException();
        }

        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }
    }
    public enum TransportType
    {
        WALK, BIKE, CAR
    }
}
