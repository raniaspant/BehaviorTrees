using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* @class Inverter
 * @brief Class describing an inverter's behavior.
 * Whatever result the inverter's child returns, gets... inverted. Unless there is a PROCESSING
 * outcome, in that case it stays the same.
 */

namespace BTnamespace
{
    public class Inverter : Decorator
    {
        public override Outcome Process(BehaviorTree tree)
        {
            Outcome Result = _child.Process(tree);
            if (Result == Outcome.FAIL)
                return Outcome.SUCCESS;
            else if (Result == Outcome.SUCCESS)
                return Outcome.FAIL;
            else
                return Outcome.PROCESSING;
        }
    }
}
