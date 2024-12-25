using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class ArticleDTO
    {
        public int Id { get; set; } // Ovdje može biti ID članka, ukoliko postoji
        public long TourId { get; set; }
        public long AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public string TourDescription { get; set; }
        public string Weight { get; set; }
        public string[] Tags { get; set; }
        public decimal? Price { get; set; }
        public long LengthInKm { get; set; }
        public List<string> Checkpoints { get; set; } = new List<string>();
        public List<string> EquipmentList { get; set; } = new List<string>();

    }
}
