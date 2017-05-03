using BattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map.ChangePosition
{
    /// <summary>
    ///  走路
    /// </summary>
    public class WalkAction : AbstractChangePosition
    {
        public Vector2 dir;
        public WalkAction()
        {

        }

        public override void Update()
        {
            base.Update();

            float finalX, finalY;
            this.player.Position = this.player.Position + this.dir * this.player.Speed * Room.FRAME_TIME_SEC;

            BattleMap map = this.player.Map;
            if (map != null)
            {
                //检查是否会超出边界
                if (map.IsOutMap(this.player.Position.X, this.player.Position.Y, out finalX, out finalY))
                {
                    this.player.Position.X = finalX;
                    this.player.Position.Y = finalY;
                }
            }
        }
    }
}
