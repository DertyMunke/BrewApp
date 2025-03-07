﻿using UnityEngine;
using System.Collections;

public class GlassControl : MonoBehaviour 
{
	private Vector3 origPos;
	private Quaternion origRot;

	public static GlassControl glassScript;
	public bool glassDestroy = false;
	public float centerMass = -2;
	public float glassSpeed = 10.0f;

	private void Awake()
	{
		glassScript = this;
	}

	private void Start()
	{
		Invoke ("GetPosition", .5f);

	}

	private void GetPosition()
	{
		origPos = transform.position;
		origRot = transform.rotation;
	}

	private void Update () 
	{		
		if(gameObject.transform.position.y < -2 || gameObject.transform.position.z < -3 || 
		   gameObject.transform.position.z > 11)
		{
//			audio.clip = "LongSlide1";
			KillIt();
			ThrowGlassControl.tgcScript.getPoints = false;
		}

		if (glassDestroy)
		{
//			audio.clip = "LongSlide1";
			KillIt ();
			glassDestroy = false;
		}
	}

	private void KillIt()
	{
		gameObject.SetActive(false);
		transform.rotation = origRot;
		transform.position = origPos;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}

//	private void OnCollisionEnter(Collision other)
//	{
//		if(other.gameObject.tag == "Bar")
//		{
//			audio.Play ();
////			if(!firstSlideSound)
////				audio.clip = "SoftHitSlide";
//			
//		}
//	}

//	private void OnCollisionExit()
//	{
//		if(audio.isPlaying)
//		{
//			audio.Stop();
//		}
//	}
}
