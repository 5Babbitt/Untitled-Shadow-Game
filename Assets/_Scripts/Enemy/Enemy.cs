using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(EnemySense))]
public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject patrolPointParent;
    private List<Transform> patrolPoints = new();
    public EnemySense enemySense;
    StateMachine stateMachine;

    //   private void OnValidate() => this.ValidateRefs();

    private void Awake()
    {
        SetPatrolPoints();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemySense = GetComponent<EnemySense>();
    }

    private void Start()
    {

        stateMachine = new StateMachine();
        var wanderState = new EnemyWanderState(this, animator, agent, patrolPoints);
        var chaseState = new EnemyChaseState(this, animator, agent, enemySense.player,enemySense);
        
        At(from:wanderState,to:chaseState,condition:new FuncPredicate(() => enemySense.CanDetectPlayer()));
        At(from: chaseState, to: wanderState, condition: new FuncPredicate(() => !enemySense.CanDetectPlayer()));
        stateMachine.SetState(wanderState);
    }

    void SetPatrolPoints()
    {
        Transform[] allTransforms = patrolPointParent.GetComponentsInChildren<Transform>();

        foreach (Transform t in allTransforms)
        {
            if (t != patrolPointParent)
            {
                patrolPoints.Add(t);
            }
        }
    }

    void At(IState from,  IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    private void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Count == 0)
            return;

        Gizmos.color = Color.yellow;

        foreach (Transform patrolPoint in patrolPoints)
        {
            Gizmos.DrawSphere(patrolPoint.position, 0.3f);
        }

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            Vector3 currentPoint = patrolPoints[i].position;
            Vector3 nextPoint = patrolPoints[(i + 1) % patrolPoints.Count].position;
            Gizmos.DrawLine(currentPoint, nextPoint);
        }
    }
}
