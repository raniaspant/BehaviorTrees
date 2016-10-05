using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTnamespace;

/* @class ScorePoint
 * @brief In this class, bot has reached its base and is scoring the winning point.
 * This class cannot return a behavior result of FAIL.
 */

namespace CTF
{
    class ScorePoint : Leaf
    {
        BotScript.TeamID myTeamID;

        public override void Initialize(BehaviorTree tree)
        {
            myTeamID = (BotScript.TeamID)tree.getValue(TreeData.MY_TEAMID);
        }

        public override Outcome Process(BehaviorTree tree)
        {
			Debug.Log("score for team:"+myTeamID);
            Application.LoadLevel(0);		//score point - load level from the beginning 
            return Outcome.SUCCESS;
        }
    }
}
