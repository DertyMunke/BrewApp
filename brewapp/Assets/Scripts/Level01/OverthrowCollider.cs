using UnityEngine;
using System.Collections;

public class OverthrowCollider : MonoBehaviour 
{
	private bool isHit = false;

	public static OverthrowCollider overThrowScript;

	private void Awake()
	{
		overThrowScript = this;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Glass")
		{
			other.gameObject.GetComponent<GlassControl>().glassDestroy = true;
			other.gameObject.GetComponent<DestructionController>().goBoom = true;
			ThrowGlassControl.tgcScript.getPoints = false;
//			BarController.barControlScript.numOrders--;
//			Debug.Log("OTC " + BarController.barControlScript.numOrders);
			isHit = true;
		}
	}

	// Checks overthrow to see if it should count the throw
	public bool GetOverthrow()
	{
		if(isHit)
		{
			isHit = false;
			return true;
		}
		return isHit;
	}
}
