using BattleServer.Room.Map.SceneObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map.ChangePosition
{
    /// <summary>
    /// 不同的位移行为可能存在互相打断等等。用策略模式运行时替换
    /// 也会存在个别位移行为产生“霸体”效果，后续行为无法打断前置行为
    /// </summary>
    public interface IChangePosition
    {
        void Init();
        void Update();
        void Clear();

        ChangePositionEnum Level { get; }

        /// <summary>
        /// 是否霸体，无法被弹飞
        /// </summary>
        //bool IsEndure { get; }
        /// <summary>
        /// 是否僵直（无法移动等，但可以被击飞）
        /// </summary>
        //bool IsStiff { get; }
        BattlePlayer Player { get; set; }

    }
}
