using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace gptAzureCognitive
{
    class GptService
    {
        private static List<string> davinciCategories = new List<string>
        {
            "Event",
            "Organization",
            "Skill",
            "Location"
        };

        public static async Task<string> SpeakWithIA(string prompt, List<string> categories)
        {
            if (string.IsNullOrEmpty(prompt))
                return prompt;

            string apiKey = "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";
            string model = "text-curie-001";
            int max_tokens = 1048;

            if (categories.Count > 0)
            {
                var hasCategories = davinciCategories.Any(a => categories.Contains(a));
                model = hasCategories ? "text-davinci-003" : "text-curie-001";
                max_tokens = hasCategories ? 2048 : 1048;
            }
            else
            {
                var hasBigSearch = prompt.Length > 250;
                model = hasBigSearch ? "text-davinci-003" : "text-curie-001";
                max_tokens = hasBigSearch ? 2048 : 1048;
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var request = new
            {
                model,
                prompt,
                max_tokens,
                n = 1,
                stop = "",
                temperature = 0.5
            };

            try
            {
                var requestJson = JsonConvert.SerializeObject(request);
                var response = await client.PostAsync("https://api.openai.com/v1/completions",
                                new StringContent(requestJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseJson = JObject.Parse(responseString);
                    
                    // Obtenha o texto da resposta
                    var responseText = Regex.Replace(responseJson["choices"][0]["text"].ToString(), @"\t|\n|\r", string.Empty);

                    return responseText;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(await response.Content.ReadAsStringAsync());

                    return "desculpe, acho que dei uma bugada";
                }
            }
            catch (Exception)
            {
                return "não conseguir localizar nada no momento, volte mais tarde minha agenda já esta lotada";
            }
        }
    }
}
