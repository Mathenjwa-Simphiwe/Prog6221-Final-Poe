using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEFinal
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOption { get; set; }
        public string Explanation { get; set; }
        public List<string> RelatedTopics { get; set; } = new List<string>();
    }
}
