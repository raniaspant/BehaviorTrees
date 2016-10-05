using System;

/* @class Decorator
 * @brief Class describing a decorator's structure.
 */
 
namespace BTnamespace
{
	public abstract class Decorator : Node
	{
		protected Node _child;

		public virtual void addChild( Node child )
		{
			_child = child;
		}

		public override void Initialize (BehaviorTree tree)
		{
			_child.Initialize (tree);
		}
	}
}

