using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class GoForFlag
 * @brief In this class, bot is required to check if any of its teammates is going for the flag, compares distances and chooses accordingly.
 * If any teammate is going for the flag, class returns behavior result of success, otherwise behavior result of failure.
 */

namespace CTF
{
    class GoForFlag : Leaf
    {
        GameObject HumanPlayer = null;
        GameObject enemyFlag = null;
        BotScript.TeamID myTeamID;
        public override void Initialize(BehaviorTree tree)
        {
            List<GameObject> allies = tree.getValue(TreeData.ALLIES) as List<GameObject>;
            for (int i = 0; i < allies.Count; i++){		//construct a list with my bot allies and/or the human player
                if(allies[i].CompareTag("HumanPlayer"))
                    HumanPlayer = allies[i];
            }
            enemyFlag = tree.getValue(TreeData.ENEMY_FLAG) as GameObject;
            myTeamID = (BotScript.TeamID)tree.getValue(TreeData.MY_TEAMID);

        }

        public override Outcome Process(BehaviorTree tree)
        {
            Transform MyTransform = (Transform)tree.getValue(TreeData.MY_TRANSFORM);

            if (HumanPlayer && HumanPlayer.GetComponent<HumanPlayerScript>().inBase != myTeamID && !HumanPlayer.GetComponent<HumanPlayerScript>().isTagged){
                float theirDistance = (new Vector3(HumanPlayer.transform.position.x - enemyFlag.transform.position.x, HumanPlayer.transform.position.y - enemyFlag.transform.position.y, 0.0f)).sqrMagnitude;
                float myDistance = (new Vector3(MyTransform.transform.position.x - enemyFlag.transform.position.x, MyTransform.transform.position.y - enemyFlag.transform.position.y, 0.0f)).sqrMagnitude;
                if (theirDistance <= myDistance)	//compare distance I have with the distance my teammates do
                    return Outcome.SUCCESS;
            }

            Dictionary<GameObject, BotScript.broadcastMessage> MymyTeamID = tree.getValue(TreeData.STATE) as Dictionary<GameObject, BotScript.broadcastMessage>;
            foreach (KeyValuePair<GameObject, BotScript.broadcastMessage> teammatesMessage in MymyTeamID){	//check if any teammate is broadcasting "going" or "retrieving" the flag
				if (teammatesMessage.Value == BotScript.broadcastMessage.GOING_TO_FLAG || teammatesMessage.Value == BotScript.broadcastMessage.RETRIEVING_FLAG)
                    return Outcome.SUCCESS;
            }
            return Outcome.FAIL;
        }
    }
}
