using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class SeekEnemyFlag
 * @brief In this class, the bot is seeking for the flag, after being the one chosen to go for it.
 */

namespace CTF
{
    class SeekEnemyFlag : Leaf
    {
		Transform myTransfrom;
        public override void Initialize(BehaviorTree tree)
        {
            myTransfrom = (Transform)tree.getValue(TreeData.MY_TRANSFORM);
            myTransfrom.gameObject.GetComponent<BotScript>().BroadCastTeamMessage(BotScript.broadcastMessage.GOING_TO_FLAG);
        }

        public override Outcome Process(BehaviorTree tree)
        {
            GameObject EnemyFlag = tree.getValue(TreeData.ENEMY_FLAG) as GameObject;
            tree.setValue(TreeData.SEEK_THIS, EnemyFlag);
            return Outcome.SUCCESS;
        }
    }
}
