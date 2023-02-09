using Microsoft.CognitiveServices.Speech;

namespace gptAzureCognitive
{
    class TextToSpeech
    {
        public static async Task RecognizeAsync(string sintax)
        {
            if (string.IsNullOrEmpty(sintax))
                return;

            var speechConfig = SpeechConfig.FromSubscription("24327e61058648f782d48aae31c4d6c3", "eastus");
            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "pt-BR-FabioNeural";
            using var speechSynthesizer = new SpeechSynthesizer(speechConfig);
            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(sintax);
            OutputSpeechSynthesisResult(speechSynthesisResult, sintax);
        }

        private static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
