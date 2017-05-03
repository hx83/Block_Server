using BattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BattleServer.Room.Map.PlayerOp
{
    /// <summary>
    /// 移动指令
    /// </summary>
    public class MoveOp : IPlayerOp
    {
        private PlayerOpEnum _type;

        private Vector2 dir;
        
        public MoveOp()
        {
            _type = PlayerOpEnum.Move;
            Direction = new Vector2();
        }

        public Vector2 Direction
        {
            set
            {
                dir = value;
            }
            get
            {
                return dir;
            }
        }
        public PlayerOpEnum Type
        {
            get
            {
                return _type;
            }
        }
    }
}
