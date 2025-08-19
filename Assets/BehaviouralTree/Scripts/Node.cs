using System.Collections.Generic;
#region BaseNode
public enum NodeState
{
    Running,
    Success,
    Failure
}

public abstract class Node
{
    protected NodeState _state;
    public NodeState State => _state;

    public abstract NodeState Evaluate();
}
#endregion


#region CompositeNode
/// <summary>
/// SequenceNode
///  1 cai fail la tat ca fail
/// </summary>
public class Sequence : Node
{
    private List<Node> _children;

    public Sequence(List<Node> children) => _children = children;

    public override NodeState Evaluate()
    {
        bool anyRunning = false;

        foreach (var node in _children)
        {
            var result = node.Evaluate();
            switch (result)
            {
                case NodeState.Failure:
                    _state = NodeState.Failure;
                    return _state;
                case NodeState.Running:
                    anyRunning = true;
                    break;
                case NodeState.Success:
                    continue;
            }
        }

        _state = anyRunning ? NodeState.Running : NodeState.Success;
        return _state;
    }
}

/// <summary>
/// Selector node
/// chỉ cần 1 cái true hoặc runing
/// </summary>
public class Selector : Node
{
    private List<Node> _children;

    public Selector(List<Node> children) => _children = children;

    public override NodeState Evaluate()
    {
        foreach (var node in _children)
        {
            var result = node.Evaluate();
            if (result == NodeState.Success || result == NodeState.Running)
            {
                _state = result;
                return _state;
            }
        }

        _state = NodeState.Failure;
        return _state;
    }



}
/// <summary>
/// ParalledNode
/// phải thỏa mãn điều kiện parallel policy đề ra
/// </summary>
public enum ParallelPolicy
{
    RequireOne, // chỉ cần 1 Success thì cả Parallel Success
    RequireAll  // tất cả Success thì mới Success
}

public class Parallel : Node
{
    private List<Node> _children;
    private ParallelPolicy _successPolicy;
    private ParallelPolicy _failurePolicy;

    public Parallel(List<Node> children, ParallelPolicy successPolicy, ParallelPolicy failurePolicy)
    {
        _children = children;
        _successPolicy = successPolicy;
        _failurePolicy = failurePolicy;
    }

    public override NodeState Evaluate()
    {
        int successCount = 0;
        int failureCount = 0;

        foreach (var node in _children)
        {
            var result = node.Evaluate();
            if (result == NodeState.Success) successCount++;
            if (result == NodeState.Failure) failureCount++;
        }

        // Success policy
        if (_successPolicy == ParallelPolicy.RequireOne && successCount > 0)
        {
            _state = NodeState.Success;
            return _state;
        }
        if (_successPolicy == ParallelPolicy.RequireAll && successCount == _children.Count)
        {
            _state = NodeState.Success;
            return _state;
        }

        // Failure policy
        if (_failurePolicy == ParallelPolicy.RequireOne && failureCount > 0)
        {
            _state = NodeState.Failure;
            return _state;
        }
        if (_failurePolicy == ParallelPolicy.RequireAll && failureCount == _children.Count)
        {
            _state = NodeState.Failure;
            return _state;
        }

        _state = NodeState.Running;
        return _state;
    }
}
#endregion