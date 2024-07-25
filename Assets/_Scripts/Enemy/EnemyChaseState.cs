using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


    public class EnemyChaseState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;
        readonly EnemySense sensor;
        
        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player,EnemySense sensor) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
            this.sensor = sensor;
        }

        public override void OnEnter()
        {
            Debug.Log("Chase");
            
            animator.CrossFade(RunHash, 0.1f);
        }

        public override void Update()
        {
       // GetAllTargets();
       
        Debug.Log($"Chasing player at {sensor.player.position}");
        agent.SetDestination(sensor.player.position);
        }
    }


