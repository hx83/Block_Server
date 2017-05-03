using BattleServer.Room.Map.SceneObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Message
{
    class RoomMessage
    {
    }
    /// <summary>
    /// 场景中所有道具、人
    /// </summary>
    public struct SceneObjectsMsg
    {
        public BattlePlayer mine;
        public List<BattlePlayer> others;
    }
}
