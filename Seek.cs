using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class Seek
 * @brief In this class, bot's velocity changes depending on the target that is
 *  currently being sought. Success is achieved if the target is reached. 
 */
 
namespace CTF
{
    class Seek : Leaf
    {
        float steering;
        float velocity;
        Transform myTransform;
        GameObject seekNow;

        public override void Initialize(BehaviorTree tree)
        {
            steering = (float)tree.getValue(TreeData.STEER);
            velocity = (float)tree.getValue(TreeData.VELOCITY);
        }

        public override Outcome Process(BehaviorTree tree)
        {
            myTransform = (Transform)tree.getValue(TreeData.MY_TRANSFORM);
            seekNow = tree.getValue(TreeData.SEEK_THIS) as GameObject;
            float iVelFactor = velocity;
            if (seekNow){
                if (seekNow.gameObject.CompareTag("Flag"))		//going for flag = accelerate
                    iVelFactor*= 2;
                else if((bool)tree.getValue(TreeData.HAS_FLAG))	//possesses flag = get half of normal speed
                    iVelFactor /= 2;
                if (routineSeek(seekNow, iVelFactor))			//reached target
                    return Outcome.SUCCESS;
                return Outcome.PROCESSING;						//still pursuing
            }
            else
                return Outcome.FAIL;
        }
	
		//routine to check if bot is close to the target it is seeking
        bool routineSeek(GameObject toSeek, float velocity)
        {
			float XYDistance = (new Vector3(myTransform.position.x - toSeek.transform.position.x, myTransform.position.y - toSeek.transform.position.y, 0.0f)).magnitude;
            if (XYDistance < 0.15f)
                return true;
			Vector3 Direction = (toSeek.transform.position - myTransform.position).normalized;
            Vector3 DesiredVelocity = Direction * velocity;
            Vector3 SteeringForce = (DesiredVelocity - myTransform.GetComponent<Rigidbody>().velocity).normalized * steering;
            myTransform.GetComponent<Rigidbody>().AddForce(SteeringForce);
            myTransform.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(myTransform.GetComponent<Rigidbody>().velocity, velocity);
            if (myTransform.GetComponent<Rigidbody>().velocity.magnitude > 0){
                float angle = Mathf.Atan2(myTransform.GetComponent<Rigidbody>().velocity.y, myTransform.GetComponent<Rigidbody>().velocity.x) * Mathf.Rad2Deg;
                myTransform.transform.rotation = Quaternion.Euler(angle, -90f, 90f);
            }
            return false;
        }
    }
}
