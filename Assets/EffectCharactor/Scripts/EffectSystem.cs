using DevLong.Charactor;
using System.Collections.Generic;
using UnityEngine;

namespace DevLong.Effect
{

    public class EffectSystem : MonoBehaviour
    {
        public CharactorInfomation infor;
        private List<Effect> activeEffects = new();
        Queue<Effect> effectRemoves = new();
        public void AddEffect(Effect effect)
        {
            effect.Init(infor);
            activeEffects.Add(effect);
        }

        private void Update()
        {
            while (effectRemoves.Count > 0)
            {
                RemoveEffect(effectRemoves.Dequeue());
            }

            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                activeEffects[i].Tick(Time.deltaTime);

                if (activeEffects[i].IsFinished)
                    effectRemoves.Enqueue(activeEffects[i]);  
            }
        }


        public void RemoveEffect(Effect effect)
        {
            activeEffects.Remove(effect);
        }
       
    }
}