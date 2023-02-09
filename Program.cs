using gptAzureCognitive;

while (true)
{
    var translated = await SpeechToText.RecognizeAsync();
    var answer = await GptService.SpeakWithIA(translated);
    await TextToSpeech.RecognizeAsync(answer);
}