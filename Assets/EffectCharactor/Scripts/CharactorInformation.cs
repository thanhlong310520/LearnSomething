using DevLong.StatSystem;
using UnityEngine;
using UnityEngine.Events;


namespace DevLong.Charactor
{

    public class CharactorInfomation : MonoBehaviour
    {
        [SerializeField] CharactorStat charactorStat;
        public CharactorStat Stats { get { return charactorStat; } } 


        [SerializeField] CharactorState charactorState;
        public CharactorState State { get { return charactorState; } }


        [SerializeField] Health health;
        public Health Health { get { return health; } } 


    }


    [System.Serializable]
    public class CharactorStat
    {
        public Stat damage = new Stat();
        public Stat speed = new Stat();

    }

    [System.Serializable]
    public class CharactorState
    {
        private int stunCount;
        private int silenceCount;
        
        public CharactorState() 
        {
            stunCount = 0;
            silenceCount = 0;
        }
        public bool IsStunned => stunCount == 0;
        public bool IsSilenced => silenceCount == 0;   

        public void AddStun() { stunCount++; }
        public void RemoveStun() { silenceCount--; }
        public void AddSilence() { silenceCount++; }
        public void RemoveSilence() { stunCount--; }

    }

    [System.Serializable]
    public class Health
    {
        public Stat statHp = new Stat();
        float currentHp;

        public UnityEvent<float, float> eventChangeHp;
        public UnityEvent eventDie;


        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            eventChangeHp?.Invoke(currentHp,statHp.value);

            if(currentHp <= 0)
            {
                Die();
            }
        }

        public void AddHealth(float numberHeal) 
        { 
            currentHp += numberHeal;
            Mathf.Clamp(0, currentHp, statHp.value);
            eventChangeHp?.Invoke(currentHp,statHp.value);
        }


        public void Revive()
        {
            currentHp = statHp.value;
        }

        public void Die()
        {
            eventDie?.Invoke();
        }
    }

}