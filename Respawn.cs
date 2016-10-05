using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class Respawn
 * @brief In this class, bot has been caught by an enemy bot and is required to spawn again in its base.
 * If this class is accessed while a bot is not currently tagged, it returns failure.
 */

namespace CTF
{
    class Respawn : Leaf
    {
        GameObject myGameobject;
        bool isTagged = false;
        Transform myTransform;
        public override void Initialize(BehaviorTree tree)
        {
            myGameobject = ((Transform)tree.getValue(TreeData.MY_TRANSFORM)).gameObject;
            isTagged = (bool)tree.getValue(TreeData.IS_TAGGED);
            myTransform = (Transform)tree.getValue(TreeData.MY_TRANSFORM);
        }
        public override Outcome Process(BehaviorTree tree)
        {
            if (isTagged){
                tree.setValue(TreeData.SEEK_THIS, false);
                myTransform.gameObject.GetComponent<BotScript>().BroadCastTeamMessage(BotScript.broadcastMessage.TAGGED);
                Vector3 respawnPosition = (Vector3) tree.getValue(TreeData.RESPAWN_POSITION);	//locate respawn position and go there instantly
                myGameobject.transform.position = new Vector3(respawnPosition.x, respawnPosition.y, myGameobject.transform.position.z);
				tree.setValue(TreeData.IS_TAGGED,false);
                return Outcome.SUCCESS;
            }

            return Outcome.FAIL;
        }
    }
}
