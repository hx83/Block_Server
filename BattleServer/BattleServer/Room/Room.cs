using BattleServer.Room.Map;
using BattleServer.Room.Map.PlayerOp;
using BattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room
{
    public class Room
    {
        public static float FRAME_TIME = 20;//毫秒
        public static float FRAME_TIME_SEC = 20/1000f;//毫秒

        private static BattleScene scene;

        public Room()
        {

        }
        /// <summary>
        /// 本地服务初始化，读取配置等
        /// </summary>
        public static void Init()
        {
            MapManager.Setup();
        }
        public static void Start()
        {
            if(scene != null)
            {
                MapManager.RemoveScene(scene.ID);
                scene.Dispose();
            }
            scene = MapManager.CreateScene(1);
        }

        public static void JoinGame(ulong id,string name,bool isRobot)
        {
            PlayerTask p = new PlayerTask();
            p.ID = id;
            p.Name = name;
            p.IsRobot = isRobot;
            scene.AddPlayer(p);
        }
        /// <summary>
        /// 玩家移动
        /// </summary>
        /// <param name="id">玩家ID</param>
        /// <param name="dir">方向(用战斗中的Vector2类)</param>
        public static void Move(ulong id,Vector2 dir)
        {
            if(scene != null)
            {
                MoveOp op = new MoveOp();
                op.Direction = dir;
                scene.AddPlayerOp(id, op);
            }
        }
        /// <summary>
        /// 跳
        /// </summary>
        /// <param name="id"></param>
        public static void Jump(ulong id)
        {
            if (scene != null)
            {
                JumpOp op = new JumpOp();
                scene.AddPlayerOp(id, op);
            }
        }
    }

    

}
