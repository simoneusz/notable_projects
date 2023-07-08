using System;
using System.Collections.Generic;

namespace TextEditor
{
    public enum EventType
    {
        TextAdded,
        CharacterDeleted,
        FileSaved
    }
    public sealed class EventLog
    {
        public DateTime TimeStamp { get; set; }
        public EventType EventType { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }

    public sealed class EventLogger
    {
        private static readonly Lazy<EventLogger> lazy = new(() => new());
        private readonly List<EventLog> eventLogs = new();

        public static EventLogger Instance => lazy.Value;

        private EventLogger() { }

        public void LogEvent(EventType eventType, int line, int column)
        {
            EventLog log = new EventLog
            {
                TimeStamp = DateTime.Now,
                EventType = eventType,
                Line = line + 1,
                Column = column + 1
            };
            eventLogs.Add(log);
        }

        public List<EventLog> GetEventLogs()
        {
            return new List<EventLog>(eventLogs);
        }
    }
}