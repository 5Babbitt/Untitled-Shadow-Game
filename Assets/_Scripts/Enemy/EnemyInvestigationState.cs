using FiveBabbittGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyInvestigationState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly Vector3 startPoint;
    readonly float wanderRadius;
    readonly Room room;
    readonly EnemySense sensor;
    public List<Enemy.Substates> substates;
    public Enemy.Substates currentSubstate;
    private CountdownTimer substateTimer;
    public EnemyInvestigationState(Enemy enemy, Animator animator, NavMeshAgent agent, Room room,EnemySense sensor, List<Enemy.Substates> substates) : base(enemy, animator)
    {
        this.agent = agent;
        this.room = room;
        this.startPoint = enemy.transform.position;
        this.sensor = sensor;
        this.substates = substates;
    }



    public override void OnEnter()
    {
        Debug.Log("Wander");
        animator.CrossFade(WalkHash, 0.1f);
    }

    public override void Update()
    {
        if (HasReachedDestination() && sensor.inRoom && currentSubstate.state == Enemy.Substates.Substate.CheckingRandomPoints)
        {
            var randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += startPoint;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            var finalPosition = hit.position;

            agent.SetDestination(finalPosition);
        }
        if(substateTimer.IsRunning)
            substateTimer.Tick(Time.deltaTime);
    }

    public void SetSubstate(Enemy.Substates substate)
    {
        if (substate != null)
        {
            currentSubstate = substate;
            substateTimer = new CountdownTimer(substate.stateTime);
            substateTimer.Start();
            substateTimer.OnTimerStop += TimerStopped;
        }
    }

    private void TimerStopped()
    {
        if (currentSubstate.state == Enemy.Substates.Substate.CheckingRandomPoints)
        {
            SetSubstate(ReturnSubstateOfType(Enemy.Substates.Substate.CheckingPotentialPoints));
        }
    }

    public void SubstateController()
    {


    }

    public Enemy.Substates ReturnSubstateOfType(Enemy.Substates.Substate type)
    {
        foreach (var substate in substates) 
        {
            if (substate.state.Equals(type))
            {
                return substate;
                
            }

        }
        return null;

    }
    bool HasReachedDestination()
    {
        return !agent.pathPending
               && agent.remainingDistance <= agent.stoppingDistance
               && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

}
