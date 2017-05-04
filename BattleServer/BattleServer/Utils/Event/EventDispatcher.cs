using System.Collections;
using System.Collections.Generic;
using System;
namespace BattleServer.Utils.Event
{
    public class EventDispatcher
    {
        private Dictionary<string, System.Action<RoomEvent>> _eventDictionary = new Dictionary<string, System.Action<RoomEvent>>();

        public bool HasEventListener(string type)
        {
            return _eventDictionary.ContainsKey(type);
        }

        public void AddEventListener(string type, System.Action<RoomEvent> listener)
        {
            System.Action<RoomEvent> function;
            if (_eventDictionary.TryGetValue(type, out function))
            {
                function -= listener;
                function += listener;
                _eventDictionary.Remove(type);
                _eventDictionary.Add(type, function);
            }
            else
            {
                _eventDictionary.Add(type, listener);
            }
        }

        public void RemoveAllEventListender()
        {
            _eventDictionary.Clear();
        }

        public void DispatchEvent(string type, object eventObj = null)
        {
            if (_eventDictionary.ContainsKey(type))
            {
                System.Action<RoomEvent> function = _eventDictionary[type];
                RoomEvent evt = RoomEvent.GetCache(type, eventObj);
                function.Invoke(evt);
                RoomEvent.Cache(evt);
            }
        }

        public void DispatchEventInstance(RoomEvent evt)
        {
            if (_eventDictionary.ContainsKey(evt.Type))
            {
                System.Action<RoomEvent> function = _eventDictionary[evt.Type];
                function.Invoke(evt);
            }
        }

        public void RemoveEventListener(string type, System.Action<RoomEvent> listener)
        {
            if (_eventDictionary.ContainsKey(type))
            {
                System.Action<RoomEvent> function = _eventDictionary[type];
                function -= listener;
                _eventDictionary.Remove(type);
                if (function != null)
                {
                    _eventDictionary.Add(type, function);
                }
            }
        }
    }

}
