using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Patron : Touch3D 
{
	private BarController barScript;
	private Transform myTarget;
	private Vector2 screenCoords;
	private Vector3 myBeerPos;
	private Vector3 myHeadPos;
	private Vector3 myDotsPos;
	private Vector3 myHeadWorldPos;
	private Animator myAnimator;
	private string myName;
	private bool targetEnabled = false; 
	private bool turnOnTarget = true;   
	private bool turnOffTarget = false; 
	private bool countMe = false;
	private bool inList = false;
	private bool startMessage = true;
	private bool beerMessage = false;
	private bool allowTouch = true;
	private float startMessageTimer = 0;
	private float arrowTimer = 0;
	private float showMessageTimer = 0;
	private int beerNum = 0;
	private int numBeers = 0;

	public GameObject target;
	public GameObject myHead;
	public GameObject arrowPtr;
	public GameObject arrowPtrClone;
	public GUITexture chatMessage;
	public GUITexture dots;
	public Texture2D gangway_IPA;
	public Texture2D long_day_lager;
	public Texture2D watership_ale;
	public Texture2D perfect_throw;
	public Texture2D imGood;
	public Vector3 arrowScaler = new Vector3(.02f, .02f, .02f);
	public string[] beers = new string[]{"Gangway IPA", "Long Day Lager", "Watership Brown Ale"};
	public string[] games = new string[]{"BeerPong", "FlipCup", "PunchGame"};
	public string beerChoice;
	public string gameChoice;
	public bool reset = false;
	public bool showArrow = false;  //??
	public bool gotBeer = false;
	public bool isTouched = false;
	public float startMessageDelay = .5f;
	public float showMessageDelay = .5f;
	public float arrowPtrHeight;
	public float arrowTimeDelay = 1.5f;
	public int animStage = 0;

	private void Start () 
	{
		startMessageTimer = Time.time + startMessageDelay;
		InitPatron ();
		barScript = BarController.barControlScript;
	}
	
	private void FixedUpdate()
	{
		if(barScript.stage != 3)
		{
			myHeadPos = Camera.main.WorldToViewportPoint(myHead.transform.position);
			myBeerPos = new Vector3 (myHeadPos.x + .15f, myHeadPos.y + .2f, 0);
			myDotsPos = new Vector3 (myHeadPos.x + .05f, myHeadPos.y + .15f, 0);
		}

		if(reset)
		{
			reset = false;
			startMessage = true;
			animStage = 0;
			SetBeerChoice();

			if(gameObject.name == "Rabbit")
				animation.Play ("BeerPlease");
			else if(gameObject.name == "Bear" || gameObject.name == "Fox")
				myAnimator.SetBool("BeerPls", true);
		}

		if(targetEnabled && barScript.targetEnable)
		{
			if(turnOnTarget)
			{
				turnOnTarget = false;
				turnOffTarget = true;
				AcivateTarget();
			}
		}
		else if(turnOffTarget)
		{
			turnOffTarget = false;
			turnOnTarget = true;
			AcivateTarget(false);
		}

		if(startMessage && barScript.stage != 3) 
			ShowStartMessage();
		else
			dots.enabled = false;

		if(beerMessage)
			GetBeerMessage();
	
		if(allowTouch)
			TouchController ();
	}

	// Initializes the patrons game and beer choice
	private void InitPatron()
	{
		name = gameObject.name;
		numBeers = GameManager.manager.barTossLevel;
		SetBeerChoice ();
		
		if(name == "Rabbit")
		{
			gameChoice = "BeerPong";
		}
		else if(name == "Bear")
		{
			gameChoice = "PunchGame";
			// This will need to be for all patrons when they are done ***********************************
			myAnimator = gameObject.GetComponent<Animator> ();
			myAnimator.SetBool("BeerPls", true);
		}
		else if(name == "Fox")
		{
			gameChoice = "FlipCup";
			myAnimator = gameObject.GetComponent<Animator> ();
			myAnimator.SetBool("BeerPls", true);
		}

	}

	// Pick which beer the patron will enjoy
	private void SetBeerChoice()
	{
		if(numBeers < 2)
		{
			beerNum = Random.Range(0, 1);
		}
		else if(numBeers < 4)
		{
			beerNum = Random.Range(0, 2);
		}
		else
		{
			beerNum = Random.Range(0, 3);
		}
		beerChoice = beers[beerNum];
	}

	// Enables the message over the patron that signals he/she wants a beer
	private void ShowStartMessage()
	{
		dots.transform.position = myDotsPos;

		if(startMessage && startMessageTimer - Time.time > startMessageDelay * .5f && !beerMessage)
		{
			dots.enabled = true;
		}
		else if(startMessage && startMessageTimer - Time.time > 0 && !beerMessage)
		{
			dots.enabled = false;
		}
		else if(startMessage && !beerMessage)
		{
			startMessageTimer = Time.time + startMessageDelay;
		}
		else 
		{
			startMessage = false;
			beerMessage = true;
		}

	}

	// Plays the correct animation and displays the beer choice when selecting a patron
	public override void OnTouchBegan()
	{
		if(!Score.scoreScript.GetTimerActivated() && !GameManager.manager.barTossToot)
			Score.scoreScript.StartTimer();
		showMessageTimer = Time.time + showMessageDelay;

		if(!countMe)
		{
			countMe = true;
			targetEnabled = true;
			BarController.barControlScript.servOrder.Add(gameObject);
//			BarController.barControlScript.numOrders ++;
			BarController.barControlScript.rightViewBtn.interactable = true;
		}
		if(animStage == 0)
		{
			SetIdleAnim();

			animStage = 2;
		}
		else if(animStage == 3)
		{
			gotBeer = true;
		}
		isTouched = true;
		beerMessage = true;
	}

	// Adds the patron to the list of patrons that you have taken the orders of
	public override void OnTouchEnded()
	{	
		targetEnabled = true;

		// Double check to make sure this patron is in the servOrder list
		foreach(GameObject patron in BarController.barControlScript.servOrder)
		{
			if(this.gameObject == patron)
			{
				inList = true;
			}
		}
		
		if(!inList)
		{
			inList = true;
			BarController.barControlScript.servOrder.Add(gameObject);
		}

		if(TootText.tootScript.tootStage == 0)
			TootText.tootScript.tootStage = 1;
		else if(TootText.tootScript.tootStage == 7)
			TootText.tootScript.tootStage = 8;
	}

	// Puts the name of the beers above the taps
	private void GetBeerMessage()
	{
		if (showMessageTimer - Time.time > 0)
		{
			if(gotBeer)
			{
				chatMessage.transform.position = myBeerPos;
				chatMessage.texture = imGood;
				chatMessage.enabled = true;
			}
			else
			{
				if(beerChoice == "Gangway IPA")
				{
					chatMessage.texture = gangway_IPA;
				}
				else if(beerChoice == "Long Day Lager")
				{
					chatMessage.texture = long_day_lager;
				}
				else if(beerChoice == "Watership Brown Ale")
				{
					chatMessage.texture = watership_ale;
				}

				chatMessage.transform.position = myBeerPos;
				chatMessage.enabled = true;
			}
		}
		else
		{
			chatMessage.enabled = false;
			beerMessage = false;
		}
	}

	// Activate target anim
	private void AcivateTarget(bool activate = true)
	{
		if(activate)
		{
			//		showArrow = true;
//			myTarget = transform.FindChild("Target");
//			target.renderer.enabled = true;
//			target.GetComponent<SpriteAnimator>().enabled = true;
			target.SetActive(true);
		}
		else
		{
			target.SetActive(false);
//			target.GetComponent<SpriteAnimator>().enabled = false;
//			target.renderer.enabled = false;
		}
	}
	
	// Determine if this patron wants another beer yet
	public void ResetOrder()
	{
		targetEnabled = false;
		inList = false;
		countMe = false; // Only turn false when this patron wants another beer
		startMessage = true;
		animStage = 0;
		SetBeerChoice();
		if(name == "Rabbit")
		{
			animation.Play ("BeerPlease");
		}
		else
		{
			myAnimator.SetBool("BeerPls", true || name == "Fox");
		}
	}

	// Starts the animation for the response from the patron according to your score for that throw
	public void SetResponseAnim(string quality)
	{
		if(quality == "bad")
		{
			myAnimator.SetTrigger("BadThrow");
		}
		else if(quality == "ok")
		{
			myAnimator.SetTrigger("OkThrow");
		}
		else if(quality == "good")
		{
			myAnimator.SetTrigger("PerfectThrow");
		}
		else
			Debug.Log("Error Patron.cs: Invalid command -> SetRespnseAnim()");
	}

	// Other scripts can disable the patron touch response
	public void ToggleTouch()
	{
		allowTouch = !allowTouch;
	}

	// Sets the idle animation
	public void SetIdleAnim()
	{
		if(name == "Rabbit")
		{
			animation.Play ("IDLE_1");
		}
		else
		{
			myAnimator.SetBool("BeerPls", false);
		}
		startMessage = false;
	}

	// Sets the beer please animation
	public void SetBeerPlsAnim()
	{
		if(name == "Rabbit")
		{
			animation.Play ("BeerPlease");
		}
		else
		{
			myAnimator.SetBool("BeerPls", true);
		}
		startMessage = true;
	}
}
