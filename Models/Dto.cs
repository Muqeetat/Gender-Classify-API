using System.Text.Json.Serialization;

namespace GenderClassifyAPI.Models;

public record GenderizeResponse(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("gender")] string? Gender,
    [property: JsonPropertyName("probability")] double Probability,
    [property: JsonPropertyName("count")] int Count
);

public record ApiResponse(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("data")] object? Data = null,
    [property: JsonPropertyName("message")] string? Message = null
);

public record ProcessedData(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("gender")] string? Gender,
    [property: JsonPropertyName("probability")] double Probability,
    [property: JsonPropertyName("sample_size")] int Sample_Size,
    [property: JsonPropertyName("is_confident")] bool Is_Confident,
    [property: JsonPropertyName("processed_at")] string Processed_At
);