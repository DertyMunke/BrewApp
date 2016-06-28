using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PunchManager : MonoBehaviour 
{
	private string[] strength = {"Flea", "Mosq", "Mouse", "Todler", "Sissy", "Nerd", "Tough", "Champ", "Herc"};
	private bool myTurn = false;
	private bool dblNothin = false;
    private bool gameOva = false;
	private int bearIndex = 0;
	private int myIndex = 0;
	private int punchNum = 0;
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

    public bool GameOva { get { return gameOva; } }

    public bool SetBag {
        set {
            bool active = value;
            bag.GetComponent<Animator>().SetBool("punch", active);}
    }

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
    /// <summary>
    /// Determines how well you punched and displays it
    /// </summary>
	public void CompletePunch(float pow, float max)
	{
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
			punchCat.text = strength[Mathf.CeilToInt((pow - max*.5f)/ blockSize)];

			if(myTurn)
				myIndex = Mathf.CeilToInt((pow - max*.5f)/ blockSize);
			else
				bearIndex = Mathf.CeilToInt((pow - max*.5f)/ blockSize);
		}

        if(myTurn)
        {
            PunchGameUI.punchGameUIScript.SetTitle = myIndex;
            PunchGameUI.punchGameUIScript.SetPlayer = 1;
        }  
        else
        {
            PunchGameUI.punchGameUIScript.SetTitle = bearIndex;
            PunchGameUI.punchGameUIScript.SetPlayer = 0;
        }

        StartCoroutine(NextPunch());
	}

    /// <summary>
    /// Prepair and reset for the next punch
    /// </summary>
	private IEnumerator NextPunch()
	{
        yield return new WaitForSeconds(2);
        //punchCatPnl.SetActive(true);

        PunchGameUI.punchGameUIScript.PunchTrigger(punchCat.text);
        yield return new WaitForSeconds(0.4f);
        mainCam.enabled = true;
        bearCam.enabled = false;

        PowerMeter2.powerMeter2Script.ResetMeters();

        //yield return new WaitForSeconds(2);
        while (!PunchGameUI.punchGameUIScript.AnimDone)
            yield return null;

        PunchGameUI.punchGameUIScript.AnimDone = false;
        yield return new WaitForSeconds(0.5f);
        PunchGameUI.punchGameUIScript.GoEnabled();
        yield return new WaitForSeconds(1f); 


        if (GameManager.manager.GetPunchToot())
			tootCns.enabled = true;


		if((myTurn && !Winner ()) || !myTurn)
		{
			punchCatPnl.SetActive (false);
            bag.GetComponent<Animator>().SetBool("punch", false);
        }

        PowerMeter2.powerMeter2Script.MeterObj_1.SetActive(true);

        myTurn = !myTurn;

        if (!myTurn && !gameOva)
            BearAI.bearScript.BearIt();
    }

	// Determines winner and updates UI and score
	private bool Winner()
	{
		// Testing
		if(testActive)
			myIndex = bearIndex;
		// |<----

		int punchIndex = punchNum % 3;
	
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
        punchIndex = punchNum % 3;
        PunchGameUI.punchGameUIScript.SetPunch = punchIndex;

        if ((punchNum >= numPunches) || Mathf.Abs(myScore - hisScore) >= 2)
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
        gameOva = true;

		int winState = 2; // 0 = win, 1 = lose, 2 = tie

		if(myScore > hisScore)
		{
			winState = 0;
            GameManager.manager.IncReRoll();
            if (GameManager.manager.GetPunchLvl() == GameManager.manager.GetDifficulty())
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

            PowerMeter2.powerMeter2Script.SetPowerDifficulty(difficulty);

			oddsTxt.text = string.Format ("{0}:1", difficulty);
			wagerTxt.text = string.Format ("{0:F2}", GameManager.manager.GetMyBetAmt ());
			totalTxt.text = string.Format ("{0:F2}", GameManager.manager.GetTotal ());

			BearAI.bearScript.BearIt ();
		}
	}
}
