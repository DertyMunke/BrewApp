using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PingPongBall : MonoBehaviour
{
	private GameObject[] ghosts;
	private Vector3 cameraLeft;
	private Vector3 cameraRight;
	private Vector3 hisCup1Pos;
	private Quaternion rotateBall;
	private Vector3 cameraStartRot;
	private bool oneDamp = true;
	private bool aimLeft = false;
	private bool aimRight = false;
	private bool aimUp = false;
	private bool aimDown = false;
	private bool increasing = true;
	private bool tootEnabled = false;
	private bool oneSplashSound = true;
	private float pwrMtrTimer = 0;
	private float pwrMtrDelay = .015f;
	private float delay = .5f;
	private float nextGhost;
	public float throwPower = 0;
	private int numGhosts = 10;
	private int ghostIndex = 0;

	public static PingPongBall ppBallScript;
	public GameObject dPad;
	public GameObject ghostBall;
	public GameObject tootCvs;
	public GameObject pwrMtrPnl;
	public AudioClip[] bounceSounds;
	public Transform pointer;
	public Transform hisCup1;
	public Quaternion dudesTarget;   // Opponents targetting system
	public Quaternion camRot;
	public Quaternion origRot;
	public Quaternion recentRotation;
	public Vector3 origPos;
	public Vector3 camPos;
	public Image pwrImg;
	public Sprite[] pwrIms = new Sprite[31];
	public bool camFollow = false;
	public bool isTouched = false;
	public bool myTurn = true;
	public bool resetTurn = false;
	public bool endTurn = false;
	public bool throwBall = false;
	public bool targeting = true;
	public bool bTouched = false;
	public bool powerTrigger = false;
	public bool showUI = true;
	public float incThrowPower = 1.5f;
	public float rotSpeed = 0;
	public float destroyDist = 35;
	public int touch2Watch = 100;
	
	private void Awake()
	{
		ppBallScript = this;
	}

	private void Start()
	{
		Init ();
		cameraStartRot = Camera.main.transform.localEulerAngles;
		recentRotation = transform.rotation;
	}

	private void FixedUpdate () 
	{
		if(transform.rotation != recentRotation && !(aimUp || aimDown || aimLeft || aimRight))
		{
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}

//		if(Application.loadedLevelName != "PongPractice")
//		{
		if(endTurn)
		{
			endTurn = false;
			EndMyTurn();
		}
				

//		else if(endTurn)
//		{
//			endTurn = false;
//			PPManager.ppManager.SetRandomCupActive(); 
//			ResetTurn();
//		}

		if(Time.time > nextGhost && targeting && PPManager.ppManager.myScore != 6)
		{
			nextGhost = Time.time + delay;
			GhostBall ();
		}

		GetPower ();
		AimBall ();
	}

	// Initialized the ping pong ball values and a set of ghost balls for trajectory trail
	private void Init()
	{
		tootEnabled = GameManager.manager.PongTootActive (); // True if pong tutorial is active

		// Create a cache of ghost balls to re-use instead of instantiating and destroying them
		ghosts = new GameObject[numGhosts];
		for (int i = 0; i < numGhosts; i++)
		{
			ghosts[i] = (Instantiate (ghostBall, transform.position, transform.rotation) as GameObject);
			ghosts[i].SetActive(false);
		}
		
		// Set up the game for a practice round
		if(Application.loadedLevelName == "PongPractice")
		{
			PPManager.ppManager.SetRandomCupActive(); 
		}
		
		// Initialize values
		incThrowPower = 1.5f;
		hisCup1Pos = hisCup1.position;
		touch2Watch = 100;
		nextGhost = Time.time + delay;
		origPos = transform.position;
		origRot = transform.rotation;
		camPos = Camera.main.transform.position;
		camRot = Camera.main.transform.rotation;
		cameraLeft = new Vector3 (-8, camPos.y, camPos.z);
		cameraRight = new Vector3 (8, camPos.y, camPos.z);
	}

	// Determines how much to move the ball flight direction on d-pad 
	private void AimBall()
	{
		if(aimLeft)
		{
			transform.Rotate (Vector3.down, Time.deltaTime * rotSpeed);
		}
		else if(aimRight)
		{
			transform.Rotate (Vector3.up, Time.deltaTime * rotSpeed);
		}
		else if(aimUp)
		{
			transform.Rotate (Vector3.left, Time.deltaTime * rotSpeed);
		}
		else if(aimDown)
		{
			transform.Rotate (Vector3.right, Time.deltaTime * rotSpeed);
		}
	}

	// Resets all of the objects in the scene to allow for your turn to begin
	public void ResetTurn()
	{
		dPad.SetActive (true);
		showUI = true;
		Camera.main.transform.parent = null;
		myTurn = true;
		camFollow = false;
		targeting = true;
		throwPower = 0;
		powerTrigger = false;
		Camera.main.transform.position = camPos;
		Camera.main.transform.rotation = camRot;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		transform.position = origPos;
		transform.rotation = origRot;
		GetComponent<Renderer>().enabled = true;
		resetTurn = false;
		pwrImg.sprite = pwrIms [0];
	
		pwrMtrPnl.SetActive (true);
		pwrMtrPnl.GetComponentInChildren<Button> ().interactable = true;
		PPManager.ppManager.GetWinner ();
	}

	// Ends your turn and allows the AI to take over
	private void EndMyTurn()
	{
		myTurn = false;
		GetComponent<Renderer>().enabled = false;
		camFollow = false;

		Camera.main.transform.localEulerAngles = Vector3.Lerp (Camera.main.transform.localEulerAngles, cameraStartRot, 1);
		if(!PPManager.ppManager.GetDblNothin())
		{
			PPManager.ppManager.GetWinner ();
		}
	}

	public void PwrMtrDown()
	{
		if(pwrMtrPnl.GetComponentInChildren<Button>().interactable)
		{
			if(tootEnabled && tootCvs.activeInHierarchy)
				PongToot.tootScript.CallTootStage(10);
			dPad.SetActive(false);
			showUI = false;
			powerTrigger = true;
			pwrMtrTimer = Time.time + pwrMtrDelay;
		}
	}

	public void PwrMtrUp()
	{
		powerTrigger = false;
		ReleaseBall (40 + throwPower);
		Invoke ("EndTurn", 5);
	}

	// Helper function for PwrMtrUp() Invoke -> ends my turn after 5 seconds
	public void EndTurn()
	{
		endTurn = true;
		pwrMtrPnl.SetActive (false);
	}

	// Helper: Pushig a d-pad button -> stops targeting while button is pushed then starts again 
	private void TargetingEnabled()
	{
		targeting = true;
	}

	// Initializes a ghost ball -> See ghost ball script for more ghost ball behavior
	private void GhostBall()
	{
		ghosts [ghostIndex].transform.position = transform.position;
		ghosts [ghostIndex].transform.rotation = transform.rotation;
		ghosts [ghostIndex].SetActive (true);
		ghosts [ghostIndex].GetComponent<Rigidbody>().AddRelativeForce (0, 0, 64);
		if(ghostIndex < numGhosts -1)
			ghostIndex ++;
		else
			ghostIndex = 0;

	}

	// Applies force to the ball
	public void ReleaseBall(float force)
	{
		oneSplashSound = true;
		oneDamp = true;
		targeting = false;
		transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		transform.GetComponent<Rigidbody>().AddRelativeForce (0, 0, force);
		throwPower = 0;
		throwBall = false;
		Invoke ("watchBall", .1f);
	}

	// Helper: ReleaseBall() -> allows the camera to look at the ball
	private void watchBall()
	{
		camFollow = true;
	}

	// Increses or decreases the power in the power meter
	private void GetPower()
	{
		if(powerTrigger && pwrMtrTimer <= Time.time)
		{
			if(throwPower <= 0 || increasing)
			{
				throwPower += .5f;
				increasing = true;
			}
			if(throwPower >= 31 || !increasing)
			{
				throwPower -= .5f;
				increasing = false;
			}
			pwrImg.sprite = pwrIms[Mathf.FloorToInt(throwPower)];
			pwrMtrTimer = Time.time + pwrMtrDelay;
		}
	}

	// Determines the behavior of the ball on collision
	private void OnCollisionEnter(Collision other)
	{
		GetComponent<AudioSource>().volume = 1;
		//For ping pong ball bounce effect
		if(gameObject.GetComponent<MeshRenderer>().enabled)
		{
			if((other.gameObject.tag == "MyCups" || other.gameObject.tag == "OtherCups"))
			{
				if(oneDamp)
				{
					oneDamp = false;
					Vector3	lastBounce = transform.GetComponent<Rigidbody>().velocity;
					transform.GetComponent<Rigidbody>().velocity = new Vector3(lastBounce.x * .5f, lastBounce.y * .5f, lastBounce.z * .5f);

					GetComponent<AudioSource>().clip = bounceSounds[1];
					GetComponent<AudioSource>().Play();

				}
			}
			else if (other.gameObject.tag == "Table")
			{
				GetComponent<AudioSource>().clip = bounceSounds[0];
				gameObject.GetComponent<AudioSource>().Play ();
			}

			else
			{
				GetComponent<AudioSource>().volume = .3f;
				GetComponent<AudioSource>().clip = bounceSounds[0];
				gameObject.GetComponent<AudioSource>().Play ();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(oneSplashSound && other.tag == "InCup")
		{
			if(!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().clip = bounceSounds[1];
				GetComponent<AudioSource>().Play();
				oneSplashSound = false;
			}
		}
	}

	// Right d-pad button
	public void R_DPadBtn(bool down)
	{
		if(down)
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(6);
			aimRight = true;
			targeting = false;
		}
		else
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(7);
			aimRight = false;
			Invoke ("TargetingEnabled", .1f);
		}
	}

	// Left d-pad button
	public void L_DPadBtn(bool down)
	{
		if(down)
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(6);
			aimLeft = true;
			targeting = false;
		}
		else
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(7);
			aimLeft = false;
			Invoke ("TargetingEnabled", .1f);
		}
	}

	// Up d-pad button
	public void U_DPadBtn(bool down)
	{
		if(down)
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(6);
			aimUp = true;
			targeting = false;
		}
		else
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(7);
			aimUp = false;
			Invoke ("TargetingEnabled", .1f);
		}
	}

	// Down d-pad button
	public void D_DPadBtn(bool down)
	{
		if(down)
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(6);
			aimDown = true;
			targeting = false;
		}
		else
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(7);
			aimDown = false;
			Invoke ("TargetingEnabled", .1f);
		}
	}

	// Left views button
	public void L_ViewsBtn(bool down)
	{
		if(down)
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(3);
			Camera.main.transform.position = cameraLeft;
			Camera.main.transform.LookAt (hisCup1Pos);
		}
		else
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(4);
			Camera.main.transform.position = camPos;
			Camera.main.transform.rotation = camRot;
		}
	}

	// Right views button
	public void R_ViewsBtn(bool down)
	{
		if(down)
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(3);
			Camera.main.transform.position = cameraRight;
			Camera.main.transform.LookAt (hisCup1Pos);
		}
		else
		{
			if(tootEnabled)
				PongToot.tootScript.CallTootStage(4);
			Camera.main.transform.position = camPos;
			Camera.main.transform.rotation = camRot;
		}
	}

	// Returns the starting position for the ghost balls
	public Vector3 GetGhostStartPos()
	{
		return origPos;
	}
}