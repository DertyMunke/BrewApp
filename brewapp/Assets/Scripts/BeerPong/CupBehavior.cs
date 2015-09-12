using UnityEngine;
using System.Collections;

public class CupBehavior : MonoBehaviour 
{
	private float delay = 2;
	private float visibleTimer;
	private bool visibleTrigger = false;
	private PPManager ppManagerScript;
	public GameObject ppBall;


	void Start()
	{
		ppManagerScript = Camera.main.GetComponent<PPManager> ();
	}
	void Update()
	{
		if(visibleTrigger && Time.time > visibleTimer)
		{
			visibleTrigger = false;
			transform.parent.gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(visibleTimer < Time.time && other.gameObject.name == "PingPongBall")
		{
			if(transform.parent.tag == "OtherCups")
			{
				ppManagerScript.myScore ++;
			}
			if(transform.parent.tag == "MyCups")
			{
				ppManagerScript.hisScore ++;
			}

			visibleTimer = Time.time + delay;
			other.renderer.enabled = false;
			visibleTrigger = true;
			PingPongBall.ppBallScript.camFollow = false;
		}
	}
}
