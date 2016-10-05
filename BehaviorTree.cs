using System;
using System.Collections.Generic;
using UnityEngine;

/* @class BehaviorTree
 * @brief Behavior Tree class, including routines for getValue, setRoot, setValue.
 */

namespace BTnamespace
{
	[System.Serializable]
	public class BehaviorTree
	{
		private Node _root;
        private Dictionary<string, object> _blackboardData = new Dictionary<string, object>();
		
        public object getValue(string name)
		{
			return _blackboardData [name];
		}
		
        public void setValue(string name, object value)
		{
			_blackboardData [name] = value;
		}

		public void setRoot(Node root)
		{
			_root = root;
		}

		public void Initialize()
		{
			_root.Initialize( this );
		}

		public Outcome Process()
		{
			return _root.Process( this );
		}
	}
}