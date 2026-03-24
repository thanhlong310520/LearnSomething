// GoapPlanner.cs
using System.Collections.Generic;
using System.Linq;

public class GoapPlanner
{
    // Node trong cây tìm kiếm A*
    private class Node
    {
        public Node parent;
        public float runningCost;
        public WorldStateData state;
        public GoapAction action;

        public Node(Node parent, float cost, WorldStateData state, GoapAction action)
        {
            this.parent = parent;
            this.runningCost = cost;
            this.state = state;
            this.action = action;
        }
    }

    /// <summary>
    /// Lập kế hoạch: trả về Queue các action cần thực hiện theo thứ tự,
    /// hoặc null nếu không tìm được kế hoạch.
    /// </summary>
    public Queue<GoapAction> Plan(
        WorldStateData currentState,
        Dictionary<string, bool> goal,
        List<GoapAction> availableActions)
    {
        // Lọc ra actions có procedural precondition hợp lệ
        var usableActions = availableActions
            .Where(a => a.CheckProceduralPrecondition(currentState))
            .ToList();

        var start = new Node(null, 0, currentState.Clone(), null);
        var leaves = new List<Node>();

        // Backward search: tìm từ goal ngược lại
        bool found = BuildGraph(start, leaves, usableActions, goal);

        if (!found) return null;

        // Chọn leaf có tổng cost thấp nhất
        Node cheapest = leaves.OrderBy(n => n.runningCost).First();

        // Truy ngược để lấy chuỗi action
        var result = new List<GoapAction>();
        var node = cheapest;
        while (node != null)
        {
            if (node.action != null) result.Insert(0, node.action);
            node = node.parent;
        }

        return new Queue<GoapAction>(result);
    }

    private bool BuildGraph(
        Node parent,
        List<Node> leaves,
        List<GoapAction> actions,
        Dictionary<string, bool> goal)
    {
        bool foundPath = false;

        foreach (var action in actions)
        {
            // Nếu preconditions của action thỏa mãn trong state hiện tại
            if (parent.state.Satisfies(action.Preconditions))
            {
                // Áp dụng effects để tạo state mới
                WorldStateData newState = ApplyEffects(parent.state, action.Effects);

                var node = new Node(parent, parent.runningCost + action.Cost, newState, action);

                // Nếu state mới thỏa mãn goal → tìm được đường!
                if (GoalAchieved(newState, goal))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    // Tiếp tục tìm sâu hơn (DFS với chi phí tích lũy)
                    var subset = actions.Where(a => a != action).ToList();
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found) foundPath = true;
                }
            }
        }

        return foundPath;
    }

    private WorldStateData ApplyEffects(WorldStateData state, Dictionary<string, bool> effects)
    {
        var newState = state.Clone();
        foreach (var kv in effects)
            newState.Set(kv.Key, kv.Value);
        return newState;
    }

    private bool GoalAchieved(WorldStateData state, Dictionary<string, bool> goal)
        => state.Satisfies(goal);
}