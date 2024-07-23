
using UnityEngine;
public abstract class BaseState : IState
{

    public PlayerController playerController;
    public Animator animator;

    protected static readonly int LocomotionHash = Animator.StringToHash(name: "Locomotion");
    
    
    protected BaseState(PlayerController player, Animator animator) 
    { 
        this.playerController = player;
        this.animator = animator;
    }


    public virtual void FixedUpdate()
    {
        //noop
    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnExit()
    {

    }
    public virtual void Update()
    {

    }
}