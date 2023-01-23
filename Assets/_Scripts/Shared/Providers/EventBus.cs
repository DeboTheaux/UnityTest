using System;
using System.Collections.Generic;
using UniRx;

namespace UT.Shared
{
    public class EventBus
    {
        IObserver<Event> eventObserver;

        public EventBus(IObserver<Event> eventObserver)
        {
            this.eventObserver = eventObserver;
        }

        public void Execute(Event evt)
        {
            eventObserver.OnNext(evt);
        }
    }

    public class Event
    {

        public string EventId { get; private set; }
        public Dictionary<string, object> Parameters { get; private set; }

        public Event() : this("", new Dictionary<string, object>())
        {

        }

        public Event(string eventId, Dictionary<string, object> parameters)
        {
            EventId = eventId;
            this.Parameters = parameters;
        }
    }

    public class EventsProvider
    {
        private static EventBus EventBusInstance = null;
        private static Subject<Event> SubjectInstance = null;

        public static EventBus EventBus()
        {
            return EventBusInstance ?? (EventBusInstance = new EventBus(EventObserver()));
        }

        private static Subject<Event> GetEventSubject()
        {
            return SubjectInstance ?? (SubjectInstance = new Subject<Event>());
        }

        public static IObserver<Event> EventObserver()
        {
            return GetEventSubject();
        }

        internal static IObservable<Event> EventObservable()
        {
            return GetEventSubject();
        }
    }
}
