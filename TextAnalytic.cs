using Azure;
using Azure.AI.TextAnalytics;

namespace gptAzureCognitive
{
    class TextAnalytic
    {
        public static List<string> EntityStandartRecognition(string text)
        {
            List<string> categories = new();

            if (!string.IsNullOrEmpty(text))
                return categories;

            AzureKeyCredential credentials = new AzureKeyCredential("c18fb0a403fb4d15b67ae3ca64915e76");
            Uri endpoint = new Uri("https://gpt-analytics-cognitive.cognitiveservices.azure.com/");

            var client = new TextAnalyticsClient(endpoint, credentials);
            var response = client.RecognizeEntities(text);

            if (response.Value.Count > 0)
            {
                Console.WriteLine("Named Entities NER:");
                foreach (var entity in response.Value)
                {
                    if (!string.IsNullOrEmpty(entity.Category.ToString()))
                        categories.Add(entity.Category.ToString());

                    Console.WriteLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
                    Console.WriteLine($"\t\tScore: {entity.ConfidenceScore:F2},\tLength: {entity.Length},\tOffset: {entity.Offset}\n");
                    Console.WriteLine();
                }
            }

            return categories;
        }
    }
}
