using UnityEngine;
using System.Collections;

public class CupBehavior : MonoBehaviour 
{
    public GameObject ppBall;
    public GameObject splash;
    public GameObject poof;

    private float delay = 2;
	private float visibleTimer;
	private bool visibleTrigger = false;
	private PPManager ppManagerScript;

	void Start()
	{
		ppManagerScript = Camera.main.GetComponent<PPManager> ();
	}
	void Update()
	{
		if(visibleTrigger && Time.time > visibleTimer)
		{
            Instantiate(poof, transform.position, Quaternion.identity);
			visibleTrigger = false;
			transform.parent.gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(visibleTimer < Time.time && other.gameObject.name == "PingPongBall")
		{
            Instantiate(splash, transform.position, Quaternion.identity);

			if(transform.parent.tag == "OtherCups")
			{
				ppManagerScript.myScore ++;
                PPManager.ppManager.NiceShot();
            }
			if(transform.parent.tag == "MyCups")
			{
				ppManagerScript.hisScore ++;
			}

			visibleTimer = Time.time + delay;
			other.GetComponent<Renderer>().enabled = false;
			visibleTrigger = true;
			PingPongBall.ppBallScript.camFollow = false;
		}
	}
}
