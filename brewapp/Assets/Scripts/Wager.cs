using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Wager : MonoBehaviour
{
	private GameManager managerScript;
	private List<Button> validBetBtnLst = new List<Button> ();
	private bool reRoll = false;
	private double myTotal = 0;
	private double newTotal = 0;
	private float myBetAmt = 0;
	private float reRollTimer = 100;
	private int miniGamelvl = 0;
	private int diffAnim = 1;

	public static Wager wagerScript;
	public Button[] bets = new Button[3];
	public Button[] menuBtns = new Button[2];
	public Button placeBetBtn;
	public Button reRollBtn;
	public Text betTxt_1;
	public Text betTxt_5;
	public Text betTxt_10;
	public Text numRollsTxt;
	public Text oddsTxt;
	public Text myTotalTxt;
	public int difficulty;  // 1 = 1:1; 2 = 2:1; 3 = 3:1;

	private void Awake()
	{
		wagerScript = this;
	}

	private void Start()
	{
		managerScript = GameManager.manager;
		Init ();
	}

	private void Update()
	{
		ReRollAnim ();
	}

	// The game is paused so have to do this anim in Update()
	private void ReRollAnim()
	{
		if(reRoll)
		{
			if(reRollTimer > 0)
			{
				reRollTimer -= 1f;
				if(reRollTimer < 100 - (diffAnim * 2))
				{
					oddsTxt.text = string.Format("{0}:1", diffAnim % 5);
					diffAnim ++;
				}
			}
			else
			{
				oddsTxt.text = string.Format("{0}:1", difficulty);
				diffAnim = 1;
				reRollTimer = 100;
				reRoll = false;
			}
		}
	}

	// Initialize wager panel values and game difficulty
	private void Init()
	{
		PauseManager.pauseScript.Pause ();
		myTotal = managerScript.GetTotal();
		myTotalTxt.text = string.Format ("${0:F2}", myTotal);
		UsableBetBtns();

		if(Application.loadedLevelName == "FlipCup")
			miniGamelvl = managerScript.GetFlipLvl();
		else if(Application.loadedLevelName == "BeerPong")
			miniGamelvl = managerScript.GetPongLvl();
		else if(Application.loadedLevelName == "Level01_Punch")
			miniGamelvl = managerScript.GetPunchLvl();

		if(managerScript.GetLoadBackup())
		{
			GameManager.manager.GetReRollsBackup();
			difficulty = managerScript.GetLvlDiffBackup();
			managerScript.SetLoadBackup(false);
		}
		else 
		{
			RollDifficulty();
		}

		managerScript.SetLvlDifficulty(difficulty);
		oddsTxt.text = string.Format("{0}:1", difficulty);

		if(managerScript.GetReRolls() > 0)
		{
			if(miniGamelvl > 0)
				reRollBtn.interactable = true;
			numRollsTxt.text = managerScript.GetReRolls().ToString();
		}
	}

	// Picks a random difficulty level
	private void RollDifficulty()
	{
		if(miniGamelvl < 5)
			difficulty = Random.Range (1, 20 + miniGamelvl*20);
		else
			difficulty = Random.Range (1, 100);
		difficulty = Mathf.CeilToInt(difficulty / 20.0f);
		managerScript.SetLvlDiffBackup(difficulty);
	}

	// Determines the usable buttons, puts them in usableLst, and turns off other btns
	private void UsableBetBtns()
	{
		float betbtn = 0;
		float betMultiply = myTotal < 10000 ? Mathf.Floor (Mathf.Log10 ((float)myTotal) - 1) : 2;

		if(myTotal < 1)
		{
			betTxt_1.text = string.Format("${0:F2}", myTotal);
			bets[0].gameObject.name = myTotal.ToString();
		}
		else if(myTotal >= 10)
		{
			if(betMultiply >= 6)
				betMultiply = 5;
			float mul = Mathf.Pow(10, betMultiply);
			betTxt_1.text = string.Format("${0}", mul);
			betTxt_5.text = string.Format("${0}", (5 * mul));
			betTxt_10.text = string.Format("${0}", (10 * mul));

			bets[0].gameObject.name = mul.ToString();
			bets[1].gameObject.name = (5 * mul).ToString();
			bets[2].gameObject.name = (10 * mul).ToString();
		}


		foreach(Button b in bets)
		{
			if(!float.TryParse(b.gameObject.name, out betbtn))
			{
				Debug.Log("Error: Wager.cs -> invalid bet amt");
			}

			if(betbtn <= myTotal)
			{
				b.interactable = true;
				validBetBtnLst.Add(b);
			}
			else
			{
				b.interactable = false;
			}
		}
	}

	// When you click a bet btn, this stores the bet value
	public void BetAmt(Button bet)
	{
		if(!float.TryParse(bet.gameObject.name, out myBetAmt))
		{
			Debug.Log("Error: Wager.cs -> invalid bet amt");
		}
		else
		{
			foreach(Button b in validBetBtnLst)
			{
				b.interactable = true;
			}
			bet.interactable = false;
			newTotal = myTotal - myBetAmt;
			myTotalTxt.text = string.Format ("${0:F2}", newTotal);
			managerScript.SetTotal(newTotal);
		}
	}

	// Starts the level after a bet is placed
	public void PlaceBetBtn()
	{
		if(myBetAmt != 0)
		{
			Time.timeScale = 1;
			managerScript.SetMyBetAmt(myBetAmt);
			gameObject.SetActive(false);
			menuBtns[0].image.enabled = false;
			menuBtns[1].image.enabled = true;
            GameManager.manager.Save();
        }
	}

	// Calls end wager to start the levels ending animations
	public void InitEndWager(int winState)
	{
		EndWager.endWagerScript.InitEndWager (winState, myBetAmt, newTotal, difficulty);
	}

	// Returns your new total dollar amount
	public double GetNewTotal()
	{
		return newTotal;
	}

	// Returns the difficulty level
	public int GetDifficulty()
	{
		return difficulty;
	}

	// Returns the amount you have chosen to bet
	public float GetBetAmt()
	{
		return myBetAmt;
	}

	// Use re-roll button to get a new difficulty level
	public void ReRollDifficulty()
	{
		RollDifficulty ();
		managerScript.SetLvlDifficulty(difficulty);
		managerScript.DecReRoll ();
		numRollsTxt.text = managerScript.GetReRolls ().ToString();
		if(managerScript.GetReRolls() == 0)
		{
			reRollBtn.interactable = false;
		}
		reRoll = true;
	}


}
