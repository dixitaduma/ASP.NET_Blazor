using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "Your Api key";

    public OpenAIService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> GetAIResponse(string message)
    {
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = message }
            }
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            content);

        var responseString = await response.Content.ReadAsStringAsync();
        dynamic json = JsonConvert.DeserializeObject(responseString);

        return json.choices[0].message.content;
    }
}