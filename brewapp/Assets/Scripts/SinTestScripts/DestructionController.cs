using UnityEngine;
using System.Collections;

public class DestructionController : MonoBehaviour 
{
	private Vector3 mugVelocity;

	public static DestructionController destroyScript;
	public GameObject force;
	public GameObject remains;	
	public bool goBoom = false;

	void Awake()
	{
		destroyScript = this;
	}

	void Update () 
	{
		if(rigidbody.velocity.x <= mugVelocity.x)
		{
			mugVelocity = rigidbody.velocity;
		}
		BeerExplode ();
	}

	void BeerExplode()
	{
		if (goBoom)
		{
			Instantiate (remains, transform.position, transform.rotation);
			goBoom = false;
		}
	}
}
