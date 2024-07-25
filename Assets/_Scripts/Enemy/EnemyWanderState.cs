using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly Vector3 startPoint;
    readonly List<Transform> patrolPoints;
    public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, List<Transform> patrolPoints): base(enemy, animator)
    {
        this.agent = agent;
        this.startPoint = enemy.transform.position;
        this.patrolPoints = patrolPoints;
    }
    private static System.Random random = new System.Random();

    public static T PickRandomItem<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("The list cannot be null or empty.", nameof(list));
        }

        int randomIndex = random.Next(list.Count);
        return list[randomIndex];
    }
    public override void OnEnter()
    {
        Debug.Log("Entered wander state");
        animator.CrossFade(WalkHash, 0.1f);
    }

    public override void Update()
    {
        if (hasReachedDestination())
        {
            agent.SetDestination(PickRandomItem(patrolPoints).position);
        }
    }

    bool hasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}
