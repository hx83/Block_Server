using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleServer.Room.Map.PlayerOp;
using BattleServer.Utils;
using BattleServer.Room.Map.ChangePosition;
using System.Timers;

namespace BattleServer.Room.Map.SceneObj
{
    /// <summary>
    /// 战斗场景中的玩家数据
    /// </summary>
    public class BattlePlayer : BaseSceneObject
    {
        private Timer timer;
        private ulong id;
        private Vector2 pos;
        private bool isDie;
        private BattleMap map;
        private IChangePosition changePos;
        private Vector2 bornPos;
        //————————————————————————————基础属性(基本都是读配置)
        /// <summary>
        /// 移动速度
        /// </summary>
        private float speed = 1;
        /// <summary>
        /// 跳跃的整个时间
        /// </summary>
        private float jumpTime = 0.8f;
        /// <summary>
        /// 跳跃结束后的恢复时间
        /// </summary>
        private float revertTime = 1;
        /// <summary>
        /// 死亡到复活的时间
        /// </summary>
        private float dieTime = 1;
        /// <summary>
        /// 可翻转方块的数量
        /// </summary>
        private int power = 2;

        public BattlePlayer()
        {
            pos = new Vector2();
            timer = new Timer(DieTime);
            timer.Elapsed += new ElapsedEventHandler(this.TimerHandler);
            timer.AutoReset = false;
        }
        public Vector2 Position
        {
            get
            {
                return pos;
            }
            set
            {
                this.pos = value;
            }
        }
        public Vector2 BornPosition
        {
            set
            {
                this.bornPos = value;
                this.X = this.bornPos.X;
                this.Y = this.bornPos.Y;
            }
        }
        public ulong ID
        {
            set
            {
                id = value;
            }
            get
            {
                return id;
            }
        }
        /// <summary>
        /// 获取人物当前所在的格子
        /// </summary>
        public MapGrid CurrentGrid
        {
            get
            {
                if(map != null)
                {
                    return map.GetMapGrid(this.pos.X, this.pos.Y);
                }
                else
                {
                    return null;
                }

            }
        }
        

        public BattleMap Map
        {
            set
            {
                this.map = value;
            }
            get
            {
                return this.map;
            }
        }
        public void ApplyOp(IPlayerOp op)
        {
            if (this.isDie)
                return;

            switch(op.Type)
            {
                case PlayerOpEnum.Move:
                    WalkAction wa = new WalkAction();
                    wa.dir = (op as MoveOp).Direction;
                    this.ApplyChangePos(wa);                    

                    break;

                case PlayerOpEnum.Jump:

                    JumpAction ja = new JumpAction();
                    this.ApplyChangePos(ja);
                    break;
            }
        }
        /// <summary>
        /// 应用位移算法
        /// </summary>
        /// <param name="cp"></param>
        private void ApplyChangePos(IChangePosition cp)
        {
            if (this.changePos == null)
            {
                this.changePos = cp;
            }
            else if (this.changePos.Level <= cp.Level)
            {
                this.changePos.Clear();
                this.changePos = cp;
            }

            if(this.changePos != null)
            {
                this.changePos.Player = this;
                this.changePos.Init();
            }   
        }
        /// <summary>
        /// 被弹飞（死了）
        /// </summary>
        public void Die()
        {
            if(this.isDie)
            {
                return;
            }
            isDie = true;
            timer.Start();
        }
        private void TimerHandler(object obj, ElapsedEventArgs e)
        {
            this.Relife();
        }
        /// <summary>
        /// 复活
        /// </summary>
        public void Relife()
        {
            this.isDie = false;
            this.BornPosition = this.bornPos;
        }

        public void Update()
        {
            if (isDie)
                return;
            //
            if (changePos != null)
                changePos.Update();
        }

        public void Dispose()
        {
            this.map = null;
            this.changePos.Clear();
            this.changePos = null;
        }
        /// <summary>
        /// 获取玩家跳跃以后需要翻转的格子（默认以十字为计算方式，后期改为可动态变化）
        /// </summary>
        /// <returns></returns>
        public List<List<MapGrid>> GetRollGrids()
        {
            List<List<MapGrid>> list = new List<List<MapGrid>>();
            MapGrid curGrid = this.CurrentGrid;
            //list.Add(curGrid);

            int x = curGrid.XIndex;
            int y = curGrid.YIndex;

            MapGrid grid = null;

            List<MapGrid> left = new List<MapGrid>();
            List<MapGrid> right = new List<MapGrid>();
            List<MapGrid> up = new List<MapGrid>();
            List<MapGrid> down = new List<MapGrid>();

            for (int i = x; i > x - power; i--)
            {
                grid = this.map.GetMapGridByIndex(i, y);
                left.Add(grid);
            }
            for (int i = x; i < x + power; i++)
            {
                grid = this.map.GetMapGridByIndex(i, y);
                right.Add(grid);
            }

            for (int i = y; i > y - power; i--)
            {
                grid = this.map.GetMapGridByIndex(x, i);
                up.Add(grid);
            }
            for (int i = y; i < y + power; i++)
            {
                grid = this.map.GetMapGridByIndex(x, i);
                down.Add(grid);
            }

            list.Add(left);
            list.Add(right);
            list.Add(up);
            list.Add(down);

            return list;
        }

        //
        //
        //
        //
        //
        public float Speed
        {
            get
            {
                return speed;
            }
        }
        public float JumpTime
        {
            get
            {
                return jumpTime;
            }
        }
        public float RevertTime
        {
            get
            {
                return revertTime;
            }
        }
        public float DieTime
        {
            get
            {
                return dieTime;
            }
        }
    }
}
