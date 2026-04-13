using System;
using System.Threading.Tasks;
using GenderClassifyAPI.Models;

namespace GenderClassifyAPI.Services;

public class GenderService(IHttpClientFactory clientFactory) : IGenderService
{
    public async Task<(bool Success, object Result, int StatusCode)> ClassifyNameAsync(string name)
    {
        var client = clientFactory.CreateClient("GenderizeClient");

        try
        {
            var response = await client.GetFromJsonAsync<GenderizeResponse>($"?name={name}");

            if (response == null || string.IsNullOrEmpty(response.Gender) || response.Count == 0)
            {
                return (false, new ApiResponse("error", Message: "No prediction available for the provided name"), 200);
            }

            var processed = new ProcessedData(
                response.Name,
                response.Gender,
                response.Probability,
                response.Count,
                Is_Confident: response.Probability >= 0.7 && response.Count >= 100,
                Processed_At: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            );

            return (true, processed, 200);
        }
        catch
        {
            return (false, new ApiResponse("error", Message: "Internal Server Error"), 500);
        }
    }
}