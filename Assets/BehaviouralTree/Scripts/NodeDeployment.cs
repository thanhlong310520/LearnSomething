using System;
using UnityEngine;



[Serializable]
public class RunningDataNode
{
    public Transform target;
    public bool IsLooking;
}


public class CheckHealth : Node
{
    private IHealth health;
    private int numberHealConstance = 0; // Default value if not specified

    public CheckHealth(IHealth health, int numberHealConstance)
    {
        this.health = health;
        this.numberHealConstance = numberHealConstance;
    }

    public override NodeState Evaluate()
    {
        _state = health.GetHealth() < numberHealConstance ? NodeState.Success : NodeState.Failure;
        return _state;
    }
}



public class FindPlayer : Node
{
    private Transform _npc;
    private float _range;
    private LayerMask _playerMask;
    private RunningDataNode runningData;

    public FindPlayer(Transform npc, float range, LayerMask playerMask, RunningDataNode runningData)
    {
        _npc = npc;
        _range = range;
        _playerMask = playerMask;
        this.runningData = runningData;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("finding ");
        if (runningData.target != null) return NodeState.Success;

        Collider2D[] hits = Physics2D.OverlapCircleAll(_npc.position, _range, _playerMask);

        if (hits.Length > 0)
        {
            // chọn player đầu tiên
            runningData.target = hits[0].transform;
            Debug.Log("found " + hits[0].name + " at " + runningData.target.position);
            _state = NodeState.Success;
        }
        else
        {
            runningData.target = null;
            _state = NodeState.Failure;
        }

        return _state;
    }
}
public class IsPlayerInSight : Node
{
    private Transform selfTransform;
    private RunningDataNode runningData;
    private float _range;

    public IsPlayerInSight(Transform selfTransform,RunningDataNode runningData, float range)
    {
        this.selfTransform = selfTransform;
        this.runningData = runningData;
        _range = range;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(selfTransform.position, runningData.target.position);
        _state = dist < _range ? NodeState.Success : NodeState.Failure;
        if (_state == NodeState.Failure)
        {
            runningData.target = null; // Clear target if not in sight
        }
        else
        {
            Debug.Log("Player is in sight!");
        }
        return _state;
    }
}

public class MoveToTarget : Node
{
    private Transform selfTransform;
    private RunningDataNode runningData;
    private float _speed;
    private float minDistanceMove;

    public MoveToTarget(Transform selfTransform, RunningDataNode runningData, float speed, float minDistanceMove)
    {
        this.selfTransform = selfTransform;
        _speed = speed;
        this.minDistanceMove = minDistanceMove;
        this.runningData = runningData;
    }

    public override NodeState Evaluate()
    {
        if(runningData.target == null)
        {
            Debug.Log("move to target");
            _state = NodeState.Failure;
            return _state;
        }

        float dist = Vector3.Distance(selfTransform.position, runningData.target.position);
        if (dist < minDistanceMove)
        {
            _state = NodeState.Success;
            return _state;
        }

        selfTransform.position = Vector3.MoveTowards(
            selfTransform.position,
            runningData.target.position,
            _speed * Time.deltaTime
        );

        _state = NodeState.Running;
        return _state;
    }
}

public class AttackPlayer : Node
{

    private RunningDataNode runningData;
    private IAttack attackHandle;
    public AttackPlayer(RunningDataNode runningData, IAttack attackHandle)
    {
        this.runningData = runningData;
        this.attackHandle = attackHandle;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Start Attacking player!");
        _state = NodeState.Success;
        attackHandle.HandleAttack();
        runningData.IsLooking = true;
        return _state;
    }
}

public class Patrol : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("Patrolling...");
        _state = NodeState.Running;
        return _state;
    }
}

public class GoToBase : Node
{
    private Transform _npc, _base;
    private IHealing healingHandle;
    private float _speed;

    public GoToBase(Transform npc, Transform baseTransform, float speed, IHealing healingHandle)
    {
        _npc = npc;
        _base = baseTransform;
        _speed = speed;
        this.healingHandle = healingHandle;
    }

    public override NodeState Evaluate()
    {
        _npc.position = Vector3.MoveTowards(
            _npc.position,
            _base.position,
            _speed * Time.deltaTime
        );

        Debug.Log("Returning to base to heal...");
        if(Vector3.Distance(_npc.position,_base.position) < 0.1f)
        {
            Debug.Log("Reached base!");
            _state = NodeState.Success;
            healingHandle.Heal();
            return _state;
        }
        _state = NodeState.Running;
        return _state;
    }
}