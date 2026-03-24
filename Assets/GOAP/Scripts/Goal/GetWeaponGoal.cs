// GoapGoal.cs
using System.Collections.Generic;
using UnityEngine;


// Goal: Lấy vũ khí
public class GetWeaponGoal : GoapGoal
{
    public override float Priority => 5f;

    public override Dictionary<string, bool> DesiredState =>
        new Dictionary<string, bool> { { "hasWeapon", true } };

    public override bool IsValid(WorldStateData ws) => !ws.Get("hasWeapon");
}
