using System;
using System.Collections.Generic;

/* @class Composite
 * @brief Class describing a composite node behavior.
 */

namespace BTnamespace
{
	public abstract class Composite : Node
	{
		protected List<Node> _children = new List<Node>();
	
		public virtual void addChild(Node child)
		{
			_children.Add (child);
		}

		public Node at (int index)
        {
            if(_children.Count <= index)
                return null;
            return _children[index];
        }
	}
}

