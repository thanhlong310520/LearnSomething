using DevLong.StatSystem;
using UnityEngine;
namespace DevLong.Effect
{
    [CreateAssetMenu(fileName = "DataEffect", menuName = "Scriptable Objects/DataEffect")]
    public class DataEffect : ScriptableObject
    {
        public EffectType effectType;
        public float duration;

        public ModifierType typeModifier;
        public float quality;
        public float timeDelay;
    }
}