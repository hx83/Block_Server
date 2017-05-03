using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleServer.Room.Map;
using BattleServer.Room.Map.SceneObj;

namespace BattleServer.Room.Broadcast
{
    /// <summary>
    /// 广播消息的接口，客户端本地版本和服务器版本实现不同的广播机制（事件\SOCKET通信)
    /// </summary>
    public interface IBroadcast
    {
        void BroadcastToAll(string type,List<BattlePlayer> list,Object obj);
        void Broadcast(string type, BattlePlayer player, Object obj);
    }
}
