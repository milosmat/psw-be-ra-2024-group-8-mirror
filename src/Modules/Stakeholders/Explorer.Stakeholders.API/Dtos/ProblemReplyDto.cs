using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProblemReplyDto
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int ProblemId { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string Text { get; private set; }
    }
}
