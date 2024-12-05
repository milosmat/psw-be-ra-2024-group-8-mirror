using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.ValueObjects
{
    public class DailyAgenda : ValueObject<DailyAgenda>
    {
        [JsonPropertyName("Day")]
        public int Day { get; set; }

        [JsonPropertyName("StartDestination")]
        public string StartDestination { get; set; }

        [JsonPropertyName("BetweenDestinations")]
        public string[] BetweenDestinations { get; set; }

        [JsonPropertyName("EndDestination")]
        public string EndDestination { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        public DailyAgenda() { }

        [JsonConstructor]
        public DailyAgenda(int day, string startDestination, string[] betweenDestinations, string endDestination,
            string description)
        {
            this.Day = day;
            this.StartDestination = startDestination;
            this.EndDestination = endDestination;
            this.Description = description;
            this.BetweenDestinations = betweenDestinations;
            this.Description = description;
        }

        protected override bool EqualsCore(DailyAgenda other)
        {
            return this.Day == other.Day && this.StartDestination == other.StartDestination &&
                this.Description == other.Description && this.BetweenDestinations == other.BetweenDestinations &&
                this.EndDestination == other.EndDestination;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Day.GetHashCode();
                hash = hash * 23 + StartDestination.GetHashCode();
                hash = hash * 23 + BetweenDestinations.GetHashCode();
                hash = hash * 23 + EndDestination.GetHashCode();
                hash = hash * 23 + Description.GetHashCode();
                return hash;
            }
        }
    }
}
