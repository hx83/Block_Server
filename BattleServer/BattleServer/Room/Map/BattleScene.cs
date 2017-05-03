using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleServer.Room.Broadcast;
using BattleServer.Room.Map.PlayerOp;
using BattleServer.Room.Map.SceneObj;
using System.Diagnostics;


namespace BattleServer.Room.Map
{
    /// <summary>
    /// 战斗场景，包含地图、人物
    /// </summary>
    public class BattleScene
    {
        public ulong ID;

        private ulong PlayerID = 0;
        private ulong RobotID = 10000000;


        private BattleMap map;

        private IBroadcast broadcast;
        //private List<BattlePlayer> playerList;
        private Dictionary<ulong, BattlePlayer> playerDict;
        //
        private Dictionary<ulong, IPlayerOp> opDict;
        public BattleScene()
        {
            //playerList = new List<BattlePlayer>();
            playerDict = new Dictionary<ulong, BattlePlayer>();
            opDict = new Dictionary<ulong, IPlayerOp>();

            map = new BattleMap();
            map.Scene = this;
        }
        /// <summary>
        /// 添加一个玩家
        /// </summary>
        /// <param name="p"></param>
        public void AddPlayer(PlayerTask p)
        {
            BattlePlayer player = null;

            if(playerDict.ContainsKey(p.ID))
            {
                player = playerDict[p.ID];
            }
            else
            {
                player = new BattlePlayer();
                player.ID = this.GetUserID();
                playerDict.Add(player.ID, player);
            }

            player.Map = this.map;
            player.BornPosition = this.map.NextBornPos;
            
        }

        /// <summary>
        /// 某个玩家被弹飞
        /// </summary>
        /// <param name="p"></param>
        public void ShootOff(BattlePlayer p)
        {
            if (p != null)
            {
                p.Die();
            }
        }

        /// <summary>
        /// 帧驱动进行地图运算
        /// </summary>
        public void Update()
        {
            //应用操作
            foreach (var item in opDict)
            {
                if(this.playerDict.ContainsKey(item.Key))
                {
                    BattlePlayer p = this.playerDict[item.Key];
                    p.ApplyOp(item.Value);
                }                
            }
            opDict.Clear();

            //更新数据
            foreach (var item in playerDict)
            {
                item.Value.Update();
            }
            //地图更新
            this.map.Update();
        }
        /// <summary>
        /// 同一个玩家在同一帧中有多个操作，后者会覆盖前者
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="op"></param>
        public void AddPlayerOp(ulong ID,IPlayerOp op)
        {
            if(playerDict.ContainsKey(ID) == false)
            {
                return;
            }
            else
            {
                if(opDict.ContainsKey(ID) == true)
                {
                    opDict.Remove(ID);
                }
                opDict.Add(ID, op);
            }
        }


        /// <summary>
        /// 获取一个用户在战斗场景中的ID
        /// </summary>
        /// <returns></returns>
        public ulong GetUserID()
        {
            PlayerID++;            
            return PlayerID;
        }
        /// <summary>
        /// 获取一个机器人的ID
        /// </summary>
        /// <returns></returns>
        public ulong GetRobotID()
        {
            RobotID++;
            return RobotID;
        }
        /// <summary>
        /// 场景中的玩家列表
        /// </summary>
        public Dictionary<ulong, BattlePlayer> PlayerDict
        {
            get
            {
                return this.playerDict;
            }
        }
    }
}
