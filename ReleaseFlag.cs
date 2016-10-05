using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class ReleaseFlag
 * @brief In this class, bot is required to drop the flag because it is currently tagged.
 * Results in failure if the bot doesn't currently possess the flag.
 */

namespace CTF
{
	class ReleaseFlag : Leaf
	{
		Transform MyTransform;
		public override void Initialize(BehaviorTree tree)
		{
			MyTransform = (Transform)tree.getValue(TreeData.MY_TRANSFORM);
		}
		
		public override Outcome Process(BehaviorTree tree)
		{
			bool hasFlag = (bool)tree.getValue(TreeData.HAS_FLAG);
			if (hasFlag){
				tree.setValue(TreeData.HAS_FLAG, false);
				tree.setValue(TreeData.SEEK_THIS, false);
				GameObject enemyFlag = tree.getValue(TreeData.ENEMY_FLAG) as GameObject;
				enemyFlag.GetComponent<FlagScript>().Release(MyTransform.gameObject);
				return Outcome.SUCCESS;
			}
			return Outcome.FAIL;
		}
	}
}