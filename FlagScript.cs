using UnityEngine;
using System.Collections;

/* @class FlagScript
 * @brief This is the flag's class, containing its behavior depending on captivity status.
 */

public class FlagScript : MonoBehaviour {
	
	public bool isCaptured = false;
	public GameObject parent;
	public Vector3 flagPosition;
	
	// Use this for initialization
	void Start () {
		flagPosition = transform.position;
		parent = null;
	}

	//if the flag is captured, parent it to the capturer
	public void Captured(GameObject iParentItTo)
	{
		if(parent == null && isCaptured == false){
			parent = iParentItTo;
			isCaptured = true;
		}
	}

	public bool IsCaptured()
	{
		return isCaptured;
	}

	//if the flag is released, it returns to its prime position
	public void Release(GameObject currentParent)
	{
		if(isCaptured && currentParent == parent){
			isCaptured = false;
			parent = null;
			returnToBase();
		}
	}

	void returnToBase()
	{
		transform.position = flagPosition;
	}

	// Update is called once per frame
	void Update () {
		if(parent)
			transform.position = parent.transform.position;
	}
}
