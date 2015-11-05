using UnityEngine;
using System.Collections;

public class PauseOnStart : MonoBehaviour 
{
	private bool oneTime = true;

	public static PauseOnStart POSscript;
	// Use this for initialization

	private void Start()
	{
		POSscript = this;
	}

	private void Update ()
	{
		if(oneTime)
		{
			Time.timeScale = 0;
			oneTime = false;
		}

		if(Time.timeScale != 0 && (!gameObject.GetComponent<Animator> ().GetBool("expand2")))// || myAnim.animation.name != "ExpandRepSliders2" ||
		   //myAnim.animation.name != "ExpandRepSliders3"))
		{
			oneTime = true;
			POSscript.enabled = false;
		}

	}

	public void ResetOneTime()
	{
		oneTime = true;
	}
}
