using BattleServer.Room.Map.SceneObj;
using BattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map
{
    /// <summary>
    /// 战斗地图。包含格子信息等
    /// </summary>
    public class BattleMap
    {
        private int KEY_COUNT = 100000;
        private Dictionary<int, MapGrid> dict;

        private float _maxWidth;
        private float _maxHeight;
        public BattleScene Scene;

        private int bornIndex = 0;

        private List<Vector2> bornList;
        public BattleMap()
        {
            dict = new Dictionary<int, MapGrid>();

            bornIndex = 0;

            bornList = new List<Vector2>();
            Create();
        }

        private void Create()
        {
            int w = 13;
            int h = 9;

            _maxWidth = w * MapGrid.WIDTH;
            _maxHeight = h * MapGrid.HEIGHT;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    MapGrid grid = new MapGrid();
                    grid.XIndex = i;
                    grid.YIndex = j;

                    dict.Add(i * KEY_COUNT + j, grid);
                }
            }

            MapGrid temp = this.GetMapGridByIndex(0, 0);
            bornList.Add(temp.Center);
            temp = this.GetMapGridByIndex(w - 1, 0);
            bornList.Add(temp.Center);
            temp = this.GetMapGridByIndex(0, h - 1);
            bornList.Add(temp.Center);
            temp = this.GetMapGridByIndex(w - 1, h - 1);
            bornList.Add(temp.Center);
        }
        /// <summary>
        /// 每帧更新。驱动格子可能需要运算的行为
        /// </summary>
        public void Update()
        {
            foreach (var item in this.dict)
            {
                item.Value.Update();
            }
        }
        /// <summary>
        /// 占据格子
        /// </summary>
        /// <param name="player"></param>
        public void OccupyGrids(BattlePlayer player)
        {
            List<List<MapGrid>> list = player.GetRollGrids();
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                List<MapGrid> subList = list[i];
                int m = subList.Count;
                for (int j = 0; j < m; j++)
                {
                    MapGrid grid = subList[j];
                    if (j < m - 1)
                        grid.NextRollGrid = subList[j + 1];
                    else
                        grid.NextRollGrid = null;
                }
            }
            //
            MapGrid curGrid = player.CurrentGrid;
            if (curGrid != null)
                curGrid.Roll(player, true);

            for (int i = 0; i < n; i++)
            {
                List<MapGrid> subList = list[i];
                MapGrid grid = subList[0];
                if (grid != null)
                    grid.Roll(player);
            }
        }
        
        public float MinWidth
        {
            get
            {
                return 0;
            }
        }
        public float MinHeight
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 地图最大宽度
        /// </summary>
        public float MaxWidth
        {
            get
            {
                return _maxWidth;
            }
        }
        /// <summary>
        /// 地图最大高度
        /// </summary>
        public float MaxHeight
        {
            get
            {
                return _maxHeight;
            }
        }
        /// <summary>
        /// 根据X\Y索引获取格子
        /// </summary>
        /// <param name="xindex"></param>
        /// <param name="yindex"></param>
        /// <returns></returns>
        public MapGrid GetMapGridByIndex(int xindex,int yindex)
        {
            int key = xindex * KEY_COUNT + yindex;
            if(dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据实例坐标获取格子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MapGrid GetMapGrid(float x,float y)
        {
            int xindex = (int)Math.Floor(x / MapGrid.WIDTH);
            int yindex = (int)Math.Floor(y / MapGrid.HEIGHT);

            return this.GetMapGridByIndex(xindex, yindex);
        }
        /// <summary>
        /// 是否超出地图
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="finalX">如果超出范围，则返回应该达到的位置</param>
        /// <param name="finalY">如果超出范围，则返回应该达到的位置</param>
        /// <returns></returns>
        public bool IsOutMap(float x,float y,out float finalX,out float finalY)
        {
            finalX = 0;
            finalY = 0;

            if (x >= MinWidth && x <= this.MaxWidth && y >= MinHeight && y <= MaxHeight)
            {
                return false;
            }
            else
            {
                if (x < MinWidth)
                {
                    finalX = MinWidth;
                }
                else if (x > MaxWidth)
                {
                    finalX = MaxWidth;
                }
                if(y < MinHeight)
                {
                    finalY = MinHeight;
                }
                else if(y > MaxHeight)
                {
                    finalY = MaxHeight;
                }

                return true;
            }
            
        }
        /// <summary>
        /// 下一个出生点
        /// </summary>
        public Vector2 NextBornPos
        {
            get
            {
                Vector2 v = bornList[bornIndex];
                bornIndex++;
                if (bornIndex >= bornList.Count)
                    bornIndex = 0;
                return v;
            }
        }
    }
}
