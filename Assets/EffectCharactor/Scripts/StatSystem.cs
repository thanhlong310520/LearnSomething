using System.Collections.Generic;
using UnityEngine;

namespace DevLong.StatSystem
{

    // loại biến đổi cho thông số charactor
    public enum ModifierType
    {
        Flat,
        Percent
    }


    // class để lưu chữ thông số biến đổi
    public class StatModifier
    {
        public float value;
        public ModifierType type;
        public object source;
        public StatModifier(float value, ModifierType type, object source)
        {
            this.value = value;
            this.type = type;
            this.source = source;
        }
    }


    // class thông số của charactor, trong đó kèm theo list class lưu thông số biến đổi.
    [System.Serializable]
    public class Stat
    {
        public float baseValue;
        private List<StatModifier> modifiers = new List<StatModifier>(); 
        public float value
        {
            get
            {
                float result = baseValue;
                float percenAdd = 0;
                foreach (StatModifier modifier in modifiers) 
                {
                    if(modifier.type == ModifierType.Flat)
                    {
                        result += modifier.value;
                    }
                    else
                    {
                        percenAdd += modifier.value;
                    }
                }
                result *= (1 + percenAdd);
                return result;
            }
        }

        public void AddModifier(StatModifier modifier) 
        {
            modifiers.Add(modifier);
        }
        public void RemoveModifierFormSource(object source)
        {
            modifiers.RemoveAll(m=> m.source == source);
        }
    }
}