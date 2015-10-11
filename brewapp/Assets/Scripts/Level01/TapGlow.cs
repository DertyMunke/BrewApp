using UnityEngine;
using System.Collections;

public class TapGlow : MonoBehaviour 
{
	private BarController barControlScript;
	private bool resetRim = false;
	private float rimPower = 2f;
	private float pulseTimer = 0;
	
	public float pulseDelay = 2;
	public float rimPowerInc = .1f;
	public float rimMax = 5;
	public float rimMin = 2;

	void Start()
	{
		barControlScript = GameObject.Find ("Controller").GetComponent<BarController> ();
	}

	void Update()
	{
		GlowPulse ();
	}

	void GlowPulse()
	{
		if(barControlScript.stage == 2 && !barControlScript.glass)
		{
			if(pulseTimer - Time.time > pulseDelay * .5f)
			{
				if(rimPower < rimMax)
				{
					rimPower += rimPowerInc;
				}
			}
			else if(pulseTimer - Time.time > 0)
			{
				if(rimPower > rimMin)
				{
					rimPower -= rimPowerInc;
				}
			}
			else
			{
				pulseTimer = Time.time + pulseDelay;
				rimPower = rimMin;
			}
		
			renderer.material.SetFloat ("_RimPower", rimPower);
			resetRim = true;
		}
		else 
		{
			if(resetRim)
			{
				renderer.material.SetFloat("_RimPower", 4.8f);
				resetRim = false;
			}
		}
	}
}
