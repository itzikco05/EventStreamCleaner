using EventStreamCleaner.Services;
namespace EventStreamCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
            Console.WriteLine(filePath);
            string outputPath = "cleaned_events.json";
            var events = FileHandlerService.ReadJson(filePath);
            EventsProcessor eventsProcessor = new EventsProcessor();
            var cleanedEvents= eventsProcessor.Process(events);
            FileHandlerService.WriteJson(outputPath, cleanedEvents);
            eventsProcessor.PrintSummary();        }
    }
}
