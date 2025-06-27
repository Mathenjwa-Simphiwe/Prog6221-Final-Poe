using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace POEFinal
{
    public class BotResponse
    {
        [JsonPropertyName("keywords")]
        public List<string> Keywords { get; set; } = new List<string>();

        [JsonPropertyName("responses")]
        public List<string> Responses { get; set; } = new List<string>();
    }
}
