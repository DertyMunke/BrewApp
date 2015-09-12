using UnityEngine;
using System.Collections;

public class Dude : MonoBehaviour 
{
	private GameObject[] myCups;
	private PingPongBall ppballScript;
	private bool throwPowerSwitch = true;
	private float missRotation = 0;
	private float missRotRange = 10;
	private float missProbability = 25;
	private float missValue = 0;
	private int missPowRange = 6;
	private int missPower = 0;
	private int throwPower = 0;
	private int cupIndex;
	
	public static Dude dudeScript;
	public GameObject ppBall;
	public Transform cup1;
	public Transform cup2;
	public Transform cup3;
	public Transform cup4;
	public Transform cup5;
	public Transform cup6;
	public AnimationClip celebration1;
	public AnimationClip drinkBeer;
	public bool dudetest = false;

	private void Awake()
	{
		dudeScript = this;
	}

	private void Start ()
	{
		ppballScript = PingPongBall.ppBallScript;
	}

	private void Update () 
	{
		// Testing dudes ReRack
//		if(dudetest)
//		{
//			dudetest = false;
//			PPManager.ppManager.ReRackDude();
//		}

		DudesTarget ();
	}

	// The AI that determins what cup to throw at, how likey it will make it and adds variances to the flight of the ball
	private void DudesTarget()
	{
		if(!ppballScript.myTurn && throwPowerSwitch)
		{
			if(GameManager.manager.PongTootActive())
				PongToot.tootScript.CallTootStage (11);
			PPManager.ppManager.ReRackDude();  // Checks if dude will re-rack
			ppBall.transform.position = new Vector3(-ppballScript.origPos.x, ppballScript.origPos.y, -ppballScript.origPos.z);

			myCups = GameObject.FindGameObjectsWithTag("MyCups");
			if(myCups.Length != 0)
			{
				cupIndex = Random.Range(0, myCups.Length - 1);
				missValue = Random.Range(1, 100);

				// Testing
//				missValue = 0;
//				Debug.Log("miss prob " + missValue + " " + missProbability);

				ppBall.transform.LookAt(myCups[cupIndex].transform);
				AimIt(myCups[cupIndex].name);

				if(missValue >= missProbability)
				{
					missPower = Random.Range(0, missPowRange);
					missRotation = Random.Range(-missRotRange, missRotRange);
					MissProbability(missPower, missRotation);
				}

				ppBall.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				Invoke("ThrowIt", 2);
				throwPowerSwitch = false;
			}
		}
	}

	// Adds a variance to the distance and direction of the ball flight
	private void MissProbability(int power, float rotation)
	{
		ppBall.transform.Rotate(-rotation, 0, 0);
		throwPower += power;
	}

	// Applies a base power and direction to successfully make it into eace cup
	private void AimIt(string index)
	{
		if(index == "MyCup6")
		{
			ppBall.transform.Rotate(-60, 0, 0);
			throwPower = 66;
		}
		else if(index == "MyCup5")
		{
			ppBall.transform.Rotate(-60, 0, 0);
			throwPower = 65;
		}
		if(index == "MyCup4")
		{
			ppBall.transform.Rotate(-60, 0, 0);
			throwPower = 66;
		}
		if(index == "MyCup3")
		{
			ppBall.transform.Rotate(-60, 0, 0);
			throwPower = 64;
		}
		if(index == "MyCup2")
		{
			ppBall.transform.Rotate(-60, 0, 0);
			throwPower = 64;
		}
		if(index == "MyCup1")
		{
			ppBall.transform.Rotate(-60, 0, 0);
			throwPower = 62;
		}
	}

	// Access the ping pong ball script and tell it to throw the ball
	private void ThrowIt()
	{
		ppballScript.ReleaseBall (throwPower);
		Invoke ("MyTurn", 5);

	}

	// Helper function for ThrowIt(): Used for Invoke() method
	private void MyTurn()
	{
		ppballScript.myTurn = true;
		throwPowerSwitch = true;
		ppballScript.ResetTurn ();
	}

	// Allows another script to set the difficulty of the AI
	public void SetDudeDiff(int diff)
	{
		missProbability = diff * 20;
	}
}
