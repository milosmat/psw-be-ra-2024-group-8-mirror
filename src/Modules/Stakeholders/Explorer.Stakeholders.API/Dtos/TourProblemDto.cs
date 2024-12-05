using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class TourProblemDto
    {
        public int Id { get; set; }
        public long TouristId { get; set; }
        public int TourId { get; set; }
        public long AuthorId { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime ReportedAt { get; set; }
        public bool Resolved { get; set; }
        public List<ProblemCommentDto> ProblemComments { get; set; }
        public DateTime? ResolvingDue { get; set; }
        public bool Closed { get; set; }

        public class ProblemCommentDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public long UserId { get; set; }
            public long TourProblemId { get; set; }
            public DateTime CommentedAt { get; set; }
        }
    }
}
