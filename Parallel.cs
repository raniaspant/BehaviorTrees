using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* @class Parallel
 * @brief Class describing a parallel's behavior.
 * The parallel will return success if every child returns success. Even if only one child returns failure
 * then the parallel returns failure as well.
 */
 
namespace BTnamespace
{
    public class Parallel : Composite
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
            bool childInProcess = false;
            for(int i = 0; i < _children.Count; i++){
				Outcome result = _children[i].Process(tree);
				_childStatus[i] = result;
				if (result == Outcome.FAIL)
					return Outcome.FAIL;
				if(result == Outcome.PROCESSING)
					childInProcess = true;
            }
            if (childInProcess){
				for(int i = 0; i < _children.Count; i++){
					if(_childStatus[i] == Outcome.SUCCESS){
						_children[i].Initialize(tree);
						_childStatus[i] = Outcome.PROCESSING;
					}
				}
				return Outcome.PROCESSING;
            }
			return Outcome.SUCCESS;
        }
    }
}