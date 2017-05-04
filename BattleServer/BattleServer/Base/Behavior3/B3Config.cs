using Behavior3CSharp.Actions;
using Behavior3CSharp.Composites;
using Behavior3CSharp.Decorators;
using LitJson;
using System;
using System.Collections.Generic;
namespace Behavior3CSharp
{
    public class B3Config
    {
        public static string CONFIG_URL = "Config/";

        //public static JsonData Load(string name)
        //{
        //    //TextAsset jsonAsset = Resources.Load<TextAsset>(CONFIG_URL + name);
        //    return JsonMapper.ToJson(jsonAsset.text);
        //}

        public static JsonData Parse(string content)
        {
            return JsonMapper.ToObject(content);
        }

        public static void Setup()
        {
            _classDic.Add(Error.name, typeof(Error));
            _classDic.Add(Failer.name, typeof(Failer));
            _classDic.Add(Runner.name, typeof(Runner));
            _classDic.Add(Succeeder.name, typeof(Succeeder));
            _classDic.Add(Wait.name, typeof(Wait));
            _classDic.Add(MemPriority.name, typeof(MemPriority));
            _classDic.Add(MemSequence.name, typeof(MemSequence));
            _classDic.Add(Priority.name, typeof(Priority));
            _classDic.Add(Sequence.name, typeof(Sequence));
            _classDic.Add(Inverter.name, typeof(Inverter));
            _classDic.Add(Limiter.name, typeof(Limiter));
            _classDic.Add(MaxTime.name, typeof(MaxTime));
            _classDic.Add(Repeater.name, typeof(Repeater));
            _classDic.Add(RepeatUntilFailure.name, typeof(RepeatUntilFailure));
            _classDic.Add(RepeatUntilSuccess.name, typeof(RepeatUntilSuccess));
        }
        public static void Register(string str, Type type)
        {
            _classDic.Add(str, type);
        }
        public static Type GetClassType(string className)
        {
            return _classDic[className];
        }

        private static Dictionary<string, Type> _classDic = new Dictionary<string, Type>();
    }
}
