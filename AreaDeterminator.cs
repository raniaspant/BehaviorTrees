using UnityEngine;
using System.Collections;

/* @class AreaDeterminator
 * @brief This is the script that determines if a bot(or the human player) is in its base.
 */

public class AreaDeterminator : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	//In right base
	void OnTriggerEnter(Collider otherPlayer) 
	{
		if((otherPlayer.gameObject.tag == "Bot"))
            otherPlayer.gameObject.GetComponent<CTF.BotScript>().botTree.setValue(CTF.TreeData.IN_ARENA, CTF.BotScript.TeamID.RIGHT);
		if ((otherPlayer.gameObject.tag == "HumanPlayer")) 
			otherPlayer.gameObject.GetComponent<HumanPlayerScript> ().inBase = CTF.BotScript.TeamID.RIGHT;
	}
	
    void OnTriggerExit(Collider otherPlayer)
    {
        if ((otherPlayer.gameObject.tag == "Bot"))
            otherPlayer.gameObject.GetComponent<CTF.BotScript>().botTree.setValue(CTF.TreeData.IN_ARENA, CTF.BotScript.TeamID.LEFT);
		if((otherPlayer.gameObject.tag == "HumanPlayer"))
			otherPlayer.gameObject.GetComponent<HumanPlayerScript>().inBase = CTF.BotScript.TeamID.LEFT;
    }
}