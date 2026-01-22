using Google.GenAI;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace API.Services;


    public sealed class OpenAiPriorityService
    {
        private readonly Client _client;
        private readonly string _model;

        public OpenAiPriorityService(IConfiguration config)
        {
        // Client picks up API key from GEMINI_API_KEY or GOOGLE_API_KEY env vars automatically. :contentReference[oaicite:1]{index=1}
        // Ensure the key is present (library reads GOOGLE_API_KEY env var)
        var key = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("GOOGLE_API_KEY is missing. Set it in launchSettings.json or as a system env var.");

        _client = new Client(); // library uses GOOGLE_API_KEY internally
        _model = config["GEMINI_MODEL"] ?? "gemini-3-flash-preview";

        // You can override via appsettings if you want
        _model = config["GEMINI_MODEL"] ?? "gemini-3-flash-preview";
        }

        public async Task<string> PredictPriorityAsync(string title, string description, DateTime? dueDate)
        {
            var prompt = BuildPrompt(title, description, dueDate);

            // Retry with exponential backoff for transient errors
            var delays = new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4) };

            for (int attempt = 0; attempt <= delays.Length; attempt++)
            {
                try
                {
                    var response = await _client.Models.GenerateContentAsync(
                        model: _model,
                        contents: prompt
                    );

                    var text = response?.Candidates?
                        .FirstOrDefault()?
                        .Content?
                        .Parts?
                        .FirstOrDefault()?
                        .Text ?? string.Empty;

                    var normalized = NormalizePriority(text);
                    return normalized ?? "Medium";
                }
                catch (Exception) when (attempt < delays.Length)
                {
                    await Task.Delay(delays[attempt]);
                }
            }

            return "Medium";
        }

        private static string BuildPrompt(string title, string description, DateTime? dueDate)
        {
            var due = dueDate.HasValue ? dueDate.Value.ToString("yyyy-MM-dd") : "None";

            // Force 1-word output and give crisp rules
            var sb = new StringBuilder();
            sb.AppendLine("You are a strict classifier.");
            sb.AppendLine("Classify the task priority as exactly one word: High, Medium, or Low.");
            sb.AppendLine("Return ONLY that one word. No punctuation. No extra text.");
            sb.AppendLine();
            sb.AppendLine($"Title: {title}");
            sb.AppendLine($"Description: {description}");
            sb.AppendLine($"DueDate: {due}");
            sb.AppendLine();
            sb.AppendLine("Priority:");
            return sb.ToString();
        }

        private static string? NormalizePriority(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            var t = raw.Trim().ToLowerInvariant();

            if (t.StartsWith("high")) return "High";
            if (t.StartsWith("medium") || t.StartsWith("mid")) return "Medium";
            if (t.StartsWith("low")) return "Low";

            return null;
        }
    }

