using System;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using AnalyticsEvent = Unity.Services.Analytics.Event;

public class EventManager : MonoBehaviour
{

    public class LevelStartEvent : AnalyticsEvent
    {
        public LevelStartEvent() : base("LevelStart") { }
        public int level { set { SetParameter("level", value); } }
    }

    public class LevelCompleteEvent : AnalyticsEvent
    {
        public LevelCompleteEvent() : base("LevelComplete") { }
        public int level { set { SetParameter("level", value); } }
        public int time { set { SetParameter("time", value); } }
        public int death { set { SetParameter("death", value); } }
    }

    public class GameCompleteEvent : AnalyticsEvent
    {
        public GameCompleteEvent() : base("GameComplete") { }
        public int time { set { SetParameter("time", value); } }
        public int death { set { SetParameter("death", value); } }
    }

    public class GameOverEvent : AnalyticsEvent
    {
        public GameOverEvent() : base("GameOver") { }
        public string type { set { SetParameter("type", value); } }
        public string name { set { SetParameter("name", value); } }
        public int level { set { SetParameter("level", value); } }
    }

    public class InteractEvent : AnalyticsEvent
    {
        public InteractEvent() : base("Interact") { }
        public int time { set { SetParameter("time", value); } }

    }
}