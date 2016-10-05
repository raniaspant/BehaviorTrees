using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTnamespace;

/* @class BotScript
 * @brief This is the script that Initializes all information about a bot, regarding its team and its team's tiles, flag position etc.
 * One of the most valuable aspects implemented is the way the bot is keeping track of the enemies that are within its vicinity.
 */

namespace CTF
{
    public class BotScript : MonoBehaviour
    {
        public enum TeamID
        {
            RIGHT,
            LEFT
        }

        public enum broadcastMessage
        {
            DEFENDING,
            GOING_TO_FLAG,
            TAGGED,
            RETRIEVING_FLAG
        }

		public bool hasFlag;
		public bool isTagged;
		public broadcastMessage currentBroadcast;
        public float steer;
        public float velocity;
        public TeamID myTeamID;
        public BehaviorTreeScript botTree;
        public Dictionary<GameObject, broadcastMessage> myState = new Dictionary<GameObject, broadcastMessage>();

        // Initializeialize everything concerning the bot. Team tiles, flag position, allies, respawn position
        void Start()
        {
            botTree = new BehaviorTreeScript();
			List<GameObject> enemiesNearby = new List<GameObject>();
			List<GameObject> enemies = new List<GameObject>();
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Bot");
            GameObject HumanPlayer = GameObject.FindGameObjectWithTag("HumanPlayer");
            List<GameObject> allies = new List<GameObject>();
            for (int i = 0; i < allPlayers.Length; i++){
                if ((allPlayers[i] != this.gameObject) && (allPlayers[i].GetComponent<BotScript>().myTeamID == myTeamID))
                    allies.Add(allPlayers[i]);
				if((allPlayers[i]!=this.gameObject) && (allPlayers[i].GetComponent<BotScript>().myTeamID!=myTeamID))
					enemies.Add(allPlayers[i]);
            }

            if (HumanPlayer.GetComponent<HumanPlayerScript> ().myTeamID == myTeamID)
				allies.Add (HumanPlayer);
			else
				enemies.Add (HumanPlayer);

            currentBroadcast = broadcastMessage.DEFENDING;

			botTree.setValue(TreeData.ALLIES, allies);
			botTree.setValue(TreeData.STATE, myState);
            GameObject seekThis = null;
            botTree.setValue(TreeData.SEEK_THIS, seekThis);
            botTree.setValue(TreeData.IN_ARENA, myTeamID);
			botTree.setValue (TreeData.ENEMIES, enemies);
            botTree.setValue(TreeData.ENEMIES_NEARBY, enemiesNearby);
            botTree.setValue(TreeData.MY_TEAMID, myTeamID);
            botTree.setValue(TreeData.STEER, steer);
            botTree.setValue(TreeData.VELOCITY, velocity);
            botTree.setValue(TreeData.HAS_FLAG, false);
			botTree.setValue(TreeData.IS_TAGGED, false);
			botTree.setValue(TreeData.MY_TRANSFORM, transform);
            if (myTeamID == TeamID.RIGHT){
                botTree.setValue(TreeData.ALL_TEAM_TILES, GameObject.FindGameObjectWithTag("RightTiles"));
                botTree.setValue(TreeData.ENEMY_FLAG, GameObject.Find("LeftFlag"));
                botTree.setValue(TreeData.RESPAWN_POSITION, GameObject.Find("RightRespawnNode").transform.position);
             }
            else{
                botTree.setValue(TreeData.ALL_TEAM_TILES, GameObject.FindGameObjectWithTag("LeftTiles"));
                botTree.setValue(TreeData.ENEMY_FLAG, GameObject.Find("RightFlag"));
                botTree.setValue(TreeData.RESPAWN_POSITION, GameObject.Find("LeftRespawnNode").transform.position);
            }

            botTree.Initialize();
            transform.rotation = Quaternion.Euler(0f, -90f, 90f);
        }

        // Update is called once per frame
        void Update()
        {
			hasFlag = (bool) botTree.getValue(TreeData.HAS_FLAG);
			isTagged = (bool) botTree.getValue(TreeData.IS_TAGGED); 
            botTree.setValue(TreeData.MY_TRANSFORM, transform);
            if(Outcome.PROCESSING != botTree.Process())
                botTree.Initialize();
        }

        public void Message(GameObject PlayerObj, broadcastMessage PlayerMessage)
        {
            if (myState.ContainsKey(PlayerObj))
                myState[PlayerObj] = PlayerMessage;
            else
                myState.Add(PlayerObj, PlayerMessage);
			botTree.setValue(TreeData.STATE, myState);
        }

        public void BroadCastTeamMessage(broadcastMessage MyMessage)
        {
			currentBroadcast = MyMessage;
            List<GameObject> allies = botTree.getValue(TreeData.ALLIES) as List<GameObject>;

            for (int i = 0; i < allies.Count; i++){
				if(allies[i].CompareTag("Bot"))
                	allies[i].GetComponent<BotScript>().Message(gameObject, MyMessage);
            }
        }

		//add a nearby enemy on collision 
        void OnTriggerEnter(Collider otherPlayer)
        {
			bool enemyFound = false;
            if ((otherPlayer.gameObject.tag == "Bot") && (otherPlayer.GetComponent<BotScript>().myTeamID != myTeamID))
				enemyFound = true;
			else if(otherPlayer.gameObject.tag == "HumanPlayer" && otherPlayer.GetComponent<HumanPlayerScript>().myTeamID != myTeamID)
				enemyFound = true;
			if(enemyFound){
				List<GameObject> enemiesNearby = botTree.getValue(TreeData.ENEMIES_NEARBY) as List<GameObject>;
				enemiesNearby.Add(otherPlayer.gameObject);
				botTree.setValue(TreeData.ENEMIES_NEARBY, enemiesNearby);
			}
		}
		
		//remove the enemy while exiting collision
		void OnTriggerExit(Collider otherPlayer)
        {
			bool enemyFound = false;
			if ((otherPlayer.gameObject.tag == "Bot") && (otherPlayer.GetComponent<BotScript>().myTeamID != myTeamID))
				enemyFound = true;
			else if(otherPlayer.gameObject.tag == "HumanPlayer" && otherPlayer.GetComponent<HumanPlayerScript>().myTeamID != myTeamID)
				enemyFound = true;

			if (enemyFound){
                GameObject CurrentSeekingObject = botTree.getValue(TreeData.SEEK_THIS) as GameObject;
                if (CurrentSeekingObject == otherPlayer.gameObject){
                    CurrentSeekingObject = null; 
                    botTree.setValue(TreeData.SEEK_THIS, CurrentSeekingObject);
                }
                List<GameObject> enemiesNearby = botTree.getValue(TreeData.ENEMIES_NEARBY) as List<GameObject>;
                enemiesNearby.Remove(otherPlayer.gameObject);
                botTree.setValue(TreeData.ENEMIES_NEARBY, enemiesNearby);
            }
        }
    }
}