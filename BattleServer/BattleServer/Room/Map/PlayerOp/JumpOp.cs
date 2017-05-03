using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map.PlayerOp
{
    public class JumpOp : IPlayerOp
    {
        private PlayerOpEnum _type;
        public JumpOp()
        {
            _type = PlayerOpEnum.Jump;
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
