using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : IState
{
    protected readonly Enemy enemy;
    protected readonly Animator animator;
    protected static readonly int IdleHash = Animator.StringToHash(name: "Idle");
    protected static readonly int RunHash = Animator.StringToHash(name: "Run");
    protected static readonly int WalkHash = Animator.StringToHash(name: "Walk");
    protected static readonly int AttackHash = Animator.StringToHash(name: "Attack");
    
    public EnemySense enemySense;

    protected EnemyBaseState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
       
    }
    public virtual void FixedUpdate()
    {
        //no op
    }

    public virtual void OnEnter()
    {
       //no op
    }

    public virtual void OnExit()
    {
        //no op
    }

    public virtual void Update()
    {
        
    }
}
