using System;
using UnityEngine;
namespace BTnamespace
{
    public class Succeeder : Decorator
    {
        public override Outcome Process(BehaviorTree tree)
        {
            Outcome result = _child.Process(tree);
            if (result == Outcome.PROCESSING)
                return Outcome.PROCESSING;
            return Outcome.SUCCESS;
        }
    }
}

