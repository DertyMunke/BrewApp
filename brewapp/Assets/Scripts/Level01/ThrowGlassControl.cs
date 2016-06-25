using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic;

public class ThrowGlassControl : Touch3D
{
	private CameraManager cameraScript;
	private GameManager managerScript;
	private Patron patScript;
	private Score scoreScript;
	private Transform patArrow;
	private Vector3 armRot = new Vector3 (0, 14.2f, 0);
	private Vector3 cameraStart;
	private Vector3 cam3FollowPos;
	private Quaternion cam3FollowRot;
	private bool endLvl = false;
	private bool cameraReturn = true;
	private bool throwIt = false;
	private bool throwTimeTrigger = true;
	private bool repSwitch = false;	  // This turns off the gui for rep earned
	private bool cameraSwitch = true;
	private bool repBuilder = false;
	private bool touchActive = true;
	private float arrowsTimer = 0;
	private float throwTimer = 0;
	private float throwTimeDelay = .5f;
	private float throwStart = 0;
	private float armRotMax;	// Get max in Start()
	private float throwTipTot = 0f;
	private float armRotMin = 14.2f;
	private float armRotCurr = 0;
	private float xpPercInc = .3f;   // Multiplier for xp increase
	private float rabbitRepPerc = .3f;  // Multiplier for rep increase per level
	private float bearRepPerc = .3f;   // Multiplier for rep increase per level
	private float foxRepPerc = .3f;   // Multiplier for rep increase per level
	private int numThrown = 0;
	private int repTotal = 0;

	public static ThrowGlassControl tgcScript;
	public GameObject viewsBtns;
	public GameObject perfThrow;
	public GameObject arrowPtr;
	public GameObject glassObj = null;
	public GameObject pat;
	public GameObject cam3Follow;
	public GameObject armWithPivot;
	public GUITexture downArrows;
	public Texture2D down_arrow_0;
	public Texture2D down_arrow_1;
	public Texture2D down_arrow_2;
	public Texture2D down_arrow_3;
	public Texture2D down_arrow_4;
	public Texture2D down_arrow_5;
	public Texture2D down_arrow_6;
	public Texture2D down_arrow_7;
	public Texture2D down_arrow_8;
	public Texture2D down_arrow_9;
	public Texture2D down_arrow_10;
	public Texture2D down_arrow_11;
	public Image pwrMeterSkin;
	public Image powerMeter;
	public Image powerMeterPanel;
	public Text numThrownAnim;
	public Sprite pwr_bg;
	public Sprite pwr_1;
	public Sprite pwr_2;
	public Sprite pwr_3;
	public Sprite pwr_4;
	public Sprite pwr_5;
	public Sprite pwr_6;
	public Sprite pwr_7;
	public Sprite pwr_8;
	public Sprite pwr_9;
	public Sprite pwr_10;
	public Sprite pwr_11;
	public Sprite pwr_12;
	public Sprite pwr_13;
	public Sprite pwr_14;
	public Sprite pwr_15;
	public Sprite pwr_16;
	public Sprite pwr_17;
	public Sprite pwr_18;
	public Sprite pwr_19;
	public Sprite pwr_20;
	public Sprite pwr_21;
	public Sprite pwr_22;
	public Sprite pwr_23;
	public Sprite pwr_24;
	public Sprite pwr_25;
	public Sprite pwr_26;
	public Sprite pwr_27;
	public Sprite pwr_28;
	public Sprite pwr_29;
	public Sprite pwr_30;
	public Sprite pwr_31;
	public Sprite pwr_32;
	public Sprite pwr_33;
	public Sprite pwr_34;
	public Sprite pwr_35;
	public Sprite pwr_36;
	public Sprite pwr_37;
	public Sprite pwr_38;
	public Sprite pwr_39;
	public Sprite pwr_40;
	public Sprite pwr_41;
	public Sprite pwr_42;
	public Sprite pwr_43;
	public Sprite pwr_44;
	public Sprite pwr_45;	
	public bool getPoints = true;
	public bool endLevelPause = false;
//	public bool turnOnTarget = false;
	public bool glassThrown = false;
	public bool isMoving = false;
	public bool glassParented = false;
	public float arrowsDelay = 1.5f;
	public float armRotScaler = 2f;
	public float throwPower = 0;
	public float scorePauseTime = 5f;   // For end level pause length
	public float tipAndRepDelay = .7f;
	public int throwPowerTrigger = 1;

