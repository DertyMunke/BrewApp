using UnityEngine;
using System.Collections;

// An object on the cup that detects a successful flip 
public class WinTrigger : MonoBehaviour 
{
	public static WinTrigger triggerScript;
	public string collideName = " ";

	void OnAwake()
	{
		triggerScript = this;
	}

	void OnTriggerEnter(Collider other)
	{
		collideName = other.gameObject.name;
	}

	void OnTriggerExit(Collider other)
	{
		collideName = " ";
	}

}
