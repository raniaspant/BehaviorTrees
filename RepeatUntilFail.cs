using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* @class RepeatUntilFail
 * @brief Class describing a repeat until failure node's behavior.
 * This type of node has one child and executes it until the child returns a failure result.
 */

namespace BTnamespace
{
    public class RepeatUntilFail : Decorator
    {
        public override Outcome Process(BehaviorTree tree)
        {
			Outcome Result = _child.Process(tree);
			if ( Result != Outcome.FAIL){
				if (Result == Outcome.SUCCESS)
					_child.Initialize(tree);	//Begin again
                return Outcome.PROCESSING;
            }
            return Outcome.FAIL;
        }
    }
}