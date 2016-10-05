using System;
using System.Collections.Generic;
using UnityEngine;

using BTnamespace;

/* @class BehaviorTreeScript
 * @brief This is the script that composes the final behavior tree structure and defines the blackboard data.
 */

namespace CTF
{
	//Blackboard Data
    public abstract class TreeData
    {
        public const string MY_TEAMID = "MyTeamID";
        public const string ENEMY_FLAG = "EnemyFlag";
        public const string HAS_FLAG = "HasFlag";
        public const string MY_TRANSFORM = "MyTransform";
        public const string ALL_TEAM_TILES = "TeamTiles";
        public const string RESPAWN_POSITION = "RespawnPosition";
        public const string IS_TAGGED = "IsTagged";
        public const string TAGGED_TEAMMATE = "TaggedTeammate";
        public const string SEEK_THIS = "SeekThis";
        public const string IN_ARENA = "InArena";
        public const string STEER = "Steer";
        public const string VELOCITY = "Velocity";
        public const string ENEMIES_NEARBY = "EnemiesNearby";
        public const string ALLIES = "Allies";
		public const string ENEMIES = "Enemies";
        public const string STATE = "State";
    }

	[System.Serializable]
    public class BehaviorTreeScript : BehaviorTree
    {
        public BehaviorTreeScript()
        {
            //Defence
			
            ChaseEnemy chaseEnemy = new ChaseEnemy();
            Inverter dontChaseEnemy = new Inverter();
            dontChaseEnemy.addChild(chaseEnemy);
            Defend defend = new Defend();
            Seek seekObject = new Seek();
            Sequence Defence = new Sequence();
            Defence.addChild(defend);
            Defence.addChild(seekObject);
            RepeatUntilFail defendUntilFail = new RepeatUntilFail();
            defendUntilFail.addChild(Defence);
            Parallel DefendCheckEnemy = new Parallel();
            DefendCheckEnemy.addChild(dontChaseEnemy);
            DefendCheckEnemy.addChild(defendUntilFail);

            //Chase enemy
			
            Seek seekObjectEnemy = new Seek();
            CatchEnemy catchEnemy = new CatchEnemy();
            Sequence seekCatch = new Sequence();
            seekCatch.addChild(seekObjectEnemy);
            seekCatch.addChild(catchEnemy);
            ChaseEnemy enemyInSight = new ChaseEnemy();
            Parallel seekAndCatch = new Parallel();
            seekAndCatch.addChild(enemyInSight);
            seekAndCatch.addChild(seekCatch);
            Succeeder alwaysCatch = new Succeeder();
            alwaysCatch.addChild(seekAndCatch);
			Selector DefendSelector = new Selector();
            DefendSelector.addChild(DefendCheckEnemy);
            DefendSelector.addChild(alwaysCatch);

            //defend depending on flag captivity status
			
            CheckFlagCaptivity flagCaptured = new CheckFlagCaptivity();
            GoForFlag anyoneGoingForFlag = new GoForFlag();
            Selector anyoneOffensive = new Selector();
            anyoneOffensive.addChild(flagCaptured);
            anyoneOffensive.addChild(anyoneGoingForFlag);
			Parallel DefenceParallel = new Parallel();
            DefenceParallel.addChild(anyoneOffensive);
            DefenceParallel.addChild(DefendSelector);


            //offence

            SeekEnemyFlag findEnemyFlag = new SeekEnemyFlag();
            Seek seekEnemyFlag = new Seek();

            Sequence findSeekEnemyFlag = new Sequence();
            findSeekEnemyFlag.addChild(findEnemyFlag);
            findSeekEnemyFlag.addChild(seekEnemyFlag);


            GoForFlag anyoneFetchingFlag = new GoForFlag();
            Inverter nooneFetchingFlag = new Inverter();
            nooneFetchingFlag.addChild(anyoneFetchingFlag);


            Parallel findFlagParallel = new Parallel();
            findFlagParallel.addChild(nooneFetchingFlag);
            findFlagParallel.addChild(findSeekEnemyFlag);

            //bring the flag back
			
            CaptureFlag captureFlag = new CaptureFlag();
            Seek findIDTile = new Seek();
            ScorePoint score = new ScorePoint();
            Sequence captureReturnScore = new Sequence();
            captureReturnScore.addChild(captureFlag);
            captureReturnScore.addChild(findIDTile);
            captureReturnScore.addChild(score);
            Sequence retrieveFlagParallel = new Sequence();
            retrieveFlagParallel.addChild(findFlagParallel);
            retrieveFlagParallel.addChild(captureReturnScore);
            Selector seekFlagSelector = new Selector();
            seekFlagSelector.addChild(retrieveFlagParallel);

            //Defence or Offence?
			
            Selector DefenceOffence = new Selector();
            DefenceOffence.addChild(DefenceParallel);
            DefenceOffence.addChild(seekFlagSelector);


            Tagged tagged = new Tagged();
            Inverter notTagged = new Inverter();
            notTagged.addChild(tagged);

            Parallel defendOrOffend = new Parallel();
            defendOrOffend.addChild(notTagged);
            defendOrOffend.addChild(DefenceOffence);

            //respawn
			
            ReleaseFlag releaseFlag = new ReleaseFlag();
            Succeeder alwaysReleaseFlag = new Succeeder();
            alwaysReleaseFlag.addChild(releaseFlag);

            Respawn respawn = new Respawn();

            Sequence releaseFlagRespawn = new Sequence();
            releaseFlagRespawn.addChild(alwaysReleaseFlag);
            releaseFlagRespawn.addChild(respawn);


            Selector root = new Selector();
            root.addChild(defendOrOffend);
            root.addChild(releaseFlagRespawn);

            setRoot(root);
        }
    }
}
