using UnityEngine;
using System.Collections;

public class BeerPwrMeter : MonoBehaviour
{
	private float timer = 0;
	private float powerDir;
	private Animator anim;

	public float delay = .5f;

	private void Start ()
	{
		anim = GetComponent<Animator> ();
		timer = Time.time + delay;
		anim.SetBool ("PowerEnabled", true);
	}
	
	private void Update () 
	{
		if(timer - Time.time > delay * .5f)
		{
			anim.speed = 1;
		}
		else if(timer - Time.time > 0)
		{
			anim.speed = 0;
		}
		else 
		{
			timer = Time.time + delay;
		}

	}
}
