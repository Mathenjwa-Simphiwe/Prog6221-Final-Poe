using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEFinal
{
    public static class QuizHelper
    {
        public static List<QuizQuestion> GetQuestions(List<string> userInterests = null)
        {
            var allQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What is the minimum recommended length for a secure password?",
                    Options = new List<string> { "6 characters", "8 characters", "12 characters", "16 characters" },
                    CorrectOption = 2,
                    Explanation = "12 characters is the current recommended minimum length for secure passwords.",
                    RelatedTopics = new List<string> { "password" }
                },
                new QuizQuestion
                {
                    Question = "Which of these is NOT a common cyber attack?",
                    Options = new List<string> { "Phishing", "SQL Injection", "DDoS", "TCP Handshake" },
                    CorrectOption = 3,
                    Explanation = "TCP Handshake is a normal network communication process.",
                    RelatedTopics = new List<string> { "phishing", "network" }
                },
                // Add more questions...
            };

            if (userInterests == null || userInterests.Count == 0)
                return allQuestions;

            return allQuestions
                .OrderByDescending(q => q.RelatedTopics.Intersect(userInterests).Count())
                .ThenBy(q => Guid.NewGuid())
                .ToList();
        }
    }
}
