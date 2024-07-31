using FiveBabbittGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

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
        //Debug.Log("Investigation entered");
        sensor.RoomCheckComplete = false;
        animator.CrossFade(WalkHash, 0.1f);
        sensor.SusOccurance += ReciecedSusEvent;
        AudioManager.Instance.PlayMusic(enemy.investigationMusic, true);
        if (sensor.eventHeardInRoom) 
        {
            sensor.eventHeardInRoom = false;
            enemy.Suspicion = 75f;

        }

        if (sensor.eventHeardOutOfRoom)
        {
            sensor.eventHeardOutOfRoom = false;
            enemy.Suspicion = 50f;
            agent.SetDestination(sensor.currentRoom.enemyEnterance.position);
            SetSubstate(ReturnSubstateOfType(Enemy.Substates.Substate.CheckingRandomPoints));
        }

        
            
        
    }

    private void ReciecedSusEvent(Transform location, Room room)
    {
        if(sensor.eventHeardInRoom && currentSubstate.state == Enemy.Substates.Substate.CheckingPotentialPoints || currentSubstate.state == Enemy.Substates.Substate.CheckingRandomPoints && enemy.Suspicion <= 75)
        {
            enemy.Suspicion = Mathf.Clamp(enemy.Suspicion + 50f, 0, 100);
            sensor.eventHeardInRoom = false;
            agent.SetDestination(location.position);
            SetSubstate(ReturnSubstateOfType(Enemy.Substates.Substate.CheckingEvent));
        }
    }

    public override void Update()
    {

        if(substateTimer.IsRunning && substateTimer != null)
            substateTimer.Tick(Time.deltaTime);

        Debug.Log($"my current investigation substate is {currentSubstate.state}");
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
        if (currentSubstate.state == Enemy.Substates.Substate.CheckingRandomPoints && enemy.Suspicion >= 50f && !InvestigatingEvent())
        {
            SetSubstate(ReturnSubstateOfType(Enemy.Substates.Substate.CheckingPotentialPoints));
        }

        if(currentSubstate.state == Enemy.Substates.Substate.CheckingPotentialPoints && !InvestigatingEvent())
        {
            enemy.Suspicion = Mathf.Clamp(enemy.Suspicion - 50f, 0, 100);
            sensor.ResetBools();
            sensor.RoomCheckComplete = true;
        }
        if (InvestigatingEvent())
        {
            sensor.ResetBools();
            SetSubstate(ReturnSubstateOfType(Enemy.Substates.Substate.CheckingPotentialPoints));
        }
    }
    private static System.Random random = new System.Random();


    public bool InvestigatingEvent()
    {
        return (currentSubstate.state == Enemy.Substates.Substate.CheckingEvent);
        
    }

    public static T PickRandomItem<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("The list cannot be null or empty.", nameof(list));
        }

        int randomIndex = random.Next(list.Count);
        return list[randomIndex];
    }
    public void SubstateController()
    {
        if (HasReachedDestination() && sensor.inRoom && currentSubstate.state == Enemy.Substates.Substate.CheckingRandomPoints)
        {
            if(InvestigatingEvent())
                { return; }
            var randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;
            randomDirection += startPoint;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            var finalPosition = hit.position;

            agent.SetDestination(finalPosition);
        }

        if(HasReachedDestination() && sensor.inRoom && currentSubstate.state == Enemy.Substates.Substate.CheckingPotentialPoints)
        {
            if (InvestigatingEvent())
            { return; }
            agent.SetDestination(PickRandomItem(room.potentialPlayerLocations).position);
        }

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
