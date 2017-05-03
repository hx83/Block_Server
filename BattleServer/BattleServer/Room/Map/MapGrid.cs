using BattleServer.Room.Map.SceneObj;
using BattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map
{
    /// <summary>
    /// 地图上每个单元格子
    /// </summary>
    public class MapGrid
    {
        public static uint WIDTH = 100;
        public static uint HEIGHT = 100;
        /// <summary>
        /// 下一个需要翻转的格子
        /// </summary>
        public MapGrid NextRollGrid;

        private int xindex;
        private int yindex;


        private float x;
        private float y;

        private Vector2 pos;
        private Vector2 center;
        /// <summary>
        /// 当前格子的所有者（被谁占了）
        /// </summary>
        private BattlePlayer owner;
        /// <summary>
        /// 每帧翻转的角度(假设500毫秒翻180度)
        /// </summary>
        private float rollSpeed = 180*Room.FRAME_TIME/500;
        private bool isStartRoll;

        private BattlePlayer tempPlayer;
        //
        private float startAngle = 0;
        private float endAngle = 180;

        public MapGrid()
        {
            pos = new Vector2();
            center = new Vector2();
        }

        public int XIndex
        {
            set
            {
                xindex = value;
                x = MapGrid.WIDTH * xindex;
                pos.X = x;
            }
            get
            {
                return xindex;
            }
        }

        public int YIndex
        {
            set
            {
                yindex = value;
                y = MapGrid.WIDTH * yindex;
                pos.Y = y;
            }
            get
            {
                return yindex;
            }
        }
        public Vector2 Center
        {
            get
            {
                center.X = this.pos.X + MapGrid.WIDTH/2;
                center.Y = this.pos.Y + MapGrid.HEIGHT/2;
                return center;
            }
        }
        public Vector2 Postion
        {
            get
            {
                return this.pos;
            }
        }


        public BattlePlayer Owner
        {
            set
            {
                this.owner = value;
            }
            get
            {
                return this.owner;
            }
        }
        /// <summary>
        /// 检查是否需要转、弹飞别人等
        /// </summary>
        public void Update()
        {
            if(isStartRoll)
            {
                startAngle += rollSpeed;
                if(startAngle > 90 && this.Owner != tempPlayer)
                {
                    this.Owner = tempPlayer;
                    if(this.NextRollGrid != null)
                    {
                        this.NextRollGrid.Roll(this.Owner);
                    }
                }
                if(startAngle >= 180)
                {
                    //翻转结束
                    isStartRoll = false;
                    startAngle = 0;
                }
                this.CheckPlayer();
            }
        }
        /// <summary>
        /// 查看玩家是否在该格子上，如果是且不是自己人，弹飞
        /// </summary>
        private void CheckPlayer()
        {
            if(this.tempPlayer == null)
                return;
            //
            Dictionary<ulong, BattlePlayer> dict = this.tempPlayer.Map.Scene.PlayerDict;
            foreach (var item in dict)
            {
                BattlePlayer p = item.Value;
                if(p.CurrentGrid == this && p != this.tempPlayer)
                {
                    //弹飞
                    p.Map.Scene.ShootOff(p);
                }
            }
        }
        /// <summary>
        /// 开始翻转
        /// </summary>
        public void Roll(BattlePlayer player, bool immediately = false)
        {
            tempPlayer = player;
            if(immediately)
            {
                this.Owner = player;
                isStartRoll = false;
            }
            else
            {
                isStartRoll = true;
                startAngle = 0;
            }
        }
    }
}