	private void Awake()
	{
		tgcScript = this;
	}

	private void Start () 
	{
		armRotMax = armWithPivot.transform.eulerAngles.y;

		cam3FollowPos = cam3Follow.transform.position;
		cam3FollowRot = cam3Follow.transform.rotation;

		cameraScript = GameObject.Find ("Cameras").GetComponent<CameraManager> ();
		scoreScript = Score.scoreScript;
		managerScript = GameManager.manager;

		//SetPerIncLvl ();
		//if(managerScript.GetPongLvl() < 14)
		//	rabbitRepPerc -= (managerScript.GetPongLvl () * .02f);
		//else 
		//	rabbitRepPerc -= .29f;
		//if(managerScript.GetFlipLvl() < 14)
		//	foxRepPerc -= (GameManager.manager.GetFlipLvl () * .02f);
		//else 
		//	foxRepPerc -= .29f;
		//if(managerScript.GetPunchLvl() < 14)
		//	bearRepPerc -= (GameManager.manager.GetPunchLvl () * .02f);
		//else 
		//	bearRepPerc -= .29f;
	}
	
	private void Update()
	{
		if(touchActive)
			TouchController ();

		if(BarController.barControlScript.glass)
		{
			glassObj = BarController.barControlScript.glass;
		}

		if(glassObj)
		{
			CameraFollow ();
		}

		if(throwIt)
		{
			ThrowIt();
		}

		PerfectThrowAnim ();
	}

	// Animation for the arrows in stage 3
	private void DownArrows()
	{
		if(arrowsTimer - Time.time > arrowsDelay * .88f)
		{
			downArrows.texture = down_arrow_0;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .80f)
		{
			downArrows.texture = down_arrow_1;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .72f)
		{
			downArrows.texture = down_arrow_2;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .64f)
		{
			downArrows.texture = down_arrow_3;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .56f)
		{
			downArrows.texture = down_arrow_4;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .48f)
		{
			downArrows.texture = down_arrow_5;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .40f)
		{
			downArrows.texture = down_arrow_6;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .32f)
		{
			downArrows.texture = down_arrow_7;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .24f)
		{
			downArrows.texture = down_arrow_8;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .16f)
		{
			downArrows.texture = down_arrow_9;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > arrowsDelay * .08f)
		{
			downArrows.texture = down_arrow_10;
			downArrows.enabled = true;
		}
		else if(arrowsTimer - Time.time > 0)
		{
			downArrows.texture = down_arrow_11;
			downArrows.enabled = true;
		}
		else
		{
			arrowsTimer = Time.time + arrowsDelay;
		}
	}

	// Throws the glass and resets the power meter and arm position
	private void ThrowIt()
	{
		if(throwTimeTrigger)
		{
			armRotCurr = armWithPivot.transform.eulerAngles.y;
			throwTimer = Time.time + throwTimeDelay;
			throwTimeTrigger = false;
		}

		if(throwTimer - Time.time > throwTimeDelay * .8f)
		{
			armRot = new Vector3(0, armRotCurr + (armRotMax - armRotCurr) * .2f , 0);
			armWithPivot.transform.eulerAngles = armRot;
		}
		else if(throwTimer - Time.time > 0)
		{
			armRot = new Vector3(0, armRotMin, 0);
			armWithPivot.transform.eulerAngles = armRot;
			glassObj.transform.parent = null;
			glassObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			glassObj.GetComponent<Rigidbody>().freezeRotation = true;
			ApplyForce();
			throwPowerTrigger = 1;
			throwPower = 0;
			Invoke("ResetArm", .5f);
			glassParented = false;
			throwIt = false;
			throwTimeTrigger = true;
		}
	}

