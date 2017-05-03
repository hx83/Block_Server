using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map.PlayerOp
{
    /// <summary>
    /// 玩家的操作指令接口
    /// </summary>
    public interface IPlayerOp
    {
        PlayerOpEnum Type { get; }
        
    }

    public enum PlayerOpEnum
    {
        Move,
        Jump,
    }
}
