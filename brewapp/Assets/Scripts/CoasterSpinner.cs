using UnityEngine;
using System.Collections;

public class CoasterSpinner : MonoBehaviour 
{	
	//private Transform targetRotation;
	private int count = 0;

	void Update () 
	{
		if(count < 370)
		{
			Invoke("Grow", .1f);	
			Rotate ();
		}
	}
	
	void Grow()
	{
		count++;
		transform.localScale += new Vector3(.035f, .035f, 0);
	}

	void Rotate()
	{
		//transform.Rotate(0, 120*Time.deltaTime, 0);
		Quaternion rotate = Quaternion.AngleAxis(count, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 5*Time.deltaTime);
	}

}
