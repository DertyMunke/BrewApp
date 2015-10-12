using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PunchManager : MonoBehaviour 
{
	private string[] strength = {"Toddler", "Baby", "Child", "Sissy", "Weakling", "Bully", "Tough Guy", "World Champ", "Herculies"};
	private bool myTurn = false;
	private bool dblNothin = false;
	private float bearIndex = 0;
	private float myIndex = 0;
	private int punchNum = 0;
	private int winner = 0;
	private int difficulty = 0;
	private int hisScore = 0;
	private int myScore = 0;
	private int numPunches = 3;

	public static PunchManager pManagerScript;
	public GameObject punchCatPnl;
	public GameObject loadingImg;
	public GameObject wagerPnl;
	public GameObject bag;
	public GameObject[] menuBtns;
	public Canvas tootCns;
	public Camera bearCam;
	public Camera mainCam;
	public Image[] myGlove = new Image[3];
	public Image[] hisGlove = new Image[3];
	public Image[] myX = new Image[3];
	public Image[] myCheck = new Image[3];
	public Image[] hisX= new Image[3];
	public Image[] hisCheck= new Image[3];
	public Text totalTxt;
	public Text wagerTxt;
	public Text oddsTxt;
	public Text punchCat;
	public Text winnerTxt;
	public bool testActive = true;


	private void Awake()
	{
		pManagerScript = this;
	}

	private void Start()
	{
		Init ();
	}

	// Initializes the current level
	private void Init()
	{
		GameManager.manager.SetPunchMsg (false);
		dblNothin = GameManager.manager.GetDblNothing ();

		if(dblNothin)
		{
			menuBtns[0].SetActive(false);
			menuBtns[1].SetActive(true);

			numPunches = 1;
			wagerPnl.SetActive(false);

			for(int i = 1; i < 3; i++)
			{
				myGlove[i].enabled = false;
				hisGlove[i].enabled = false;
			}

			PauseManager.pauseScript.Pause();
			StartGameBtn();
		}

	}

	// Determines how well you punched and displays it
	public void CompletePunch(float pow, float max)
	{
//		Debug.Log (pow);
		if(pow <= max *.5f)
		{
			punchCat.text = strength[0];

			if(myTurn)
				myIndex = 0;
			else
				bearIndex = 0;
		}
		else
		{
			float blockSize = (max *.5f) / 8;
			punchCat.text = strength[Mathf.FloorToInt((pow - max*.5f)/ blockSize)];

			if(myTurn)
				myIndex = Mathf.CeilToInt((pow - max*.5f)/ blockSize);
			else
				bearIndex = Mathf.CeilToInt((pow - max*.5f)/ blockSize);
		}

		punchCatPnl.SetActive(true);
		Invoke ("NextPunch", 2);
	}

	// Prepair and reset for the next punch
	private void NextPunch()
	{
		Debug.Log ("toot");
		if(GameManager.manager.GetPunchToot())
			tootCns.enabled = true;
		mainCam.enabled = true;
		bearCam.enabled = false;

		if(myTurn && Application.loadedLevelName != "PunchPractice")
		{
			myTurn = false;
			if(!Winner ())
			{
				FingerTrail.fingerScript.ResetRound (myTurn);
				BearAI.bearScript.BearIt();
				punchCatPnl.SetActive (false);
			}

		}
		else
		{
			myTurn = true;
			FingerTrail.fingerScript.ResetRound (myTurn);
			BearAI.bearScript.RestartBear();
			punchCatPnl.SetActive (false);
		}
	}

	// Determines winner and updates UI and score
	private bool Winner()
	{
		// Testing
		if(testActive)
			myIndex = bearIndex;
		// |<----

		int punchIndex = punchNum % 3;

		Debug.Log (myIndex + " " + bearIndex);
		if(myIndex > bearIndex)
		{
			myScore ++;
			myCheck[punchIndex].enabled = true;
			hisX[punchIndex].enabled = true;
		}
		else if(bearIndex > myIndex)
		{
			hisScore ++;
			myX[punchIndex].enabled = true;
			hisCheck[punchIndex].enabled = true;
		}
		else
		{
			myScore ++;
			hisScore ++;
			myCheck[punchIndex].enabled = true;
			hisCheck[punchIndex].enabled = true;
		}

		punchNum ++;

		if((punchNum >= numPunches) || Mathf.Abs(myScore - hisScore) >= 2)
		{
			if(myScore != hisScore)
			{
				GameOver();
				return true;
			}
			else
			{
				int nextIndex = punchNum % 3;
				if(nextIndex == 0)
				{
					for(int i = 0; i < 3; i++)
					{
						myGlove[i].enabled = false;
						hisGlove[i].enabled = false;

						myCheck[i].enabled = false;
						hisCheck[i].enabled = false;

						myX[i].enabled = false;
						hisX[i].enabled = false;
					}
				}

				myGlove[nextIndex].enabled = true;
				hisGlove[nextIndex].enabled = true;
			}

		}

		return false;
	}

	// Ends the game and shows end wager panel and double or nothing challenge
	void GameOver()
	{
		int winState = 2; // 0 = win, 1 = lose, 2 = tie

		if(myScore > hisScore)
		{
			winState = 0;
			if(GameManager.manager.GetPunchLvl() == GameManager.manager.GetDifficulty() - 1)
				GameManager.manager.punchLevel ++;
		}
		else
		{
			winState = 1;
			winnerTxt.color = new Color(.88f, .55f, .44f);
		}

		GameManager.manager.punchSliderVal = 0;

		Wager.wagerScript.InitEndWager (winState);
	}

	// Saves data and returns to beer toss level
	private void EndLevel()
	{
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}

	// Sets the final values for the top banner -> Used with start game button and in Init()
	public void StartGameBtn()
	{
		if (Wager.wagerScript.GetBetAmt () != 0 || dblNothin) 
		{
			menuBtns[0].SetActive(false);
			menuBtns[1].SetActive(true);

			Time.timeScale = 1;
            
			bag.GetComponent<Animator>().enabled = true;
			difficulty = GameManager.manager.GetDifficulty ();
			if (GameManager.manager.GetDblNothing ())
				BearAI.bearScript.SetBearDiff (difficulty < 5 ? difficulty + 1 : difficulty);
			else 
				BearAI.bearScript.SetBearDiff (difficulty);
			PowerMeter.pmeterScript.PowerDifficulty (difficulty);

			oddsTxt.text = string.Format ("{0}:1", difficulty);
			wagerTxt.text = string.Format ("{0:F2}", GameManager.manager.GetMyBetAmt ());
			totalTxt.text = string.Format ("{0:F2}", GameManager.manager.GetTotal ());

			if (Application.loadedLevelName != "PunchPractice")
				BearAI.bearScript.BearIt ();
		}
	}
}
