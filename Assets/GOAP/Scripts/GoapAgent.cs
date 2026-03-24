// GoapAgent.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WorldState))]
public class GoapAgent : MonoBehaviour
{
    private enum AgentState { Idle, MoveTo, PerformAction }

    private GoapPlanner planner;
    private Queue<GoapAction> currentPlan;
    private GoapAction currentAction;
    private AgentState agentState = AgentState.Idle;

    private WorldState worldState;
    private List<GoapGoal> goals;
    private List<GoapAction> actions;
    private GoapGoal activeGoal;

    void Awake()
    {
        planner = new GoapPlanner();
        worldState = GetComponent<WorldState>();
        goals = GetComponents<GoapGoal>().ToList();
        actions = GetComponents<GoapAction>().ToList();
    }

    void Update()
    {
        switch (agentState)
        {
            case AgentState.Idle:
                PlanAndBegin();
                break;

            case AgentState.PerformAction:
                ExecuteCurrentAction();
                break;
        }
    }

    private void PlanAndBegin()
    {
        // Chọn goal có priority cao nhất còn valid
        activeGoal = goals
            .Where(g => g.IsValid(worldState.Data))
            .OrderByDescending(g => g.Priority)
            .FirstOrDefault();

        if (activeGoal == null) return;

        // Lập kế hoạch
        currentPlan = planner.Plan(worldState.Data, activeGoal.DesiredState, actions);

        if (currentPlan != null && currentPlan.Count > 0)
        {
            currentAction = currentPlan.Dequeue();
            currentAction.Reset();
            agentState = AgentState.PerformAction;
            Debug.Log($"[GOAP] {name} → Goal: {activeGoal.GetType().Name}, Plan: {currentPlan.Count + 1} actions");
        }
        else
        {
            Debug.LogWarning($"[GOAP] {name} không tìm được kế hoạch cho {activeGoal.GetType().Name}");
        }
    }

    private void ExecuteCurrentAction()
    {
        if (currentAction == null) { agentState = AgentState.Idle; return; }

        // Thực hiện action
        bool success = currentAction.Perform(worldState.Data);

        if (!success)
        {
            // Action thất bại → replan
            Debug.Log($"[GOAP] Action {currentAction.GetType().Name} thất bại, replanning...");
            agentState = AgentState.Idle;
            return;
        }

        if (currentAction.IsDone())
        {
            // Chuyển sang action tiếp theo
            if (currentPlan.Count > 0)
            {
                currentAction = currentPlan.Dequeue();
                currentAction.Reset();
            }
            else
            {
                // Hoàn thành kế hoạch → quay lại Idle để chọn goal mới
                Debug.Log($"[GOAP] {name} hoàn thành goal: {activeGoal.GetType().Name}");
                agentState = AgentState.Idle;
            }
        }
    }

    // Gọi từ bên ngoài khi có thay đổi lớn (ví dụ: bị tấn công)
    public void ForceReplan()
    {
        currentPlan = null;
        currentAction = null;
        agentState = AgentState.Idle;
    }
}