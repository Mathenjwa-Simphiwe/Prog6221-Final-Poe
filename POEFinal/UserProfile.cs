using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEFinal
{
    internal class UserProfile
    {
        public string Name { get; set; }
        public List<string> Interests { get; } = new List<string>();

        public void AddInterest(string interest)
        {
            if (!Interests.Contains(interest))
            {
                Interests.Add(interest);
            }
        }
    }
}
