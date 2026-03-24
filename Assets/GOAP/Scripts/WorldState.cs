// WorldState.cs
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    private Dictionary<string, bool> states = new Dictionary<string, bool>();

    public void Set(string key, bool value) => states[key] = value;

    public bool Get(string key)
    {
        states.TryGetValue(key, out bool val);
        return val;
    }

    public bool Contains(string key) => states.ContainsKey(key);

    // Kiểm tra xem một tập điều kiện có thỏa mãn không
    public bool Satisfies(Dictionary<string, bool> conditions)
    {
        foreach (var kv in conditions)
        {
            if (!states.TryGetValue(kv.Key, out bool val) || val != kv.Value)
                return false;
        }
        return true;
    }

    // Tạo bản sao để planner mô phỏng
    public WorldState Clone()
    {
        var clone = new WorldState();
        foreach (var kv in states)
            clone.Set(kv.Key, kv.Value);
        return clone;
    }
}