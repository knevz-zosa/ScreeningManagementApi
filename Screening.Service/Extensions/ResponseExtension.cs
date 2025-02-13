using Screening.Common.Wrapper;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Screening.Service.Extensions;
public static class ResponseExtension
{
    public static async Task<ResponseWrapper<T>> ToResponse<T>(this HttpResponseMessage responseMessage)
    {        
        var responseAsString = await responseMessage.Content.ReadAsStringAsync();

        if (responseMessage.IsSuccessStatusCode)
        {
            try
            {
                var responseObject = JsonSerializer.Deserialize<ResponseWrapper<T>>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });

                return responseObject ?? new ResponseWrapper<T>().Failed("Deserialization resulted in null.");
            }
            catch (JsonException ex)
            {
                return new ResponseWrapper<T>().Failed($"Invalid response format. {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
        else
        {
            return new ResponseWrapper<T>().Failed($"Request failed with status code {responseMessage.StatusCode}: {responseAsString}");
        }
    }
}

