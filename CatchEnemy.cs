using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class CatchEnemy
 * @brief In this class, bot is catching an enemy == tagging him.
 */

namespace CTF
{
    class CatchEnemy : Leaf
    {

        public override void Initialize(BehaviorTree tree)
        {}

        public override Outcome Process(BehaviorTree tree)
        {
            GameObject enemy = tree.getValue(TreeData.SEEK_THIS) as GameObject;
            if (enemy && enemy.CompareTag("Bot")){
                enemy.GetComponent<BotScript>().botTree.setValue(TreeData.IS_TAGGED, true);
                tree.setValue(TreeData.SEEK_THIS, null);		//I don't have anything to seek for now
                return Outcome.SUCCESS;
            }
            else if (enemy && enemy.CompareTag("HumanPlayer")){	//Same applies if the enemy-threat is the human player
                enemy.GetComponent<HumanPlayerScript>().catchHuman();
				return Outcome.SUCCESS;
            }
            else
                return Outcome.FAIL; 
        }
    }
}
