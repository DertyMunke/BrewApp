using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PPManager : Touch3D 
{
	private Vector3[] hisStarPos = new Vector3[6];
	private Vector3[] myStartPos = new Vector3[6];
	private Vector3 lookAtCupsPos;
	private Vector3 lookAtCupsAngle;
	private Vector3 origBallPos;
	private bool iHaveCups = true;
	private bool heHasCups = true;
	private bool myTurn = true;
	private bool setMyLast = true;
	private bool reRackClose = false;
	private bool reRacked = false;
	private bool winner = false;
	private bool dblNothin = false;
	private float camFastLook = 10;
	private int myLastTurn = 0;
	private int hisReRack = 0;
	private int myReRack = 0;
	private int totReRacks = 1;
	private int difficulty = 0;
	private int winLoseDraw = 2; // 0 = win, 1 = lose, 2 = draw
	private int maxCups = 6; // Increases in "double or nothing" when both players make their shot

	public static PPManager ppManager;
	public GameObject[] menuBtns;
	public GameObject[] rerackBtns = new GameObject[3];
	public GameObject[] hisCups = new GameObject[6];
	public GameObject[] myCups = new GameObject[6];
	public GameObject wagerPnl;
	public GameObject loadingImg;
	public GameObject tootCnvs;
	public Sprite[] rerackIms = new Sprite[9];
	public Transform pingPongBall;
	public Text wagerTxt;
	public Text totalTxt;
	public Text oddsTxt;
	public Text winnerTxt;
	public bool cupBonusEnabled = false;
	public float camLookSpeed = .5f;
	public int hisScore = 0;
	public int myScore = 0;

	private void Awake()
	{
		ppManager = this;
	}

	private void Start () 
	{
		Init ();
	}
	
	private void FixedUpdate () 
	{
		if(!winner)
		{
			myTurn = PingPongBall.ppBallScript.myTurn;
			WatchBallFly();
			if(!dblNothin && Application.loadedLevelName != "PongPractice")
			{
				ReRackCheck ();
			}
		}
	}

	// Initializes the level 
	private void Init()
	{
		// Testing
//		myScore = 4;
//		hisScore = 4;
		// |<--

		GameManager.manager.SetPongMsg (false);

		// Checks if its a double or nothing game
		dblNothin = GameManager.manager.GetDblNothing ();
		
		// Gets the start position of each cup
		for(int i = 0; i < hisCups.Length; i++)
		{
			hisStarPos[i] = hisCups[i].transform.position;
			myStartPos[i] = myCups[i].transform.position;
		}

		// Setup the game for the "double or nothing" challenge
		if(dblNothin)
		{
			SetupDblNothing();

			// Starts the game
			PauseManager.pauseScript.Pause();
			StartGameBtn ();

			// Set this so the game knows there's only one more cup for each player to make
			maxCups = 1;
			
			// Testing
//			myScore = 0;
//			hisScore = 0;
			// |<----
		}
		
		setMyLast = true;
		lookAtCupsPos = new Vector3 (transform.position.x, transform.position.y, 5.75f);
		lookAtCupsAngle = new Vector3 (59, transform.rotation.y, transform.rotation.z);
	}

	// Disables all cups but 1 on each side, in a random position
	private void SetupDblNothing()
	{
		menuBtns [0].SetActive (false);
		menuBtns [1].SetActive (true);
		wagerPnl.SetActive (false);

		int randomCup = Random.Range(0, 6); 
		// Turn off all of the cups except for 1, to setup for "double or nothing" challenge
		for(int i = 0; i < hisCups.Length; i++)
		{
			if(randomCup != i)
			{
				hisCups[i].SetActive(false);
				myCups[i].SetActive(false);
			}
			else 
			{
				hisCups[i].SetActive(true);
				myCups[i].SetActive(true);
			}
		}
	}

	// Checks if the re-rack button should be active
	private void ReRackCheck()
	{
		if(myScore >= 2 && myScore <= 4 && myReRack < totReRacks && PingPongBall.ppBallScript.showUI)
		{
			if(GameManager.manager.GetPongRackToot())
			{
				PongToot.tootScript.CallTootStage(15);
			}

			rerackBtns[0].SetActive(true);
			if(myScore == 4)
			{
				rerackBtns[1].GetComponent<Image>().sprite = rerackIms[0];
				rerackBtns[2].GetComponent<Image>().sprite = rerackIms[1];
			}
			else if(myScore == 3)
			{
				rerackBtns[1].GetComponent<Image>().sprite = rerackIms[2];
				rerackBtns[2].GetComponent<Image>().sprite = rerackIms[5];
			}
		}
		else
		{
			foreach(GameObject btn in rerackBtns)
			{
				if(btn.activeInHierarchy)
					btn.SetActive(false);
			}
		}
	}

	// Checks for a winner
	public void GetWinner()
	{
		// New method: First one to make all their cups wins the game except in "double or nothing"
		if(hisScore == maxCups && myScore == maxCups)
		{
			maxCups ++;
			SetupDblNothing();
		}
		else if(hisScore == maxCups)
		{
			GameOver(false);
		}
		else if(myScore == maxCups)
		{
			GameOver();
		}
	}

	// Initializes the end wager panel and the double or nothing panel
	private void GameOver(bool win = true)
	{
		if(win)
		{
			GameManager.manager.IncReRoll();
			if(GameManager.manager.GetPongLvl() == GameManager.manager.GetDifficulty() - 1)
				GameManager.manager.beerPongLevel ++;
			winLoseDraw = 0;
		}
		else
		{
			winLoseDraw = 1;
		}

		winner = true;

		GameManager.manager.pongSliderValue = 0;
		Wager.wagerScript.InitEndWager (winLoseDraw);
	}
	

	// Calls the next scene
	private void EndLevel()
	{
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}

	// The camera watches the ball fly through the air
	private void WatchBallFly()
	{
		if(PingPongBall.ppBallScript.camFollow && pingPongBall.GetComponent<Renderer>().enabled == true)
		{
			Quaternion rotate = Quaternion.LookRotation(pingPongBall.position - transform.position);

			if(!myTurn)
			{
				if(pingPongBall.position.z < 15)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook - 4));
				}
				else if(pingPongBall.position.z < 5)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook - 3));
				}
				else if(pingPongBall.position.z < 2)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook - 2));
				}
				else if(pingPongBall.position.z < 0)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook));
				}
			}
			else 
			{
				if(pingPongBall.position.z > -15)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook - 8));
				}
				else if(pingPongBall.position.z > -5)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook - 5));
				}
				else if(pingPongBall.position.z > -2)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook - 2));
				}
				else if(pingPongBall.position.z > 0)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * (camFastLook));
				}
			}
		}
	}

	// ReRackOpen() helper: Simple animation function
	private void ReRackBtnAnim()
	{
		if(reRackClose)
		{
			reRackClose = false;
			rerackBtns[1].SetActive(false);
		}
		else 
			rerackBtns[2].SetActive(true);

		if(reRacked)
		{
			myReRack ++;
		}
	}

	// Opens the re-rack option btns
	public void ReRackOpen()
	{
		if(rerackBtns[1].activeInHierarchy)
		{
			if(GameManager.manager.GetPongRackToot ())
				PongToot.tootScript.CallTootStage(18);
			reRackClose = true;
			rerackBtns[2].SetActive(false);
		}
		else
		{
			if(GameManager.manager.GetPongRackToot ())
				PongToot.tootScript.CallTootStage(17);
			rerackBtns[1].SetActive(true);
		}
		Invoke("ReRackBtnAnim", .3f);
	}

	// ReRackBtn() helper: Turns all his cups off
	private void TurnOffHisCups()
	{
		foreach (GameObject cup in hisCups) 
		{
			if(cup.activeInHierarchy)
				cup.SetActive(false);
		}
	}

	// Re-racks according to btn 1 graphics
	public void ReRackBtn_1()
	{
		if(GameManager.manager.GetPongRackToot ())
			PongToot.tootScript.CallTootStage(18);
		ReRackOpen ();
		reRacked = true;

		TurnOffHisCups ();
		if(myScore == 4)
		{
			hisCups[0].SetActive(true);
			hisCups[4].SetActive(true);

			hisCups[0].transform.position = new Vector3(hisStarPos[0].x, hisStarPos[0].y, 8.03f);
			hisCups[4].transform.position = hisStarPos[4];
		}
		else if (myScore == 3)
		{
			hisCups[0].SetActive(true);
			hisCups[1].SetActive(true);
			hisCups[4].SetActive(true);

			hisCups[0].transform.position = new Vector3(hisStarPos[0].x, hisStarPos[0].y, 7.07f);
			hisCups[1].transform.position = new Vector3(0, hisStarPos[1].y, 8.09f);
			hisCups[4].transform.position = hisStarPos[4];
		}
		else 
		{
			hisCups[0].SetActive(true);
			hisCups[1].SetActive(true);
			hisCups[2].SetActive(true);
			hisCups[4].SetActive(true);

			hisCups[0].transform.position = hisStarPos[0];
			hisCups[1].transform.position = hisStarPos[1];
			hisCups[2].transform.position = hisStarPos[2];
			hisCups[4].transform.position = hisStarPos[4];
		}
	}

	// Re-racks according to btn 2 graphics
	public void ReRackBtn_2()
	{
		if(GameManager.manager.GetPongRackToot ())
			PongToot.tootScript.CallTootStage(18);
		ReRackOpen ();
		reRacked = true;

		TurnOffHisCups ();
		if(myScore == 4)
		{
			hisCups[4].SetActive(true);
			hisCups[5].SetActive(true);

			hisCups[4].transform.position = new Vector3(.51f, hisStarPos[4].y, hisStarPos[4].z);
			hisCups[5].transform.position = new Vector3(-.51f, hisStarPos[5].y, hisStarPos[5].z);
		}
		else if (myScore == 3)
		{
			hisCups[0].SetActive(true);
			hisCups[1].SetActive(true);
			hisCups[2].SetActive(true);

			hisCups[0].transform.position = new Vector3(hisStarPos[0].x, hisStarPos[0].y, hisStarPos[0].z + .8f);
			hisCups[1].transform.position = new Vector3(hisStarPos[1].x, hisStarPos[1].y, hisStarPos[1].z + .8f);
			hisCups[2].transform.position = new Vector3(hisStarPos[2].x, hisStarPos[2].y, hisStarPos[2].z + .8f);
		}
		else 
		{
			hisCups[1].SetActive(true);
			hisCups[2].SetActive(true);
			hisCups[3].SetActive(true);
			hisCups[5].SetActive(true);

			hisCups[1].transform.position = new Vector3(hisStarPos[1].x, hisStarPos[1].y, 8.1f);
			hisCups[2].transform.position = new Vector3(hisStarPos[2].x, hisStarPos[2].y, 8.1f);
			hisCups[3].transform.position = new Vector3(.517f, hisStarPos[3].y, hisStarPos[3].z);
			hisCups[5].transform.position = new Vector3(-.515f, hisStarPos[5].y, hisStarPos[5].z);
		}
	}

	// Dude calls this when he wants to use his re-rack
	public void ReRackDude()
	{
		if(!dblNothin && hisReRack < totReRacks && hisScore >= 2 && hisScore <= 4)
		{
			int reRack = Random.Range(0, 100);
			if(hisScore == 4 || reRack < 50)
			{
				hisReRack ++;
				foreach (GameObject cup in myCups) 
				{
					if(cup.activeInHierarchy)
						cup.SetActive(false);
				}
				
				int ranNum = Random.Range (0, 100);
				if(hisScore == 4)
				{
					if(ranNum < 50)
					{
						myCups[2].SetActive(true);
						myCups[4].SetActive(true);
						
						myCups[2].transform.position = new Vector3(myStartPos[0].x, myStartPos[0].y, -7.65f);
						myCups[4].transform.position = myStartPos[4];
					}
					else
					{
						myCups[4].SetActive(true);
						myCups[5].SetActive(true);
						
						myCups[4].transform.position = new Vector3(-.51f, myStartPos[4].y, myStartPos[4].z);
						myCups[5].transform.position = new Vector3(.51f, myStartPos[5].y, myStartPos[5].z);
					}
					
				}
				else if (hisScore == 3)
				{
					if(ranNum < 50)
					{
						myCups[0].SetActive(true);
						myCups[1].SetActive(true);
						myCups[4].SetActive(true);
						
						myCups[0].transform.position = new Vector3(myStartPos[0].x, myStartPos[0].y, -6.63f);
						myCups[1].transform.position = new Vector3(0, myStartPos[1].y, -7.65f);
						myCups[4].transform.position = myStartPos[4];
					}
					else
					{
						myCups[0].SetActive(true);
						myCups[1].SetActive(true);
						myCups[2].SetActive(true);
						
						myCups[0].transform.position = new Vector3(myStartPos[0].x, myStartPos[0].y, myStartPos[0].z - .8f);
						myCups[1].transform.position = new Vector3(myStartPos[1].x, myStartPos[1].y, myStartPos[1].z - .8f);
						myCups[2].transform.position = new Vector3(myStartPos[2].x, myStartPos[2].y, myStartPos[2].z - .8f);
					}
					
				}
				else 
				{
					if(ranNum < 50)
					{
						myCups[0].SetActive(true);
						myCups[1].SetActive(true);
						myCups[2].SetActive(true);
						myCups[4].SetActive(true);
						
						myCups[0].transform.position = myStartPos[0];
						myCups[1].transform.position = myStartPos[1];
						myCups[2].transform.position = myStartPos[2];
						myCups[4].transform.position = myStartPos[4];
					}
					else
					{
						myCups[1].SetActive(true);
						myCups[2].SetActive(true);
						myCups[3].SetActive(true);
						myCups[5].SetActive(true);
						
						myCups[1].transform.position = new Vector3(myStartPos[1].x, myStartPos[1].y, -7.65f);
						myCups[2].transform.position = new Vector3(myStartPos[2].x, myStartPos[2].y, -7.65f);
						myCups[3].transform.position = new Vector3(-.517f, myStartPos[3].y, myStartPos[3].z);
						myCups[5].transform.position = new Vector3(.515f, myStartPos[5].y, myStartPos[5].z);
					}
				}
			}
		}
	}

	// Returns the difficulty level
	public int GetLvlDifficulty()
	{
		return difficulty;
	}

	// Sets a random cup active for practice scene
	public void SetRandomCupActive()
	{
		foreach(GameObject cup in hisCups)
		{
			if(cup.activeInHierarchy)
				return;
		}
		int ran = Random.Range(0, 6);
		hisCups[ran].SetActive(true);
	}

	// Returns the value of dblNothin
	public bool GetDblNothin()
	{
		return dblNothin;
	}

	// Sets the final values for the top banner -> Used with start game button and in Init()
	public void StartGameBtn()
	{
		if (Wager.wagerScript.GetBetAmt () != 0) {
			menuBtns [0].SetActive (false);
			menuBtns [1].SetActive (true);
			difficulty = GameManager.manager.GetDifficulty ();
			Dude.dudeScript.SetDudeDiff (difficulty);

			oddsTxt.text = string.Format ("{0}:1", difficulty);
			wagerTxt.text = string.Format ("{0:F2}", GameManager.manager.GetMyBetAmt ());
			totalTxt.text = string.Format ("{0:F2}", GameManager.manager.GetTotal ());
			tootCnvs.SetActive (true);
		}
	}
}
