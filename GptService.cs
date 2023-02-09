using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace gptAzureCognitive
{
    class GptService
    {
        public static async Task<string> SpeakWithIA(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
                return prompt;

            // Sua chave API
            string apiKey = "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";

            // O modelo que você deseja usar
            string model = "text-davinci-003";

            // Instância de um objeto HttpClient
            var client = new HttpClient();

            // Adicione a chave API às configurações do cabeçalho da solicitação
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Crie o corpo da solicitação
            var request = new
            {
                model,
                prompt,
                max_tokens = 2048,
                n = 1,
                stop = "",
                temperature = 0.5
            };

            // Faça a solicitação à API
            var requestJson = JsonConvert.SerializeObject(request);
            var response = await client.PostAsync("https://api.openai.com/v1/completions",
                            new StringContent(requestJson, Encoding.UTF8, "application/json"));

            // Verifique se a solicitação foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                // Obtenha a resposta da API
                var responseString = await response.Content.ReadAsStringAsync();

                // Analise a resposta como JSON
                var responseJson = JObject.Parse(responseString);

                // Obtenha o texto da resposta
                var responseText = Regex.Replace(responseJson["choices"][0]["text"].ToString(), @"\t|\n|\r", string.Empty);

                return responseText;
            }
            else
            {
                // Imprima o código de status da resposta
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());

                return "desculpe acho que buguei";
            }
        }
    }
}