	// Resets the arm position
	public void ResetArm()
	{
		armRot = new Vector3(0, armRotMax, 0);
		armWithPivot.transform.eulerAngles = armRot;
		throwPower = 0;
		throwPowerTrigger = 1;
		throwStart = 0;
	}
		
	// Initializes the power meter
	public override void OnTouchBegan()
	{
		throwPower = 0;
		throwStart = 0;
		glassObj = BarController.barControlScript.glass;
		throwStart = Input.GetTouch(0).position.y;
		throwPowerTrigger = 2;
	}

	// Inc or dec the power on the power meter
	public override void OnTouchMoved()
	{
		if(TootText.tootScript.tootStage == 4)
			TootText.tootScript.tootStage = 5;

		BarController.barControlScript.slingLabel = false;
		throwPower = throwStart - Input.GetTouch(0).position.y;
	}

	// Triggers the glass to be thrown
	public override void OnTouchEnded()
	{
		if(TootText.tootScript.tootStage == 5)
			TootText.tootScript.tootStage = 6;

		if(throwPowerTrigger == 2 && throwPower > 10)
		{
			BarController.barControlScript.slingLabel = false;
			throwIt = true;
			glassThrown = true;
		}
	}

	// Triggers animation for perfect throw
	private void PerfectThrowAnim()
	{
		if(repSwitch)
		{
			if(perfThrow.GetComponent<Animator>().GetBool("perfTrig"))
			{
				perfThrow.GetComponent<Animator>().SetBool("perfTrig",false);
				repSwitch = false;
			}
			else
				perfThrow.GetComponent<Animator>().SetBool("perfTrig",true);

			if(pat.name == "Rabbit")
			{
				scoreScript.BonusTime(10);
			}			
			else if(pat.name == "Fox")
			{
				scoreScript.BonusTime(20);
			}
			else
			{
				scoreScript.BonusTime(30);
			}

		}

		if(BarController.barControlScript.stage == 3 && glassThrown == false)
		{
			if(!glassParented)
			{
				glassObj.transform.parent = armWithPivot.transform;
				armRot = new Vector3(0, armRotMin, 0);
				glassParented = true;
			}

			armWithPivot.transform.eulerAngles = armRot;
            viewsBtns.SetActive(false);
			powerMeter.enabled = true;
			pwrMeterSkin.enabled = true;
			powerMeterPanel.enabled = true;
			armWithPivot.SetActive(true);
			DownArrows();

			if(throwPower > 235)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 45, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_45;
			}
			else if(throwPower > 230)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 44, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_44;
			}
			else if(throwPower > 225)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 43, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_43;
			}
			else if(throwPower > 220)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 42, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_42;
			}
			else if(throwPower > 215)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 41, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_41;
			}
			else if(throwPower > 210)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 40, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_40;
			}
			else if(throwPower > 205)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 39, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_39;
			}
			else if(throwPower > 200)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 38, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_38;
			}
			else if(throwPower > 195)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 37, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_37;
			}
			else if(throwPower > 190)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 36, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_36;
			}
			else if(throwPower > 185)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 35, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_35;
			}
			else if(throwPower > 180)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 34, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_34;
			}
			else if(throwPower > 175)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 33, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_33;
			}
			else if(throwPower > 170)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 32, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_32;
			}
			else if(throwPower > 165)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 31, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_31;
			}
			else if(throwPower > 160)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 30, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_30;
			}
			else if(throwPower > 155)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 29, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_29;
			}
			else if(throwPower > 150)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 28, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_28;
			}
			else if(throwPower > 145)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 27, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_27;
			}
			else if(throwPower > 140)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 26, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_26;
			}
			else if(throwPower > 135)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 25, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_25;
			}
			else if(throwPower > 130)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 24, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_24;
			}
			else if(throwPower > 125)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 23, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_23;
			}
			else if(throwPower > 120)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 22, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_22;
			}
			else if(throwPower > 115)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 21, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_21;
			}
			else if(throwPower > 110)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 20, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_20;
			}
			else if(throwPower > 105)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 19, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_19;
			}
			else if(throwPower > 100)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 18, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_18;
			}
			else if(throwPower > 95)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 17, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_17;
			}
			else if(throwPower > 90)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 16, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_16;
			}
			else if(throwPower > 85)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 15, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_15;
			}
			else if(throwPower > 80)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 14, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_14;
			}
			else if(throwPower > 75)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 13, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_13;
			}
			else if(throwPower > 70)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 12, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_12;
			}
			else if(throwPower > 65)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 11, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_11;
			}
			else if(throwPower > 60)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 10, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_10;
			}
			else if(throwPower > 55)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 9, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_9;
			}
			else if(throwPower > 50)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 8, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_8;
			}
			else if(throwPower > 45)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 7, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_7;
			}
			else if(throwPower > 40)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 6, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_6;
			}
			else if(throwPower > 35)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 5, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_5;
			}
			else if(throwPower > 30)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 4, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_4;
			}
			else if(throwPower > 20)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 3, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_3;
			}
			else if(throwPower > 10)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 2, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_2;
			}
			else if(throwPower > 5)
			{
				armRot = new Vector3(0, armRotMin + armRotScaler * 1, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_1;
			}
			else if(throwPower <= 0)
			{
				armRot = new Vector3(0, armRotMin, 0);
				powerMeter.GetComponent<Image> ().sprite = pwr_bg;
			}
		}

	}

	// Enables the power meter 
	private void ShowPower(GUITexture turnOn)
	{
		turnOn.enabled = true;
	}

	// Camera follows the glass down the bar
	private void CameraFollow()
	{
		if(glassThrown && !glassObj.transform.GetComponent<Rigidbody>().IsSleeping())
		{
			isMoving = true;
		}

		if(isMoving)
		{
			if(glassObj.transform.position.x <= 3.5)
			{
//				Debug.Log (cameraSwitch);
				if(cameraSwitch)
				{
					viewsBtns.SetActive(false);
					cameraScript.camSwitch3_follow = true;
					powerMeter.enabled = false;
					pwrMeterSkin.enabled = false;
					powerMeterPanel.enabled = false;
					armWithPivot.SetActive (false);
					downArrows.enabled = false;
					cameraSwitch = false;
				}
				else if(glassObj.transform.position.x <= -28)
				{
					//stop the camera from following beer and make it look at beer
					cam3Follow.transform.LookAt(glassObj.transform.position);
				}
				else if(glassObj.transform.position.x <= -15)
				{
					cam3Follow.transform.position = new Vector3(glassObj.transform.position.x, cam3FollowPos.y, cam3FollowPos.z);
				}
			}
		
			if(glassObj.transform.GetComponent<Rigidbody>().IsSleeping() || !getPoints)
			{
				GetPoints();
				if(cameraReturn)
				{
					Invoke("CameraReturn", 1);
				}

				glassObj.GetComponent<GlassControl>().glassDestroy = true;
				glassObj = null;
				isMoving = false;
				cameraSwitch = true;
			}
		}
	}

	// Camera position reset
	private void CameraReturn()
	{
		if(TootText.tootScript.tootStage == 6)
			TootText.tootScript.tootStage = 7;
		BarController.barControlScript.slingLabel = true;
		BarController.barControlScript.glass = null;
		glassObj = null;
		if(!managerScript.barTossToot || TootText.tootScript.tootStage > 8)
			viewsBtns.SetActive (true);
		powerMeter.enabled = false;
		pwrMeterSkin.enabled = false;
		powerMeterPanel.enabled = false;
		armWithPivot.SetActive (false);
		downArrows.enabled = false;
		glassThrown = false;
		isMoving = false;
		cameraSwitch = true;
		cam3Follow.transform.position = cam3FollowPos;
		cam3Follow.transform.rotation = cam3FollowRot;
		getPoints = true;
		repTotal = 0;

		if(BarController.barControlScript.servOrder.Count == 0)
		{
			BarController.barControlScript.leftViewBtn.interactable = false;
			cameraScript.camSwitch1 = true;
			BarController.barControlScript.stage = 1;
		}
		else
		{
			BarController.barControlScript.rightViewBtn.interactable = false;
			cameraScript.camSwitch2 = true;
			BarController.barControlScript.stage = 2;
		}

		if(patArrow)
			patArrow.gameObject.SetActive(false);

		if(patScript)
			patScript.ResetOrder();

		numThrownAnim.text = numThrown.ToString ();
		numThrownAnim.GetComponent<Animator> ().SetTrigger ("ShowCount");
	}

	// Calculates the point according to how close the glass is to the green circle
	private void GetPoints()
	{
		Score.scoreScript.reputation = 0;
		List<GameObject> patsWitOrder = BarController.barControlScript.servOrder;
		Vector3 glassPos;
		if(getPoints)
		{
			glassPos = glassObj.transform.position;
		}
		else
		{
			glassPos = new Vector3(-43, 7.6f, 3.9f); // The end of the bar
		}

		patScript = patsWitOrder[0].GetComponent<Patron>();
		patArrow = patScript.target.transform;
		float distTarget = Vector3.Distance(patArrow.position, glassPos);
		pat = patsWitOrder[0];
		foreach(GameObject p in patsWitOrder)
		{
			patScript = p.GetComponent<Patron>();
			patArrow = patScript.target.transform;
			float dist = Vector3.Distance(patArrow.position, glassPos);
			if(distTarget > dist)
			{
				distTarget = dist;
				pat = p;
			}
		}
	        
		patsWitOrder.Remove (pat);
		if(patsWitOrder.Count == 0)
		{
			BarController.barControlScript.rightViewBtn.interactable = false;
		}

		patScript = pat.GetComponent<Patron>();
		patArrow = patScript.target.transform;

		patScript.animStage = 3;

		if(patScript.beerChoice == glassObj.name)
		{
			throwTipTot += .50f;
			repTotal += 50;
			repBuilder = true;
		}

		//This determins if rep increases
		if(distTarget < 2.1f)
		{
			throwTipTot += .50f;
			repTotal += 50;

			if(repBuilder)
			{
                if (pat.name == "Bear")
                    throwTipTot += 1;
                else if (pat.name == "Fox")
                    throwTipTot += .5f;

                repBuilder = false;
				repSwitch = true;	
				scoreScript.miniGame = patScript.gameChoice;
			}
		}
		else if(distTarget < 4)
		{
			throwTipTot += .40f;
			repTotal += 40;
		}
		else if(distTarget < 5)
		{
			throwTipTot += .30f;
			repTotal += 30;
		}
		else if(distTarget < 6)
		{
			throwTipTot += .20f;
			repTotal += 20;
		}
		else if(distTarget < 7)
		{
			throwTipTot += .10f;
			repTotal += 10;
		}
		else
		{
			throwTipTot += .05f;
			repTotal += 5;
		}

		if(!getPoints)
		{
			throwTipTot = 0;
			repTotal = 0;
		}
		else
			numThrown += 1;

		if(throwTipTot < .5f)
			patScript.SetResponseAnim("bad");
		else if(throwTipTot >= 1)
			patScript.SetResponseAnim("good");
		else
			patScript.SetResponseAnim("ok");
		
		Invoke("tipAndRepEnabled", tipAndRepDelay);
		ScoreAnims.scoreAnimsScript.scoreAnimTrig = true;
		ScoreAnims.scoreAnimsScript.tipValue = throwTipTot;
		ScoreAnims.scoreAnimsScript.repValue = repTotal;

		if(endLvl)
		{
			Invoke("EndLevelPause", scorePauseTime);
		}
	}

	// Helper for GetPoints(): Allows pause to show anims
	public void EndLevelPause()
	{
		endLevelPause = true;
		cameraReturn = false;
	}

	// Determins who to give the rep to
	private void tipAndRepEnabled()
	{
		Score.scoreScript.score += throwTipTot;
		Score.scoreScript.reputation += repTotal;
		Score.scoreScript.levelScore = scoreScript.score; // + scoreScript.timer;
		Score.scoreScript.xpSlider.value += repTotal * xpPercInc;
		GameManager.manager.xpSliderValue = Score.scoreScript.xpSlider.value;

		if(pat.name == "Rabbit")
		{
			Score.scoreScript.PongSliderVal(repTotal * rabbitRepPerc);
		}
		else if(pat.name == "Bear")
		{
			Score.scoreScript.PunchSliderVal(repTotal * bearRepPerc);
		}
		else if(pat.name == "Fox")
		{
			Score.scoreScript.FlipSliderVal(repTotal * foxRepPerc);
		}

		throwTipTot = 0;
	}

	// Applies the force to the glass to throw it down the bar
	private void ApplyForce()
	{
		if(throwPower > 300)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-15000, 0, 0);
		}
		else if(throwPower > 250)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-9500, 0, 0);
		}
		else if(throwPower > 230)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-9000, 0, 0);
		}
		else if(throwPower > 220)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-8500, 0, 0);
		}
		else if(throwPower > 210)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-8000, 0, 0);
		}
		else if(throwPower > 200)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-7800, 0, 0);
		}
		else if(throwPower > 190)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-7600, 0, 0);
		}
		else if(throwPower > 180)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-7400, 0, 0);
		}
		else if(throwPower > 170)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-7200, 0, 0);
		}
		else if(throwPower > 160)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-6800, 0, 0);
		}
		else if(throwPower > 150)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-6400, 0, 0);
		}
		else if(throwPower > 140)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-6100, 0, 0);
		}
		else if(throwPower > 130)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-5800, 0, 0);
		}
		else if(throwPower > 120)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-5400, 0, 0);
		}
		else if(throwPower > 110)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-5000, 0, 0);
		}
		else if(throwPower > 100)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-4600, 0, 0);
		}
		else if(throwPower > 90)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-4200, 0, 0);
		}
		else if(throwPower > 80)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-3800, 0, 0);
		}
		else if(throwPower > 70)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-3400, 0, 0);
		}
		else if(throwPower > 60)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-3000, 0, 0);
		}
		else if(throwPower > 50)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-2500, 0, 0);
		}
		else if(throwPower > 40)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-2000, 0, 0);
		}
		else if(throwPower > 30)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-1500, 0, 0);
		}
		else if(throwPower > 20)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-1000, 0, 0);
		}
		else if(throwPower <= 20)
		{
			glassObj.transform.GetComponent<Rigidbody>().AddForce(-750, 0, 0);
		}
	}

	// Allows other scripts to end the level
	public void EndLevel()
	{
		endLvl = true;
	}

	// Reset the percent increase for xp when you gain a level
	public void SetPerIncLvl()
	{
		if(managerScript.GetBarTossLevel() < 14)
			xpPercInc -= (managerScript.GetBarTossLevel() * .02f);
		else
			xpPercInc -= .29f;
	}

	// Toggles the power meter object touch on and off
	public void TouchActive()
	{
		touchActive = !touchActive;
	}

	// Returns the number of beers successfully thrown
	public int GetNumThrown()
	{
		return numThrown;
	}
}
