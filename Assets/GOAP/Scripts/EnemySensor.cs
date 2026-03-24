// EnemySensor.cs
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    [SerializeField] private float sightRange = 15f;
    [SerializeField] private LayerMask enemyLayer;

    private WorldState worldState;
    private GoapAgent agent;

    void Awake()
    {
        worldState = GetComponent<WorldState>();
        agent = GetComponent<GoapAgent>();
    }

    void Update()
    {
        bool wasVisible = worldState.Data.Get("enemyVisible");

        // Phát hiện kẻ địch trong tầm nhìn
        Collider[] hits = Physics.OverlapSphere(transform.position, sightRange, enemyLayer);
        bool enemyNow = hits.Length > 0;

        worldState.Data.Set("enemyVisible", enemyNow);

        // Nếu trạng thái thay đổi → replan
        if (wasVisible != enemyNow)
        {
            agent.ForceReplan();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}