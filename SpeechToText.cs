using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Threading.Tasks;

namespace gptAzureCognitive
{
    class SpeechToText
    {
        public static async Task<string> RecognizeAsync()
        {
            var speechConfig = SpeechConfig.FromSubscription("4585a7bef0cb4df7b9ac31e382aaca51", "eastus");
            speechConfig.SpeechRecognitionLanguage = "pt-BR";

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);



            Console.WriteLine("aguardando fala...");
            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            return OutputSpeechRecognitionResult(speechRecognitionResult);
        }

        private static string OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
        {
            var response = string.Empty;

            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                    response = speechRecognitionResult.Text;
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
            }

            return response;
        }
    }
}
