using System;

namespace WaxingSimulation.Events
{
    public static class EventManager
    {
        public static event Action OnWaxDrying;
        public static event Action OnComplete;

        public static void GameCompleted() => OnComplete?.Invoke();

        public static void WaxDrying() => OnWaxDrying?.Invoke();
    }
}