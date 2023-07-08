using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextEditor
{
    public class ReportBuilder
    {
        private List<EventLog> eventLogs;

        public ReportBuilder(List<EventLog> eventLogs)
        {
            this.eventLogs = eventLogs;
        }

        public string BuildReport()
        {
            var report = new Dictionary<string, object>();

            var textAddedEvents = GetEventsOfType(EventType.TextAdded);
            report["text_added_count"] = textAddedEvents.Count;
            report["text_added_locations"] = GetEventLocations(textAddedEvents);

            var characterDeletedEvents = GetEventsOfType(EventType.CharacterDeleted);
            report["character_deleted_count"] = characterDeletedEvents.Count;
            report["character_deleted_locations"] = GetEventLocations(characterDeletedEvents);

            var fileSavedEvents = GetEventsOfType(EventType.FileSaved);
            report["file_saved_count"] = fileSavedEvents.Count;

            return JsonConvert.SerializeObject(report);
        }

        private List<EventLog> GetEventsOfType(EventType eventType)
        {
            return eventLogs.FindAll(log => log.EventType == eventType);
        }

        private List<string> GetEventLocations(List<EventLog> events)
        {
            var locations = new List<string>();

            foreach (var ev in events)
            {
                locations.Add($"line {ev.Line}, column {ev.Column}");
            }

            return locations;
        }
    }
}
