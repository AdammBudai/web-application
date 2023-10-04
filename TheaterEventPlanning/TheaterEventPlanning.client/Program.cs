using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TheaterEventPlanning.client;

HttpClient client = new();
client.BaseAddress = new Uri("http://localhost:5242");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

HttpResponseMessage response = await client.GetAsync("api/Event");
response.EnsureSuccessStatusCode();

if(response.IsSuccessStatusCode)
{
    var events = await response.Content.ReadFromJsonAsync<IEnumerable<EventDto>>();
    
    foreach(var @event in events)
    {
        Console.WriteLine(@event.name);
    }

}
else { Console.WriteLine("No results."); }

Console.ReadLine();