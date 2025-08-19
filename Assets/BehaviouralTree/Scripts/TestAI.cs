using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour, IHealth, IAttack,IHealing
{
    public int hp = 100;
    public Transform basePosition;
    public RunningDataNode runningData;
    public LayerMask layerPlayer = -1;
    Node _root;

    public int GetHealth()
    {
        return hp;
    }

    public void HandleAttack()
    {
        StartCoroutine(EndAttack());
    }

    public void Heal()
    {
        hp = 100; // Reset health to full
        print("Healed to full health");
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(10f); // Simulate attack duration
        runningData.IsLooking = false;
        print("Attack ended");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckHealth(this,30),
                new GoToBase(transform, basePosition, 3f,this)
            }),

            new Sequence(new List<Node>
            {
                new FindPlayer(transform, 10f, layerPlayer, runningData),
                new MoveToTarget(transform, runningData, 10f, 2f),
                new AttackPlayer(runningData,this)
            }),

            new Patrol()
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (runningData.IsLooking)
        {
            return;
        }
        _root.Evaluate();
    }
}
