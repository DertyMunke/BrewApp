using UnityEngine;
using System.Collections;

public class HisCup : MonoBehaviour 
{
	private void OnCollisionEnter(Collision other)
	{
		Debug.Log ("Collision  " +other.gameObject.name);
		if (other.gameObject.name == "FoxHand")
			gameObject.transform.parent = other.gameObject.transform;
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Trigger  " + other.gameObject.name);
		if (other.gameObject.name == "FoxHand")
			gameObject.transform.parent = other.gameObject.transform;
	}
}
