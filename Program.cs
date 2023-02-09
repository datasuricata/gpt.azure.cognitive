using gptAzureCognitive;

while (true)
{
    var translated = await SpeechToText.RecognizeAsync();
    var categories = TextAnalytic.EntityStandartRecognition(translated);
    var answer = await GptService.SpeakWithIA(translated, categories);
    await TextToSpeech.RecognizeAndOutputAsync(answer);
}