// GoapGoal.cs
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyGoal : GoapGoal
{
    public override float Priority => 10f;

    public override Dictionary<string, bool> DesiredState =>
        new Dictionary<string, bool> { { "enemyDead", true } };

    public override bool IsValid(WorldStateData ws) => ws.Get("enemyVisible");
}
