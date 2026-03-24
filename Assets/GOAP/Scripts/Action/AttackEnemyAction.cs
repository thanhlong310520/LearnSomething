
// --- Ví dụ: Tấn công kẻ địch ---
using UnityEngine;

public class AttackEnemyAction : GoapAction
{
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float damage = 20f;
    private Transform enemy;
    private bool done = false;

    void Awake()
    {
        preconditions["hasWeapon"] = true;
        preconditions["enemyVisible"] = true;
        effects["enemyDead"] = true;
        Cost = 1f;
    }

    public override bool CheckProceduralPrecondition(WorldStateData ws)
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = float.MaxValue;
        foreach (var e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < minDist) { minDist = d; enemy = e.transform; }
        }
        return enemy != null;
    }

    public override bool Perform(WorldStateData ws)
    {
        if (enemy == null) { done = true; return true; }

        if (Vector3.Distance(transform.position, enemy.position) > attackRange)
        {
            // Di chuyển lại gần
            transform.position = Vector3.MoveTowards(
                transform.position, enemy.position, 4f * Time.deltaTime);
        }
        else
        {
            // Tấn công
            var health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                if (health.IsDead)
                {
                    ws.Set("enemyDead", true);
                    done = true;
                }
            }
        }
        return true;
    }

    public override bool IsDone() => done;
    public override void Reset() { done = false; enemy = null; }
}