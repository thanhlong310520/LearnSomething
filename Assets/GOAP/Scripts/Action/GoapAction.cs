// GoapAction.cs
using DevLong.Charactor;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    // Chi phí để thực hiện action này (planner tìm đường ít tốn kém nhất)
    public float Cost { get; protected set; } = 1f;

    // Điều kiện CẦN có TRƯỚC khi thực hiện
    protected Dictionary<string, bool> preconditions = new Dictionary<string, bool>();
    // Kết quả SẼ XẢY RA sau khi thực hiện
    protected Dictionary<string, bool> effects = new Dictionary<string, bool>();

    public Dictionary<string, bool> Preconditions => preconditions;
    public Dictionary<string, bool> Effects => effects;

    // Kiểm tra preconditions trong world state thực (runtime check)
    public virtual bool CheckProceduralPrecondition(WorldStateData ws) => true;

    // Bắt đầu thực hiện action
    public abstract bool Perform(WorldStateData ws);

    // Action đã hoàn thành chưa?
    public abstract bool IsDone();

    // Reset để dùng lại
    public virtual void Reset() { }
}

