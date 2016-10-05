using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class CaptureFlag
 * @brief In this class, bot is locating the shortest path through the tiles to reach its base while carrying the flag.
 */

namespace CTF
{
    class CaptureFlag : Leaf
    {
        GameObject enemyFlag;
        Transform myTransform;
        GameObject myTiles;

        public override void Initialize(BehaviorTree tree)
        {
            enemyFlag = tree.getValue(TreeData.ENEMY_FLAG) as GameObject;
            myTransform = (Transform) tree.getValue(TreeData.MY_TRANSFORM);
            myTiles = tree.getValue(TreeData.ALL_TEAM_TILES) as GameObject;
			tree.setValue(TreeData.HAS_FLAG, true);
        }

        public override Outcome Process(BehaviorTree tree)
        {
            myTransform.gameObject.GetComponent<BotScript>().BroadCastTeamMessage(BotScript.broadcastMessage.RETRIEVING_FLAG);
            enemyFlag.GetComponent<FlagScript>().Captured(myTransform.gameObject);
            GameObject seekTile;
            float closestDistance = float.MaxValue;
            int ClosestIndex = int.MaxValue;
            for (int i = 0; i < myTiles.transform.childCount; i++){
                float tileDistance = (myTransform.transform.position - myTiles.transform.GetChild(i).transform.position).magnitude;
                if (closestDistance > tileDistance){	//get closer to the base
                    closestDistance = tileDistance;
                    ClosestIndex = i;
                }
            }
            seekTile = myTiles.transform.GetChild(ClosestIndex).gameObject;
            tree.setValue(TreeData.SEEK_THIS, seekTile);
            return Outcome.SUCCESS;
        }
    }
}
