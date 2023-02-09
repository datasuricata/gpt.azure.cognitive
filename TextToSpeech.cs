using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace gptAzureCognitive
{
    class TextToSpeech
    {
        public static async Task RecognizeAndOutputAsync(string sintax)
        {
            if (string.IsNullOrEmpty(sintax))
                return;

            var speechConfig = SpeechConfig.FromSubscription("4585a7bef0cb4df7b9ac31e382aaca51", "eastus");
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
