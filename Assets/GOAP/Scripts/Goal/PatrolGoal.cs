// GoapGoal.cs
using System.Collections.Generic;
using UnityEngine;

// Goal: Tuần tra
public class PatrolGoal : GoapGoal
{
    public override float Priority => 1f;

    public override Dictionary<string, bool> DesiredState =>
        new Dictionary<string, bool> { { "isPatrolling", true } };
}