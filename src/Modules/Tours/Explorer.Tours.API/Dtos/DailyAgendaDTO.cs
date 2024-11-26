using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class DailyAgendaDTO
    {
        public int Day { get; set; }
        public string StartDestination { get; set; }      
        public string[] BetweenDestinations { get; set; }        
        public string EndDestination { get; set; }       
        public string Description { get; set; }
    }
}
