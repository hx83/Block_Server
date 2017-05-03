using BattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map.ChangePosition
{
    public class JumpAction : AbstractChangePosition
    {
        private Vector2 start;
        private Vector2 end;

        private bool isStart;
        private bool isOver;
        private bool isSit;
        /// <summary>
        /// 已走过的时间
        /// </summary>
        private float time;
        /// <summary>
        /// 总共的持续时间
        /// </summary>
        private float duringTime = 1;
        /// <summary>
        /// 运算中的变化量
        /// </summary>
        private float cx;
        private float cy;
        /// <summary>
        /// 坐在地上的僵直时间
        /// </summary>
        private float sitTime;
        public JumpAction()
        {
            
        }
        public override void Init()
        {
            base.Init();
            
            this.start = this.player.Position.Copy();
            this.end = this.player.CurrentGrid.Postion.Copy();

            isStart = true;
            isOver = false;

            cx = this.end.X - this.start.X;
            cy = this.end.Y - this.start.Y;

            _level = ChangePositionEnum.LEVEL_HIGHEST;
            sitTime = 0;
        }
        /// <summary>
        /// 跳跃的计算在服务器其实不关心高度，只关心多长时间X方向移动到目标格子
        /// </summary>
        public override void Update()
        {
            base.Update();
            if(isOver)
            {
                return;
            }

            if(isStart)
            {
                if (isSit == true)
                {
                    this.sitTime += Room.FRAME_TIME_SEC;
                    if(this.sitTime >= this.player.RevertTime)
                    {
                        _level = ChangePositionEnum.LEVEL_ZERO;
                        isOver = true;
                        isSit = false;
                    }
                }
                else
                {
                    this.time += Room.FRAME_TIME_SEC;
                    if (this.time >= this.duringTime)
                    {
                        this.player.Position = this.end;
                        //跳完以后进入坐在地上的僵直时间
                        isSit = true;
                        _level = ChangePositionEnum.LEVEL_ONE;
                        //占据格子
                        this.player.Map.OccupyGrids(this.player);
                    }
                    else
                    {
                        this.player.Position.X = this.cx * this.time / this.duringTime + this.start.X;
                        this.player.Position.Y = this.cy * this.time / this.duringTime + this.start.Y;
                    }
                }
            }
        }

        public override void Clear()
        {
            base.Clear();

            isStart = false;
            isOver = false;
            isSit = false;
            sitTime = 0;
        }
    }
}
