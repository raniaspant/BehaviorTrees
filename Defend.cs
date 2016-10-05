using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class Defend
 * @brief In this class, bot is choosing random tiles to navigate to, as an implementation of a defensive behavior.
 */

namespace CTF
{
    class Defend : Leaf
    {
        GameObject myTiles;
        Transform MyTransfrom;
		
        public override void Initialize(BehaviorTree tree)
        {
            myTiles = tree.getValue(TreeData.ALL_TEAM_TILES) as GameObject;
            MyTransfrom = (Transform)tree.getValue(TreeData.MY_TRANSFORM);
        }

        public override Outcome Process(BehaviorTree tree)
        {
            MyTransfrom.gameObject.GetComponent<BotScript>().BroadCastTeamMessage(BotScript.broadcastMessage.DEFENDING);
			UnityEngine.Random.seed = (Guid.NewGuid().GetHashCode());
			int randomTile;
			if(myTiles.transform.childCount > 0)
				randomTile = UnityEngine.Random.Range(0, MaxCount);	//get a random tile from the ones I am allowed to navigate to
            tree.setValue(TreeData.SEEK_THIS, myTiles.transform.GetChild(randomTile).gameObject);
            return Outcome.SUCCESS;
        }
    }
}
