using BattleServer.Room.Map.SceneObj;
using BattleServer.Utils.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room
{
    /// <summary>
    /// 客户端连接过来的用户
    /// </summary>
    public class PlayerTask
    {
        /// <summary>
        /// 帐号ID（数据库ID）
        /// </summary>
        public ulong ID;
        public string Name;

        public bool IsRobot;
        public PlayerTask()
        {

        }

        public void Broadcast(string type, Object obj)
        {
            if(this.IsRobot == false)
            {
                RoomEventDispatcher.DispatchEvent(type, obj);
            }
        }
    }
}
