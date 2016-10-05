using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* @class ParallelSelector
 * @brief Class describing a parallel selector's behavior.
 * The parallel will return success if just one child returns success. The parallel selector then proceeds in terminating
 * every child. Same applies if just one child returns failure, the parallel selector will end every child in process and
 * then return failure as well.
 */
 
namespace BTnamespace
{
    public class ParallelSelector : Composite
    {
        private Outcome[] _childStatus;
        public override void Initialize(BehaviorTree tree)
        {
            _childStatus = new Outcome[_children.Count];

            for (int i = 0; i < _children.Count; i++){
                _children[i].Initialize(tree);
                _childStatus[i] = Outcome.PROCESSING;
            }
        }

        public override Outcome Process(BehaviorTree tree)
        {
            for (int i = 0; i < _children.Count; i++){
                if (_childStatus[i] == Outcome.PROCESSING){
                    Outcome result = _children[i].Process(tree);
                    _childStatus[i] = result;
                    if (result == Outcome.SUCCESS)
						return Outcome.SUCCESS;
					if(result == Outcome.FAIL)
						return Outcome.FAIL;
                }
            }
            return Outcome.PROCESSING;
        }
    }
}


