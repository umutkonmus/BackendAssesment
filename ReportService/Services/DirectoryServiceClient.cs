using ReportService.DTOs.Person;
using ReportService.Services.Abstract;
using ReportService.Utils;
using System.Text.Json;

namespace ReportService.Services
{
    public sealed class DirectoryServiceClient : IDirectoryServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DirectoryServiceClient> _logger;

        public DirectoryServiceClient(IHttpClientFactory httpClientFactory, ILogger<DirectoryServiceClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<PersonDTO>> GetAllPersonsAsync()
        {
            var client = _httpClientFactory.CreateClient("DirectoryService");
            var response = await client.GetAsync("/api/Person");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<List<PersonDTO>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null && result.IsSuccessful && result.Data != null)
                {
                    return result.Data;
                }
            }

            _logger.LogError($"Failed to get persons from Directory Service. Status code: {response.StatusCode}");
            return new List<PersonDTO>();
        }

        public async Task<List<ContactInfoDTO>> GetAllContactInfosAsync()
        {
            var persons = await GetAllPersonsAsync();
            var contactInfos = new List<ContactInfoDTO>();

            // For each person, get their contact information
            foreach (var person in persons)
            {
                var client = _httpClientFactory.CreateClient("DirectoryService");
                var response = await client.GetAsync($"/api/Person/{person.ID}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse<PersonWithContactInfoDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"Response content: {content}");

                    if (result != null && result.IsSuccessful && result.Data != null && result.Data.ContactInfos != null)
                    {
                        contactInfos.AddRange(result.Data.ContactInfos);
                    }
                }
                else
                {
                    _logger.LogError($"Failed to get contact info for person {person.ID}. Status code: {response.StatusCode}");
                }
            }

            return contactInfos;
        }
    }
}
