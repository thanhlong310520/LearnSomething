// GoapGoal.cs
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapGoal : MonoBehaviour
{
    // Ưu tiên (cao hơn = quan trọng hơn)
    public abstract float Priority { get; }

    // Điều kiện cần đạt được
    public abstract Dictionary<string, bool> DesiredState { get; }

    // Có nên kích hoạt goal này không?
    public virtual bool IsValid(WorldStateData worldState) => true;
}