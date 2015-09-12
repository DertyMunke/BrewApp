using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour 
{
	public float destroyTimer = 4; 

	void Start () 
	{
		Invoke ("DestroyTimer", destroyTimer);
	}

	void DestroyTimer ()
	{
		Destroy (gameObject);
	}
}