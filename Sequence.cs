using System;

/* @class Sequence
 * @brief Class describing a sequence's behavior.
 * This node runs its children in a sequential order. If a child returns
 * failure behavior result, then the sequence node returns failure as well.
 * If every child until the last one returns a success behavior result, then
 * the sequence node returns a success behavior result as well.
 */

namespace BTnamespace
{
	public class Sequence : Composite
	{
		private int _activeChildIndex = 0;

		public override void Initialize (BehaviorTree tree)
		{
			_activeChildIndex = 0;
			_children [_activeChildIndex].Initialize (tree);
		}

		public override Outcome Process (BehaviorTree tree)
		{
			Outcome result = _children[ _activeChildIndex ].Process (tree);
            if (result == Outcome.SUCCESS){
                ++_activeChildIndex;
                if (_activeChildIndex >= _children.Count)
                    return Outcome.SUCCESS;
                else {
                    _children[_activeChildIndex].Initialize(tree);
                    return Outcome.PROCESSING;
                }
            }
            else
                return result;
		}
	}
}

