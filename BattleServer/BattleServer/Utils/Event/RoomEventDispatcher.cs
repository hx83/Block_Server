using System;
/// <summary>
/// ACE
/// 全局事件
/// </summary>

namespace BattleServer.Utils.Event
{
    public static class RoomEventDispatcher
    {
        public static void AddEventListener(string type, Action<RoomEvent> listener)
        {
            _dispatcher.AddEventListener(type, listener);
        }

        public static void DispatchEvent(string type, object eventObj = null)
        {
            _dispatcher.DispatchEvent(type, eventObj);
        }

        public static bool HasEventListener(string type)
        {
            return _dispatcher.HasEventListener(type);
        }

        public static void RemoveAllEventListender()
        {
            _dispatcher.RemoveAllEventListender();
        }

        public static void RemoveEventListener(string type, Action<RoomEvent> listener)
        {
            _dispatcher.RemoveEventListener(type, listener);
        }

        private static EventDispatcher _dispatcher = new EventDispatcher();
    }

}
