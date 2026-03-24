// --- Ví dụ: Nhặt vũ khí ---
using UnityEngine;

public class PickupWeaponAction : GoapAction
{
    [SerializeField] private float pickupRange = 2f;
    private Transform weaponTransform;
    private bool done = false;

    void Awake()
    {
        preconditions["hasWeapon"] = false;        // Chưa có vũ khí
        effects["hasWeapon"] = true;               // Sau khi nhặt xong sẽ có
        Cost = 2f;
    }

    public override bool CheckProceduralPrecondition(WorldState ws)
    {
        // Tìm vũ khí gần nhất trong scene
        var weapons = GameObject.FindGameObjectsWithTag("Weapon");
        float minDist = float.MaxValue;
        foreach (var w in weapons)
        {
            float d = Vector3.Distance(transform.position, w.transform.position);
            if (d < minDist) { minDist = d; weaponTransform = w.transform; }
        }
        return weaponTransform != null;
    }

    public override bool Perform(WorldState ws)
    {
        if (weaponTransform == null) return false;

        // Di chuyển đến vũ khí
        // (thực tế dùng NavMeshAgent)
        transform.position = Vector3.MoveTowards(
            transform.position, weaponTransform.position, 3f * Time.deltaTime);

        if (Vector3.Distance(transform.position, weaponTransform.position) < pickupRange)
        {
            ws.Set("hasWeapon", true);
            Destroy(weaponTransform.gameObject);
            done = true;
        }
        return true;
    }

    public override bool IsDone() => done;
    public override void Reset() => done = false;
}
