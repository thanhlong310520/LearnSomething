using DevLong.Charactor;
using UnityEngine;

namespace DevLong.Effect
{
    public abstract class Effect
    {
        protected DataEffect data;
        protected float timeLeft;
        protected CharactorInfomation charactorInfo;
        
        public Effect(DataEffect data)
        {
            this.data = data;
        }
        public void Init(CharactorInfomation charactorInfo)
        {
            this.charactorInfo = charactorInfo;
            timeLeft = data.duration;
            OnApply();
        }

        public void Tick(float deltaTime)
        {
            timeLeft -= deltaTime;
            OnTick(deltaTime);

            if (timeLeft <= 0)
                OnRemove();
        }

        public bool IsFinished => timeLeft <= 0;

        protected abstract void OnApply();
        protected abstract void OnRemove();
        protected virtual void OnTick(float deltaTime) { }
    }

    public enum EffectType 
    {
        None = 0,
        BuffDamage,
        BuffAttackSpeed,
        BuffSpeedMove,
        
        Slow,
        Stun,
        Silence,
    }
}