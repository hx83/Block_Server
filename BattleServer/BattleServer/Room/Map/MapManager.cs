using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map
{
    public class MapManager
    {
        private static Dictionary<ulong, BattleScene> mapDict;
        public static void Setup()
        {
            mapDict = new Dictionary<ulong, BattleScene>();
        }
        /// <summary>
        /// 创建一个战斗场景
        /// </summary>
        public static BattleScene CreateScene(ulong id = 0)
        {
            if(mapDict.ContainsKey(id))
            {
                return mapDict[id];
            }
            else
            {
                BattleScene map = new BattleScene();
                map.ID = id;
                mapDict.Add(id, map);

                return map;
            }
        }
    }
}
