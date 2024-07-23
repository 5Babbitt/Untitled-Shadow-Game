using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : ITransition
{
    // Start is called before the first frame update
 
        public IState To { get; }
        public IPredicate Condition { get; }

        public Transition(IState to, IPredicate condition) {
            To = to;
            Condition = condition;
        }
    
}
