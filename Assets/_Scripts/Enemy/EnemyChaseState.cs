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
        //enemy.OnSearchCooldown = true;
        AudioManager.Instance.PlayMusic(enemy.chaseMusic, true);
            animator.CrossFade(RunHash, 0.1f);
        }

        public override void Update()
        {
       // GetAllTargets();
       
        //Debug.Log($"Chasing player at {sensor.player.position}");
        agent.speed = 5;
        agent.angularSpeed = 180f;
        agent.SetDestination(sensor.player.position);
        }
    }


