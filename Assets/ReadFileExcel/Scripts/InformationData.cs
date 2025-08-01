

using System;

namespace ReadFileJson
{
    [Serializable]
    public class InformationData
    {
        public string id;
        public string name;
        public string damage;
        public string hp;
        public string skill;
        public string skill_rate;
        public string ad_require;
    }

    [Serializable]
    public class WrapperArray<T>
    {
        public T[] Data;
    }

}