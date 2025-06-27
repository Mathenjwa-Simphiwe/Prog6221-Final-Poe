using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace POEFinal
{
    public partial class MainWindow : Window
    {
        private UserProfile _user = new UserProfile();
        private List<BotResponse> _responses;
        private ConversationContext _context = new ConversationContext();
        private List<TaskItem> _tasks = new List<TaskItem>();
        private List<string> _activityLog = new List<string>();
        private int _logIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUserProfile();
            LoadResponses();
            InitializeUI();
        }

        private void InitializeUserProfile()
        {
            _user.Name = Interaction.InputBox("Please enter your name:", "User Profile");
            if (string.IsNullOrWhiteSpace(_user.Name))
            {
                _user.Name = "Guest";
            }
            Log($"User {_user.Name} started the application.");
        }

        private void LoadResponses()
        {
            try
            {
                string path = "responses.json";
                if (!File.Exists(path))
                {
                    CreateDefaultResponsesFile(path);
                }

                string json = File.ReadAllText(path);
                _responses = JsonSerializer.Deserialize<List<BotResponse>>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading responses: {ex.Message}");
                _responses = GetDefaultResponses();
            }
        }

        private void CreateDefaultResponsesFile(string path)
        {
            var defaultResponses = GetDefaultResponses();
            string json = JsonSerializer.Serialize(defaultResponses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        private List<BotResponse> GetDefaultResponses()
        {
            return new List<BotResponse>
            {
                new BotResponse
                {
                    Keywords = new List<string> { "password", "credentials" },
                    Responses = new List<string>
                    {
                        "You should use strong passwords with a mix of uppercase, lowercase, numbers and symbols.",
                        "A good practice is to use passphrases instead of passwords. For example: 'BlueCoffeeMug42!'",
                        "Consider using a password manager to generate and store unique passwords for each service."
                    }
                },
                new BotResponse
                {
                    Keywords = new List<string> { "phishing", "scam" },
                    Responses = new List<string>
                    {
                        "Phishing is when attackers try to trick you into revealing sensitive information.",
                        "Common phishing signs: urgent requests, spelling errors, suspicious links, and unexpected attachments.",
                        "Always verify the sender's email address and hover over links before clicking."
                    }
                },
                new BotResponse
                {
                    Keywords = new List<string> { "virus", "malware" },
                    Responses = new List<string>
                    {
                        "Malware is malicious software designed to harm your computer or steal data.",
                        "To protect against malware: keep software updated, use antivirus, and don't open suspicious attachments.",
                        "Ransomware encrypts your files and demands payment. Regular backups are the best defense."
                    }
                },
                new BotResponse
                {
                    Keywords = new List<string> { "wifi", "network" },
                    Responses = new List<string>
                    {
                        "Public Wi-Fi networks can be risky. Avoid accessing sensitive information on them.",
                        "Use a VPN when connecting to public Wi-Fi to encrypt your connection.",
                        "At home, use WPA2 or WPA3 encryption for your Wi-Fi network."
                    }
                }
            };
        }

        private void InitializeUI()
        {
            AddToChat($"Welcome {_user.Name}! I can help with:");
            AddToChat("- Cybersecurity advice (ask about passwords, phishing, etc.)");
            AddToChat("- Task management (use the menu above)");
            AddToChat("- Cybersecurity quiz (use the menu above)");
        }

        private void AddToChat(string message)
        {
            ChatOutputBox.AppendText(message + "\n");
            ChatOutputBox.ScrollToEnd();
        }

        private void Log(string entry)
        {
            _activityLog.Add($"{DateTime.Now:HH:mm} - {entry}");
        }

        private void AskButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string input = InputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            AddToChat($"You: {input}");
            InputTextBox.Clear();

            string lowerInput = input.ToLower();

            // Check for follow-up requests
            if (_context.CurrentTopic != null && (lowerInput.Contains("more") || lowerInput.Contains("explain")))
            {
                HandleFollowUpRequest();
                return;
            }

            // Check for greetings
            if (IsGreeting(lowerInput))
            {
                AddToChat($"Bot: Hello {_user.Name}! How can I help you today?");
                return;
            }

            // Check for help request
            if (lowerInput.Contains("help") || lowerInput.Contains("what can you do"))
            {
                ShowHelp();
                return;
            }

            // Process cybersecurity topics
            var response = FindResponse(lowerInput);
            if (response != null)
            {
                HandleTopicResponse(response, lowerInput);
                return;
            }

            // Default response
            AddToChat("Bot: I'm not sure I understand. Could you rephrase or ask about cybersecurity topics?");
        }

        private bool IsGreeting(string input)
        {
            return input.Contains("hello") || input.Contains("hi") || input.Contains("hey");
        }

        private void ShowHelp()
        {
            AddToChat("Bot: I can help with:");
            AddToChat("- Cybersecurity advice (ask about passwords, phishing, etc.)");
            AddToChat("- Managing your tasks (click 'Add Task' or 'View Tasks')");
            AddToChat("- Testing your knowledge (click 'Start Quiz')");
        }

        private BotResponse FindResponse(string input)
        {
            foreach (var response in _responses)
            {
                if (response.Keywords.Any(keyword => input.Contains(keyword.ToLower())))
                {
                    return response;
                }
            }
            return null;
        }

        private void HandleTopicResponse(BotResponse response, string input)
        {
            string matchedKeyword = response.Keywords.First(keyword => input.Contains(keyword.ToLower()));
            string topic = response.Keywords[0];

            _context.CurrentTopic = topic;
            _context.DetailLevel = 1;
            _user.AddInterest(topic);

            string greeting = GetPersonalizedGreeting();
            string responseText = response.Responses[0];

            AddToChat($"Bot: {greeting}{responseText}");
            Log($"Detected keyword: {matchedKeyword} (Topic: {topic})");
        }

        private void HandleFollowUpRequest()
        {
            var match = _responses.FirstOrDefault(r => r.Keywords.Contains(_context.CurrentTopic));
            if (match != null && _context.DetailLevel < match.Responses.Count)
            {
                string response = match.Responses[_context.DetailLevel];
                AddToChat($"Bot: {response}");
                _context.DetailLevel++;
            }
            else
            {
                AddToChat("Bot: I've shared all I know on this topic. Would you like to ask about something else?");
                _context.CurrentTopic = null;
            }
        }

        private string GetPersonalizedGreeting()
        {
            if (_user.Interests.Count == 0) return $"{_user.Name}, here's what I know: ";
            if (_user.Interests.Count == 1) return $"{_user.Name}, since you're interested in {_user.Interests[0]}, you should know: ";
            return $"{_user.Name}, as someone interested in {string.Join(" and ", _user.Interests)}, ";
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = Interaction.InputBox("Enter task title:", "Add New Task");
            if (string.IsNullOrWhiteSpace(title)) return;

            string description = Interaction.InputBox("Enter task description:", "Task Details");
            string dateInput = Interaction.InputBox("Enter reminder date (yyyy-mm-dd hh:mm) or leave blank:", "Set Reminder");
            DateTime? reminder = null;

            if (DateTime.TryParse(dateInput, out DateTime parsed))
            {
                reminder = parsed;
                AddToChat($"Bot: I've set a reminder for {reminder:g}");
            }

            _tasks.Add(new TaskItem { Title = title, Description = description, ReminderDate = reminder });
            AddToChat($"Bot: Task '{title}' added successfully.");
            Log($"Task created: {title}");
        }

        private void ViewTasksButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tasks.Count == 0)
            {
                AddToChat("Bot: You don't have any tasks yet.");
                return;
            }

            AddToChat("\nBot: Here are your tasks:");
            for (int i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks[i];
                string status = task.IsCompleted ? "[✔]" : "[ ]";
                string reminderText = task.ReminderDate.HasValue ? $" (Reminder: {task.ReminderDate:g})" : "";
                AddToChat($"{i + 1}. {status} {task.Title} - {task.Description}{reminderText}");
            }

            string action = Interaction.InputBox("Type 'done X' to mark task X complete or 'delete X' to remove task X.\nLeave blank to cancel.", "Manage Tasks");
            if (string.IsNullOrWhiteSpace(action)) return;

            ProcessTaskAction(action);
        }

        private void ProcessTaskAction(string action)
        {
            try
            {
                string[] parts = action.Split(' ');
                if (parts.Length != 2) return;

                string command = parts[0].ToLower();
                if (!int.TryParse(parts[1], out int taskIndex) || taskIndex < 1 || taskIndex > _tasks.Count)
                {
                    AddToChat("Bot: Invalid task number.");
                    return;
                }

                var task = _tasks[taskIndex - 1];

                switch (command)
                {
                    case "done":
                        task.IsCompleted = true;
                        AddToChat($"Bot: Task '{task.Title}' marked as completed.");
                        Log($"Completed task: {task.Title}");
                        break;
                    case "delete":
                        _tasks.RemoveAt(taskIndex - 1);
                        AddToChat($"Bot: Task '{task.Title}' deleted.");
                        Log($"Deleted task: {task.Title}");
                        break;
                    default:
                        AddToChat("Bot: Invalid command. Use 'done X' or 'delete X'.");
                        break;
                }
            }
            catch
            {
                AddToChat("Bot: Couldn't process your request. Please try again.");
            }
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Check user's interests to personalize quiz
            if (_user.Interests.Count > 0)
            {
                AddToChat($"Bot: I see you're interested in {string.Join(", ", _user.Interests)}. " +
                         "I'll include related questions in the quiz.");
            }

            var questions = QuizHelper.GetQuestions(_user.Interests);
            int score = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                string prompt = $"\nQuestion {i + 1}/{questions.Count}:\n{q.Question}\n";

                for (int j = 0; j < q.Options.Count; j++)
                {
                    prompt += $"{j + 1}. {q.Options[j]}\n";
                }

                string answer = Interaction.InputBox(prompt, $"Question {i + 1}");
                if (int.TryParse(answer, out int chosen) && chosen == q.CorrectOption + 1)
                {
                    AddToChat($"✅ Correct! {q.Explanation}");
                    score++;
                }
                else
                {
                    AddToChat($"❌ Incorrect. {q.Explanation}");
                }
            }

            string result = $"\nQuiz Complete! You scored {score}/{questions.Count}.\n";
            result += score >= questions.Count * 0.8 ? "Excellent! " :
                      score >= questions.Count * 0.5 ? "Good effort! " : "Keep learning! ";

            if (_user.Interests.Count > 0)
            {
                result += $"\nBased on your interests, you might want to learn more about: {string.Join(", ", _user.Interests)}";
            }

            AddToChat($"Bot: {result}");
            Log($"Quiz attempted. Score: {score}/{questions.Count}");
        }

        private void ViewLogButton_Click(object sender, RoutedEventArgs e)
        {
            _logIndex = 0;
            ShowLogBatch();
        }

        private void ShowLogBatch()
        {
            var batch = _activityLog.Skip(_logIndex).Take(5).ToList();
            if (batch.Count == 0)
            {
                AddToChat("Bot: No more log entries to show.");
                return;
            }

            AddToChat("\nActivity Log:");
            foreach (var entry in batch)
            {
                AddToChat($"- {entry}");
            }

            _logIndex += batch.Count;

            if (_logIndex >= _activityLog.Count)
            {
                AddToChat("(End of log)");
            }
        }
    }
}