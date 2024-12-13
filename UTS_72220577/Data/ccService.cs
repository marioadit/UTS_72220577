using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace UTS_72220577.Data
{
    internal class ccService
    {
        private readonly HttpClient _httpClient;
        private const string CoursesEndpoint = "api/courses";
        private const string CategoriesEndpoint = "api/categories";
        private const string LoginEndpoint = "api/login"; // Add login endpoint
        private const string EnrollmentsEndpoint = "api/enrollments"; // Add enrollments endpoint

        private string token;

        public ccService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net");
        }

        // Ensure Bearer Token is set for each request
        private void EnsureBearerToken()
        {
            if (string.IsNullOrEmpty(token))
            {
                // Retrieve the token from Preferences
                token = Preferences.Get("auth_token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is missing or invalid.");
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Login and get token
        public async Task<string> LoginAsync(login loginData)
        {
            var response = await _httpClient.PostAsJsonAsync(LoginEndpoint, loginData);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<loginresponse>();
                if (loginResponse?.token != null)
                {
                    token = loginResponse.token;

                    // Store the token securely in Preferences
                    Preferences.Set("auth_token", token);

                    EnsureBearerToken(); // Set token for subsequent requests
                    return token;
                }
                else
                {
                    throw new Exception("Login successful, but token is missing in the response.");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Login failed: {response.ReasonPhrase}. Details: {errorContent}");
            }
        }

        // Logout and remove token
        public void Logout()
        {
            Preferences.Remove("auth_token"); // Remove the token from Preferences
            token = string.Empty; // Clear local token variable
        }

        // Course Services
        public async Task<IEnumerable<course>> GetCoursesAsync()
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync(CoursesEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<course>>();
        }

        public async Task<course> GetCourseByIdAsync(int id)
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync($"{CoursesEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<course>();
        }

        public async Task AddCourseAsync(course course)
        {
            EnsureBearerToken();
            var content = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(CoursesEndpoint, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCourseAsync(course course)
        {
            EnsureBearerToken();
            var content = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{CoursesEndpoint}/{course.courseId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCourseAsync(int id)
        {
            EnsureBearerToken();
            var response = await _httpClient.DeleteAsync($"{CoursesEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Category Services
        public async Task<IEnumerable<category>> GetCategoriesAsync()
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync(CategoriesEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<category>>();
        }

        public async Task<category> GetCategoryByIdAsync(int id)
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync($"{CategoriesEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<category>();
        }

        public async Task AddCategoryAsync(category category)
        {
            EnsureBearerToken();
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(CategoriesEndpoint, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategoryAsync(category category)
        {
            EnsureBearerToken();
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{CategoriesEndpoint}/{category.categoryId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            EnsureBearerToken();
            var response = await _httpClient.DeleteAsync($"{CategoriesEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Search Courses by Name
        public async Task<List<course>> GetCoursesByNameAsync(string courseName)
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync($"{CoursesEndpoint}/search/{courseName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<course>>() ?? new List<course>();
            }
            throw new Exception($"Failed to load courses: {response.ReasonPhrase}");
        }

        // get instructors
        public async Task<IEnumerable<instructors>> GetInstructorsAsync()
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync("api/instructors");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<instructors>>();
        }

        // enrollments
        public async Task<IEnumerable<enrollments>> GetEnrollmentsAsync()
        {
            EnsureBearerToken();
            var response = await _httpClient.GetAsync(EnrollmentsEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<enrollments>>();
        }

        public async Task AddEnrollmentAsync(enrollments enrollment)
        {
            EnsureBearerToken();
            var content = new StringContent(JsonSerializer.Serialize(enrollment), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(EnrollmentsEndpoint, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
