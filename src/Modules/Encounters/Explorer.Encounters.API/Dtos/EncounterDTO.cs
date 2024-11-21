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
        public string Location { get; set; } // Location of the encounter
        public int XP { get; set; } // Experience points rewarded
        public EncounterStatus Status { get; set; } // Enum representing the status of the encounter
        public EncounterType Type { get; set; } // Enum representing the type of the encounter
        public DateTime? PublishedDate { get; set; } // Date when the encounter was published
        public DateTime? ArchivedDate { get; set; } // Date when the encounter was archived
        public long AuthorId { get; set; } // ID of the administrator who created the encounter
    }

    public enum EncounterStatus
    {
        DRAFT,
        ACTIVE,
        ARCHIVED
    }

    public enum EncounterType
    {
        SOCIAL,
        LOCATION,
        MISC
    }
}
