using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ConferenceDTO;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private  HttpClient _httpClient;
        private IHttpClientFactory _clientFactory;

        public ApiClient(HttpClient httpClient)
        {

            _httpClient = httpClient;
            //   _clientFactory = clientFactory;
            //  _httpClient = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");
     
        }

        // public ApiClient(IHttpClientFactory clientFactory)
        // {
        //     _clientFactory = clientFactory;
        //     var _httpClient = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");
        // }



//         public UserService(IHttpClientFactory clientFactory, IOptions<AppSettings> appSettings)
//     {
//         _appSettings = appSettings.Value;
//         _clientFactory = clientFactory;
//     }

// var request = new HttpRequestMessage(...

// var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");

// HttpResponseMessage response = await client.SendAsync(request);

        public async Task<bool> AddAttendeeAsync(Attendee attendee)
        {
            

            var response = await _httpClient.PostAsJsonAsync($"/api/attendees", attendee);
             
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
             
            return true;
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"/api/attendees/{name}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AttendeeResponse>();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SessionResponse>();
        }

        public async Task<List<SessionResponse>> GetSessionsAsync()
        {
            // var response = await _httpClient.GetAsync("/api/sessions");

          HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        
        // Pass the handler to httpclient(from you are calling api)
         _httpClient = new HttpClient(clientHandler);
         
            // var response = await _httpClient.GetAsync("/api/sessions");
                var response = await _httpClient.GetAsync("https://localhost:5001/api/sessions");


            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SessionResponse>>();
        }

        public async Task DeleteSessionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/speakers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SpeakerResponse>();
        }

        public async Task<List<SpeakerResponse>> GetSpeakersAsync()
        {
            var response = await _httpClient.GetAsync("/api/speakers");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SpeakerResponse>>();
        }

        public async Task PutSessionAsync(Session session)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/sessions/{session.ID}", session);

            response.EnsureSuccessStatusCode();
        }
    }
}