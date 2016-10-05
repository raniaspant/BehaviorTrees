using System;

/* @class Selector
 * @brief Class describing a selector's behavior.
 * The selector will return a success result if any of its children returns
 * a success result.
 */

namespace BTnamespace
{
	public class Selector : Composite
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
            if (result == Outcome.FAIL){
                ++_activeChildIndex;
                if (_activeChildIndex >= _children.Count)
                    return Outcome.FAIL;
                else{
                    _children[_activeChildIndex].Initialize(tree);
                    return Outcome.PROCESSING;
                }
            }
            else
                return result;
		}
	}
}

