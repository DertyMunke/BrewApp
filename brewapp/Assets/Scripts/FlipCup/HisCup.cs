using UnityEngine;
using System.Collections;

public class HisCup : MonoBehaviour 
{
	private void OnCollisionEnter(Collision other)
	{
//		Debug.Log ("Collision  " +other.gameObject.name);
		if (other.gameObject.name == "FoxHand")
			gameObject.transform.parent = other.gameObject.transform;
	}

	private void OnTriggerEnter(Collider other)
	{
//		Debug.Log ("Trigger  " + other.gameObject.name);
		if (other.gameObject.name == "FoxHand")
		{
			FlipCupManager.managerScript.GetFoxCurrCup(gameObject);
			transform.position = new Vector3(transform.position.x + .78f, transform.position.y, transform.position.z);
			gameObject.SetActive (false);
		}
	}

//	// Applies the appropriate amount of force and direction to the cup
//	public void ApplyForce(Vector2 dist)
//	{
//		float ySwipe = dist.y; // Delta dist y dir
//		
//		if(ySwipe < minDist)
//		{
//			ySwipe = minDist;  
//		}
//		else if(ySwipe > maxDist)
//		{
//			ySwipe = maxDist;
//		}
//		
//		// Apply force to cup then start game timer
//		rigidbody.AddForceAtPosition(new Vector3(ySwipe * .033f, 15 - (swipeTime * 100 - 15) + Mathf.CeilToInt(ySwipe/swipeTime * .001f) * 2, 0), 
//		                             forcePoint.transform.position);
//		
//		timer = Time.time + timeDelta;
//	}
}
