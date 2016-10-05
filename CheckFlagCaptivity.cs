using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class CheckFlagCaptivity
 * @brief In this class, bot is checking if enemy flag is under captivity.
 */

namespace CTF
{
    class CheckFlagCaptivity : Leaf
    {
        public override void Initialize(BehaviorTree tree)
        {}
		
        public override Outcome Process(BehaviorTree tree)
        {
            Dictionary<GameObject, BotScript.broadcastMessage> myTeamID = tree.getValue(TreeData.STATE) as Dictionary<GameObject, BotScript.broadcastMessage>;

            foreach (KeyValuePair<GameObject, BotScript.broadcastMessage> retrievers in myTeamID){
                if (retrievers.Value == BotScript.broadcastMessage.RETRIEVING_FLAG)
                    return Outcome.SUCCESS;
            }

            return Outcome.FAIL;
        }
    }
}
