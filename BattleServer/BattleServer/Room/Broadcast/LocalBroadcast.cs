using BattleServer.Room.Map.SceneObj;
using BattleServer.Utils.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Broadcast
{
    /// <summary>
    /// 本地单机时用的通讯
    /// </summary>
    public class LocalBroadcast : IBroadcast
    {
        public LocalBroadcast()
        {

        }

        public void BroadcastToAll(string type ,List<BattlePlayer> list, Object obj)
        {
            
        }
        public void Broadcast(string type ,BattlePlayer player, Object obj)
        {
            RoomEventDispatcher.DispatchEvent(type, obj);
        }
    }


}
