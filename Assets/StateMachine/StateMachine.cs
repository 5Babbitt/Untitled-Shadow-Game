using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransitions = new();
    class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get;}

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to,IPredicate Condition)
        {
            Transitions.Add(item:new Transition(to,Condition));
        }
    }
    
}
