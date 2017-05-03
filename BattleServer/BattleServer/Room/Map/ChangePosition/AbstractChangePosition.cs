using BattleServer.Room.Map.SceneObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Room.Map.ChangePosition
{
    public abstract class AbstractChangePosition : IChangePosition
    {
        //protected bool endure;
        //protected bool stiff;
        protected BattlePlayer player;
        protected ChangePositionEnum _level;
        public AbstractChangePosition()
        {
            //endure = false;
            //stiff = false;
            _level = ChangePositionEnum.LEVEL_ZERO;
        }
        public virtual void Init()
        {

        }
        public virtual void Update()
        {

        }

        public BattlePlayer Player
        {
            set
            {
                player = value;
            }
            get
            {
                return player;
            }
        }
        public ChangePositionEnum Level
        {
            get
            {
                return _level;
            }
        }
        /// <summary>
        /// 是否霸体
        /// </summary>
        //public virtual bool IsEndure
        //{
        //    get
        //    {
        //        return endure;
        //    }
        //}
        /// <summary>
        /// 是否僵直
        /// </summary>
        //public virtual bool IsStiff
        //{
        //    get
        //    {
        //        return stiff;
        //    }
        //}
        public virtual void Clear()
        {
            this.player = null;
            //this.endure = false;
            _level = ChangePositionEnum.LEVEL_ZERO;
        }
    }
    /// <summary>
    /// 改变位置行为的优先级，低等级会被同等级或高等级覆盖
    /// </summary>
    public enum ChangePositionEnum
    {
        LEVEL_ZERO,//最低优先级
        LEVEL_ONE,
        LEVEL_HIGHEST,//最高级
    }
}
