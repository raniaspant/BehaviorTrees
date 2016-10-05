using System;

/* @class Node
 * @brief Class Node contains functions about initializing and processing behaviors, as well as
 * defining the possible behavior results : success, fail, processing.
 */

namespace BTnamespace
{
	public enum Outcome
	{
		SUCCESS,
		FAIL,
		PROCESSING
	}

	public abstract class Node
	{
		public abstract void Initialize(BehaviorTree tree);
		public abstract Outcome Process(BehaviorTree tree);
	}
}

