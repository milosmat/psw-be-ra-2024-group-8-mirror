using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class AppRating : Entity
    {
        public int Rating { get; private set; }
        public string ?Comment { get; private set; }
        public DateTime TimeCreated {  get; private set; }
        public User UserPosted {  get; private set; }
        public int UserPostedId {  get; private set; }

        public AppRating(int rating, string comment, DateTime timeCreated, int userPosted)
        {
            Rating = rating;
            Comment = comment;
            UserPostedId = userPosted;
            TimeCreated = timeCreated;
        }


    }
}
