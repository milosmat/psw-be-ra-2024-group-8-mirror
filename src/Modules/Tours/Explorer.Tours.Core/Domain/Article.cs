using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class Article : Entity
    {
        public long TourId { get; private set; }
        public long AuthorId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public bool IsPublished { get; private set; }
        public string TourDescription { get; private set; }
        public string Weight { get; private set; }
        public string[] Tags { get; private set; }
        public decimal? Price { get; private set; }
        public long LengthInKm { get; private set; }
        public List<string> Checkpoints { get; private set; } = new List<string>();
        public List<string> EquipmentList { get; private set; } = new List<string>();

        public Article(long tourId, long authorId, string title, string content,
                       string tourDescription, string weight, string[] tags,
                       decimal? price, long lengthInKm, List<string> checkpoints,
                       List<string> equipmentList, bool isPublished = false)
        {
            TourId = tourId;
            AuthorId = authorId;
            Title = title;
            Content = content;
            TourDescription = tourDescription;
            Weight = weight;
            Tags = tags;
            Price = price;
            LengthInKm = lengthInKm;
            Checkpoints = checkpoints;
            EquipmentList = equipmentList;
            IsPublished = isPublished;
        }


        public void Publish()
        {
            IsPublished = true; 
        }


        public void Update(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
