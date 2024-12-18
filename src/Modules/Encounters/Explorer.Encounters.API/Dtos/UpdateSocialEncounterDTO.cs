using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class UpdateSocialEncounterDTO
    {
        public int RequiredParticipants { get; set; }
        public int Radius { get; set; }

        public UpdateSocialEncounterDTO() { }
    }
}
