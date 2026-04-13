using System.Threading.Tasks;

namespace GenderClassifyAPI.Services;

public interface IGenderService
{
    Task<(bool Success, object Result, int StatusCode)> ClassifyNameAsync(string name);
}