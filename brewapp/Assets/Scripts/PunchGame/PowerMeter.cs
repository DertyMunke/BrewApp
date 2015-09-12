using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerMeter : MonoBehaviour
{
	private Vector2 gauge1Pos;
	private Vector2 gauge2Pos;
	private Vector3 gauge3Pos;
	private bool gaugeDecay = true;
	private float timer1 = 0;
	private float timer2 = 0;
	private float delay1 = .5f;
	private float delay2 = .5f;
	private float decay1 = 1f;
	private float decay2 = 1f;
	private float gauge1Move = 2.9f;
	private float gauge2Move = 2.9f;
	private float gauge1Value = .9f;
	private float gauge2Value = .3f;
//	private int throwPower = 0;

	public static PowerMeter pmeterScript;
	public GameObject button2;
	public GameObject b2Path;
	public GameObject b2Ring;
	public GameObject bagPath;
	public GameObject bagRing;
	public Image powerMeter;
	public Image gauge1;
	public Image gauge2;
	public Image gauge3;
	public Image gaugeEnd;

	private void Awake()
	{
		pmeterScript = this;
	}

	private void Start()
	{
		gauge1Pos = gauge1.transform.position;
		gauge2Pos = gauge2.transform.position;
		gauge3Pos = gauge3.transform.position;
	}

	private void FixedUpdate()
	{
		if(gaugeDecay)
		{
			Gauge1 ();
			Gauge2 ();
		}
	}

	// Gauge 1 behavior and decay
	private void Gauge1 ()
	{
		if(gauge1.transform.position.x > gauge1Pos.x && timer1 < Time.time)
		{
			if(gauge1.transform.position.x < gauge1Pos.x + 2)
			{
				b2Path.SetActive(false);
				b2Ring.SetActive(false);
				button2.SetActive(false);
				bagPath.SetActive(false);
				bagRing.SetActive(false);
			}
			else
			{
				gauge1.transform.position = new Vector3(gauge1.transform.position.x - decay1, gauge1.transform.position.y);
				timer1 = Time.time + delay1;
			}
		}
	}

	// Gauge 2 behavior and decay
	private void Gauge2 ()
	{
		if(gauge2.transform.position.x > gauge2Pos.x && timer2 < Time.time)
		{
			if(gauge2.transform.position.x < gauge2Pos.x + 2)
			{
				bagPath.SetActive(false);
				bagRing.SetActive(false);
			}
			else
			{
				gauge2.transform.position = new Vector3(gauge2.transform.position.x - decay2, gauge2.transform.position.y);
				timer2 = Time.time + delay2;
			}
		}
		else if(gauge2.transform.position.x < gauge2Pos.x)
		{
			b2Path.SetActive(false);
			b2Ring.SetActive(false);
			button2.SetActive(false);
			gauge2.transform.position = new Vector2(gauge2Pos.x, gauge2Pos.y);
		}
	}

	// The first power button which controls gauge1 power increase when tapped
	public void PowerButton1()
	{
		PunchToot.punchTootScript.CheckContinue (3);
		button2.SetActive(false);
		bagPath.SetActive(false);
		bagRing.SetActive(false);
		b2Path.SetActive(true);
		b2Ring.SetActive(true);
		gauge2.transform.position = gauge2Pos;

		if(gauge1.transform.position.x < gauge2Pos.x)
		{
			gauge1.transform.position = new Vector2(gauge1.transform.position.x + gauge1Move, gauge1.transform.position.y);
		}
		else 
		{
			gauge1.transform.position = new Vector2(gauge2Pos.x - 1, gauge1.transform.position.y);
		}
	}

	// The second power button, which controls gauge2 power increase when tapped	
	public void PowerButton2()
	{
		if(!bagPath.activeInHierarchy)
		{
			b2Path.SetActive(false);
			bagPath.SetActive(true);
			bagRing.SetActive(true);
		}
		
		if(gauge2.transform.position.x < gauge3Pos.x)
		{
			gauge2.transform.position = new Vector2(gauge2.transform.position.x + gauge2Move, gauge2.transform.position.y);
		}
		else 
		{
			gauge2.transform.position = new Vector2(gauge3Pos.x - 1, gauge2.transform.position.y);
		}
	}

	// Adds the swipe bonus power to gauge2 or gauge3 
	public void BonusPower2(float bonus)
	{
		if(b2Path.activeInHierarchy)
		{
			gauge2.transform.position = new Vector2(gauge2Pos.x + bonus, gauge2.transform.position.y);
		}
		else if(bagPath.activeInHierarchy)
		{
			gauge3.transform.position = new Vector2(gauge3Pos.x + bonus, gauge3Pos.y);
		}
	}

	// Subtracts a penalty from gauge2 or gauge3 when swiping too long on bonus power
	public void PenaltyPower2(float penalty)
	{
		if(b2Path.activeInHierarchy && gauge2.transform.position.x - penalty > gauge2Pos.x)
		{
			gauge2.transform.position = new Vector2(gauge2.transform.position.x - penalty, gauge2.transform.position.y);
		}
		else if(bagPath.activeInHierarchy && gauge3.transform.position.x - penalty > gauge3Pos.x)
		{
			gauge3.transform.position = new Vector2(gauge3.transform.position.x - penalty, gauge3.transform.position.y);
		}
	}

	// Starts the coroutine that adds up the total power from the gauges
	public void AddBtnPower()
	{
		gaugeDecay = false;
		StartCoroutine ("AddPowerTotal");
	}

	// Adds the power from gauge1 and gauge2 to gauge3, while animating the gauges
	IEnumerator AddPowerTotal()
	{
		yield return new WaitForSeconds (1);

		while(gauge2.transform.position.x > gauge2Pos.x && gauge3.transform.position.x < gaugeEnd.transform.position.x)
		{
			Vector2 pos2 = gauge2.transform.position;
			Vector2 pos3 = gauge3.transform.position;
			gauge2.transform.position = new Vector2(pos2.x - 3, pos2.y);
			gauge3.transform.position = new Vector2(pos3.x + gauge2Value, pos3.y);
			yield return new WaitForSeconds(.01f);
		}
		gauge2.transform.position = gauge2Pos;

		while(gauge1.transform.position.x > gauge1Pos.x && gauge3.transform.position.x < gaugeEnd.transform.position.x)
		{
			Vector2 pos1 = gauge1.transform.position;
			Vector2 pos3 = gauge3.transform.position;
			gauge1.transform.position = new Vector2(pos1.x - 3, pos1.y);
			gauge3.transform.position = new Vector2(pos3.x + gauge1Value, pos3.y);
			yield return new WaitForSeconds(.01f);
		}
		gauge1.transform.position = gauge1Pos;

		PunchManager.pManagerScript.CompletePunch (gauge3.transform.position.x - gauge3Pos.x,
		                                           gaugeEnd.transform.position.x - gauge3Pos.x);
	}

	// Activates the decaying power over time
	public void GaugeDecayActive(bool active)
	{
		gauge1.transform.position = gauge1Pos;
		gauge2.transform.position = gauge2Pos;
		gauge3.transform.position = gauge3Pos;
		gaugeDecay = active;
	}

	// Sets the power meter difficulty
	public void PowerDifficulty(int diff)
	{
		delay1 = delay1 - ((diff - 1) * .1f);
		gauge2Value = gauge2Value - ((diff - 1) * .05f);
	}

	// Returns the value of gauge2 additor
	public float GetGauge2Val()
	{
		return gauge2Value;
	}
}
