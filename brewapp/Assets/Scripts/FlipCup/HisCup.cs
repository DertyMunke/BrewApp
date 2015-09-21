using UnityEngine;
using System.Collections;

public class HisCup : MonoBehaviour 
{
	private Vector3 startPos;
	private Quaternion startRot;

	public GameObject myForcePoint;
	public GameObject winTrigger;

	private void Start()
	{
		startPos = transform.position;	
		startRot = transform.rotation;
	}

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
			SetFlipCupPos();
			FlipCupManager.managerScript.GetFoxCurrCup(gameObject);
			gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// Sets the flip cup to its starting position
	/// </summary>
	public void SetFlipCupPos()
	{
		rigidbody.velocity = Vector3.zero;
		transform.position = new Vector3(startPos.x + .78f, startPos.y, startPos.z);
		transform.rotation = startRot;
	}

	/// <summary>
	///  Checks if you landed the cup correctly
	/// </summary>
	/// <returns><c>true</c>, if you successfully flipped the cup, <c>false</c> otherwise.</returns>
	public bool CheckSuccess()
	{
		if(winTrigger.GetComponent<WinTrigger>().collideName == "TableCollider" )
		{
			return true;
		}
		
		return false;
	}

	/// <summary>
	/// Applies the appropriate amount of force and direction to the cup
	/// </summary>
	/// <param name="dist">Dist.</param>
	public void ApplyForce(Vector2 dist)
	{
		// Apply force to cup then start game timer
		rigidbody.AddForceAtPosition(new Vector3(dist.x, dist.y, 0), 
		                             myForcePoint.transform.position);
	}
}
