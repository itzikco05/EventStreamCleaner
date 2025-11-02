using EventStreamCleaner.Models;
using System.Text.Json;
namespace EventStreamCleaner.Services
{
    public class FileHandlerService
    {
        public static List<TrafficEvent> ReadJson(string path)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException("The file doesn't exist");
            string json= File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidDataException("The JSON is not valid");

            try
            {
                 var doc = JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<List<TrafficEvent>>(json) ?? new List<TrafficEvent>();
                }
                else if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    var single = JsonSerializer.Deserialize<TrafficEvent>(json);
                    return single != null ? new List<TrafficEvent> { single } : new List<TrafficEvent>();
                }
                else
                {
                    throw new InvalidDataException("JSON root must be an object or an array.");
                }
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException (ex.Message);
            }

        }
        public static void WriteJson(string path, List<TrafficEvent> data)
        {
            string cleaned_events= JsonSerializer.Serialize(data,new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, cleaned_events);
        }
    }
}
