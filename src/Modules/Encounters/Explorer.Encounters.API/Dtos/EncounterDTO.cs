using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterDTO
    {
        public long Id { get; set; } // Unique identifier for the encounter
        public string Name { get; set; } // Name of the encounter
        public string Description { get; set; } // Description of what the user needs to do
        public MapLocationDTO Location { get; set; } // Location of the encounter
        public int XP { get; set; } // Experience points rewarded
        public string Status { get; set; } // Accepts string (DRAFT, ACTIVE, ARCHIVED)
        public string Type { get; set; } // Accepts string (SOCIAL, LOCATION, MISC)
        public DateTime? PublishedDate { get; set; } // Date when the encounter was published
        public DateTime? ArchivedDate { get; set; } // Date when the encounter was archived
        public long AuthorId { get; set; } // ID of the administrator who created the encounter
        public string? Image {  get; set; }
        public List<long>? UsersWhoCompletedId {  get; set; }
        public bool? IsRequired { get; set; }
        public class MapLocationDTO
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

    }

}
