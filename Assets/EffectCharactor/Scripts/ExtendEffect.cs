using DevLong.Charactor;
using DevLong.Effect;
using DevLong.StatSystem;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


//CONTROL EFFECT
public class StunEffect : Effect
{
    public StunEffect(DataEffect data) : base(data)
    {
    }

    protected override void OnApply()
    {
        charactorInfo.State.AddStun();
    }

    protected override void OnRemove()
    {
        charactorInfo.State.RemoveStun();
    }
}

// STAT EFFECT
public class AttackBuffEffect : Effect
{

    public AttackBuffEffect(DataEffect data) : base(data)
    {
    }

    protected override void OnApply()
    {
        charactorInfo.Stats.damage.AddModifier(new StatModifier(data.quality, data.typeModifier, this));
    }

    protected override void OnRemove()
    {
        charactorInfo.Stats.damage.RemoveModifierFormSource(this);
    }
}


// OVERTIME EFFECT
public class PoisonEffect : Effect
{
    float currentTime;
    public PoisonEffect(DataEffect data) : base (data)  
    {
    }

    protected override void OnApply()
    {
        currentTime = 0;
    }

    protected override void OnTick(float deltaTime)
    {
        currentTime += deltaTime;
        if(currentTime > data.timeDelay)
        {
            charactorInfo.Health.TakeDamage(data.quality);
            currentTime = 0;
        }
    }

    protected override void OnRemove() { }
}
