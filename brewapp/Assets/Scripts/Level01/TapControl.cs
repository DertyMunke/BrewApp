using UnityEngine;
using System.Collections;

public class TapControl : Touch3D 
{
	private float swipe;

	public GameObject[] otherGlass = null;
	public GameObject glass;
	public GameObject glassSpawn;
	public GameObject glassClone;
	public float bump = 1.0f;
	public int tapSpeed = 40;

	void Start()
	{
		glassClone = (GameObject)Instantiate(glass, glassSpawn.transform.position, glassSpawn.transform.rotation);
		glassClone.name = transform.name;
		glassClone.SetActive (false);
	}

	void Update()
	{
		TouchController ();
	}

	public override void OnTouchBegan()
	{
		swipe = Input.GetTouch(0).position.y;

		otherGlass = null;	
		otherGlass = GameObject.FindGameObjectsWithTag("Glass");
		foreach(GameObject other in otherGlass)	
		{
			other.GetComponent<DestructionController>().goBoom = true;
			other.GetComponent<GlassControl>().glassDestroy = true;
		}
	}

	public override void OnTouchEnded()
	{
		swipe -= Input.GetTouch(0).position.y;
		
		if(swipe > 20)
		{
			if(TootText.tootScript.tootStage == 2)
				TootText.tootScript.tootStage = 3;
			Quaternion rotate = Quaternion.AngleAxis(40, Vector3.left);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, tapSpeed);
			glassClone.transform.position = glassSpawn.transform.position;
			glassClone.transform.rotation = glassSpawn.transform.rotation;
			glassClone.GetComponent<Animator>().enabled = true;
			glassClone.SetActive(true);
			glassSpawn.transform.FindChild("BeerPour").gameObject.SetActive(true);
			GetComponent<AudioSource>().Play ();
		}

		Invoke("TapReturn", .9f);
	}

	void TapReturn()
	{
		Quaternion rotate = Quaternion.AngleAxis(0, Vector3.right);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, tapSpeed);

		glassClone.GetComponent<Animator> ().enabled = false;
		glassSpawn.transform.FindChild("BeerPour").gameObject.SetActive(false);
		Invoke ("EndTapReturn", .1f);
	}

	private void EndTapReturn()
	{
		GetComponent<AudioSource>().Stop ();
		BarController.barControlScript.MoveToThrow (glassClone);
	}
}