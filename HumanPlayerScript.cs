using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* @class HumanPlayerScript
 * @brief This is the script that determines the human player's interaction within the game.
 */

public class HumanPlayerScript : MonoBehaviour {

	public CTF.BotScript.TeamID myTeamID;
	public CTF.BotScript.TeamID inBase;
    public bool isTagged = false;
    public bool hasFlag = false;
	public Vector3 respawnPosition;
    public float velocity;
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> allies = new List<GameObject>();
    GameObject enemyFlag;

	// Use this for initialization
	void Start () 
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 10.0f, 0.0f));
        if (GetComponent<Rigidbody>().velocity.magnitude > 0){
            float angle = Mathf.Atan2(GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(angle, -90f, 90f);
        }
        GameObject[] allBots = GameObject.FindGameObjectsWithTag("Bot");
        for (int i = 0; i < allBots.Length; i++){
            if (allBots[i].GetComponent<CTF.BotScript>().myTeamID == myTeamID)
                allies.Add(allBots[i]);
            else
                enemies.Add(allBots[i]);
        }
        GameObject[] Flags = GameObject.FindGameObjectsWithTag("Flag");
        for (int i = 0; i < Flags.Length; i++){
            if (Flags[i].name == "LeftFlag")
                enemyFlag = Flags[i];
        }
        respawnPosition = GameObject.Find("RightRespawnNode").transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isTagged){
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                GetComponent<Rigidbody>().AddForce(new Vector3(-10.0f, 0.0f, 0.0f));
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                GetComponent<Rigidbody>().AddForce(new Vector3(10.0f, 0.0f, 0.0f));
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 10.0f, 0.0f));
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -10.0f, 0.0f));
            if (GetComponent<Rigidbody>().velocity.magnitude > 0){
                float angle = Mathf.Atan2(GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(angle, -90f, 90f);
            }
            GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, velocity);
            if (inBase == myTeamID){
				for (int i = 0; i < enemies.Count; i++){
					if ((!(bool)enemies[i].GetComponent<CTF.BotScript>().botTree.getValue(CTF.TreeData.IS_TAGGED)) 
						&& Collided(enemies[i].transform.position))
						enemies[i].GetComponent<CTF.BotScript>().botTree.setValue(CTF.TreeData.IS_TAGGED, true);
				}
			}
			if (inBase != myTeamID){
				for (int i = 0; i < allies.Count; i++){
					if ((bool)allies[i].GetComponent<CTF.BotScript>().botTree.getValue(CTF.TreeData.IS_TAGGED) 
						&& Collided(allies[i].transform.position))
						allies[i].GetComponent<CTF.BotScript>().botTree.setValue(CTF.TreeData.IS_TAGGED, false);
				}
				if (enemyFlag){
					if(!enemyFlag.GetComponent<FlagScript>().IsCaptured() && Collided(enemyFlag.transform.position)){
						enemyFlag.GetComponent<FlagScript>().Captured(this.gameObject);
						hasFlag = true;
					}
				}
			}
            if (hasFlag)
                if (inBase == myTeamID)
                    Application.LoadLevel(0);	//player has the flag, restart the scene
        }
        else{
			gameObject.transform.position = new Vector3(respawnPosition.x, respawnPosition.y, gameObject.transform.position.z);
			isTagged=false;

        }
	}

    bool Collided(Vector3 Position)
    {
        float distance = (new Vector3(transform.position.x - Position.x, transform.position.y - Position.y, 0.0f)).magnitude;
        if (distance < 0.5f)
            return true;
        return false;
    }

    public void catchHuman()
    {
        isTagged = true;
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y, transform.position.z);
    }

}
