using UnityEngine;
using System.Collections;

// Ghost balls show the potential trajectory of a perfectly thrown ball 
public class GhostBall : MonoBehaviour 
{
	private Vector3 lastBounce;
	private float colliderDelay = .5f;
	private float colliderTime = 0;
	private float destroyDistZ = 0;
	private bool inactive = false;
	private bool oneDamp = true;
	
	private void Start()
	{
		destroyDistZ = 12 - PPManager.ppManager.GetLvlDifficulty () * 2;
		colliderTime = Time.time + colliderDelay;
	}

	private void FixedUpdate()
	{
		// Apply the trail, allow the blinking of the ball, and apply force to each ghost ball
		if(gameObject.activeInHierarchy)
		{
			if(inactive)
			{
				gameObject.GetComponent<TrailRenderer> ().enabled = true;
				colliderTime = Time.time + colliderDelay;
				inactive = false;
			}
			if(gameObject.transform.position == PingPongBall.ppBallScript.GetGhostStartPos())
				gameObject.GetComponent<Collider>().enabled = false;
			BlinkyGhost ();
		}
	}

	// When the ghost ball collides with something it reduces the time that it can be seen
	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "OtherCups")
		{
			Invoke ("GhostDestructor", .4f);
		
			//For ping pong ball bounce effect
			if(oneDamp)
			{
				oneDamp = false;
				Vector3	lastBounce = transform.GetComponent<Rigidbody>().velocity;
				transform.GetComponent<Rigidbody>().velocity = new Vector3(lastBounce.x * .5f, lastBounce.y * .5f, lastBounce.z * .5f);
			}
		}


	}

	// Helper function for OnCollisionEnter(): Invoke() method -> resets the ball position so it can be re-used
	private void GhostDestructor()
	{
		gameObject.GetComponent<TrailRenderer> ().enabled = false;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		gameObject.SetActive (false);
		inactive = true;
		oneDamp = true;
		GetComponent<Renderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
	}

	// Gives the ball it's blinking effects
	private void BlinkyGhost()
	{
		if(!PingPongBall.ppBallScript.targeting || transform.position.z >= destroyDistZ)
		{
			GhostDestructor ();
		}

		if(Time.time > colliderTime)
		{
			if(GetComponent<Renderer>().enabled == false)
			{
				GetComponent<Renderer>().enabled = true;
			}
			else
			{
				GetComponent<Renderer>().enabled = false;
			}
			
			colliderTime = Time.time + colliderDelay;
			if(!GetComponent<Collider>().enabled)
			{
				GetComponent<Collider>().enabled = true;
			}
		}
	}
}
