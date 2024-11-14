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
        [JsonPropertyName("Time")]
        public int Time { get; set; }

        [JsonPropertyName("TransportType")]
        public TransportType TransportType { get; set; }

        public TravelTime() { }

        [JsonConstructor]
        public TravelTime(int time, TransportType transportType) {
            this.Time = time;
            this.TransportType = transportType;
        }
        protected override bool EqualsCore(TravelTime other)
        {
            return this.Time == other.Time && this.TransportType == other.TransportType;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Time.GetHashCode();
                hash = hash * 23 + TransportType.GetHashCode();
                return hash;
            }
        }
        
    }
    public enum TransportType
    {
        WALK = 1, BIKE = 2, CAR = 3
    }
}
