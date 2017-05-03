using System.Collections.Generic;

namespace BattleServer.Utils.Event
{
    public class RoomEvent
    {
        public static string SCENE_OBJECTS = "sceneObjects";

        protected string _type;
        protected object _eventObj;

        public RoomEvent(string type, object eventObj = null)
        {
            _type = type;
            _eventObj = eventObj;
        }

        public virtual void Reset(string type, object eventObj = null)
        {
            _type = type;
            _eventObj = eventObj;
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }

        public object EventObj
        {
            get
            {
                return _eventObj;
            }
        }

        public virtual void Clear()
        {
            _eventObj = null;
            _type = "";
        }


        public static RoomEvent GetCache(string type, object eventObj = null)
        {
            if (_pool.Count > 0)
            {
                RoomEvent evt = _pool.Dequeue();
                evt.Reset(type, eventObj);
                return evt;
            }
            return new RoomEvent(type, eventObj);
        }

        public static void Cache(RoomEvent obj)
        {
            obj.Clear();
            if (_pool.Count < _poolMaxSize)
                _pool.Enqueue(obj);
        }

        private static Queue<RoomEvent> _pool = new Queue<RoomEvent>();
        private static int _poolMaxSize = 500;
    }
}

