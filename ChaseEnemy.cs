using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class ChaseEnemy
 * @brief In this class, bot is checking if it should chase an enemy. This decision is made by two facts:
 *  1)either an enemy has the flag, therefore he is a threat
 *	2)or an enemy is in my base and close to the bot that is taking this decision
 */

namespace CTF
{
	class ChaseEnemy : Leaf
	{
		BotScript.TeamID myTeamID;
		
		public override void Initialize(BehaviorTree tree)
		{
			myTeamID = (BotScript.TeamID) tree.getValue(TreeData.MY_TEAMID);
		}
		
		public override Outcome Process(BehaviorTree tree)
		{
			if (myTeamID == (BotScript.TeamID)tree.getValue(TreeData.IN_ARENA)){
				List<GameObject> enemies = tree.getValue(TreeData.ENEMIES_NEARBY) as List<GameObject>;
				if (enemies.Count > 0){
					GameObject closestEnemy = null;
					GameObject flagBearer = null;
					float minDist = 5f;
					float closestDist = 5f;
					bool hasFlag = false;
					Transform MyTransform = (Transform)tree.getValue(TreeData.MY_TRANSFORM);
					for (int i = 0; i < enemies.Count; i++){
						if (enemies[i].CompareTag("Bot")){	
							if((bool)enemies[i].GetComponent<BotScript>().botTree.getValue(TreeData.HAS_FLAG)){ //if the enemy has the flag, mark him
								flagBearer = enemies[i];
								hasFlag = true;
							}
							if (!(bool)enemies[i].GetComponent<BotScript>().botTree.getValue(TreeData.IS_TAGGED)){ //providing he is not tagged, therefore not a threat
								Transform EnemyTransform = (Transform)enemies[i].GetComponent<BotScript>().botTree.getValue(TreeData.MY_TRANSFORM);
								float DistanceWithThisEnemy = (MyTransform.position - EnemyTransform.position).magnitude;
								if (closestDist > DistanceWithThisEnemy){
									closestDist = DistanceWithThisEnemy;
									closestEnemy = enemies[i];
								}
							}
						}
						else{	//if the human player is also one of my enemies, check his position and situation comparing to the flag
							if (!enemies[i].GetComponent<HumanPlayerScript>().isTagged){ //providing he is not tagged, therefore not a threat
								float DistanceWithThisEnemy = (MyTransform.position - enemies[i].transform.position).magnitude;
								if (closestDist > DistanceWithThisEnemy){
									closestDist = DistanceWithThisEnemy;
									closestEnemy = enemies[i];
								}
							}
						}
					}
					if (closestDist <= minDist || hasFlag){ //decide chasing method depending on the previous "investigation"
						if(hasFlag)
							tree.setValue(TreeData.SEEK_THIS, flagBearer);
						else 
							tree.setValue(TreeData.SEEK_THIS, closestEnemy);
						return Outcome.SUCCESS;
					}
					
				}
			}
			return Outcome.FAIL;
		}
	}
}
