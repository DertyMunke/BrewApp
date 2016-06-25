using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class TootText : MonoBehaviour 
{
	private Patron patronScript; // Not generic needs to be assigned to a specific patrons Patron.cs
	private int subStage = 0;
	private int newToot = 0;
	
	public static TootText tootScript;
	public GameObject[] blockers = new GameObject[2];
	public GameObject[] patrons = new GameObject[3];
	public GameObject powerArrows;
	public GameObject rulesTutTouch;
	public GameObject menuTutTouch;
	public GameObject xpTutTouch;
	public GameObject loadingImg;
	public GameObject topTootPnl;
	public GameObject midTootPnl;
	public GameObject botTootPnl;
	public GameObject topBnrObj;
	public GameObject xpSliderObj;
	public GameObject xpBtnObj;
	public GameObject viewsPnlObj;
	public GameObject tutTouchObj;
	public GameObject tutDragObj;
	public GameObject pwrTutDrag;
	public GameObject throwGlassTrigger;
	public Button continueBtn;
    public Button preContBtn;
	public Button rulesBtn;
	public Button menuBtn;
	public Button xpRepBtn;
	public Button flipRepBtn;
	public Button punchRepBtn;
	public Text[] headerTxt = new Text[6]; // 0,1 = Tips; 2,3 = Timer; 4,5 = Total;
	public Text[] topTootTxt = new Text[2]; // 0 = Story; 1 = Instruction
	public Text[] midTootTxt = new Text[2]; // 0 = Story; 1 = Instruction
	public Text[] botTootTxt = new Text[2]; // 0 = Story; 1 = Instruction
	public int tootStage = 0;
	public int lastTootStage = 0;

	void Awake()
	{
		tootScript = this;
	}

	void Start()
	{
		if(GameManager.manager.barTossToot)
		{
			Init ();
		}
	}

	void FixedUpdate()
	{
		if(GameManager.manager.barTossToot)
		{
			if(tootStage == 1 && subStage == 0)
			{
				// Camera 1: Starts the game walkthrough
				subStage = 1;
				topTootTxt[0].text = "He ordered a Gangway IPA. You will need to remember the customers orders to earn more $.";
				topTootTxt[1].text = "Touch to continue...";
				continueBtn.gameObject.SetActive(true);
				viewsPnlObj.SetActive(true);
			}
			else if(tootStage == 2 && subStage == 1)
			{
				// Camera 2: Before you pull the tap handle
				subStage = 0;
				botTootTxt[0].text = "Grab the Gangway IPA tap handle and pull down to pour the beer";
				botTootTxt[1].text = "Swipe down on the tap handle...";
				topTootPnl.SetActive(false);
				botTootPnl.SetActive(true);
				tutTouchObj.SetActive(false);
				viewsPnlObj.SetActive(false);
				tutDragObj.SetActive(true);
			}
			else if(tootStage == 3 && subStage == 0)
			{
				// After you pull the tap handle
				subStage = 1;
				tutDragObj.SetActive(false);
				botTootPnl.SetActive (false);
			}
			else if(tootStage == 4 && subStage == 1)
			{
				// Camera 3: The start of where you throw the beer down the bar
				subStage = 0;
				botTootTxt[0].text = "Use the bubbly power meter to judge how hard you will throw the beer.";
				botTootTxt[1].text = "Touch to continue...";
				botTootPnl.SetActive(true);
				continueBtn.gameObject.SetActive(true);
				ThrowGlassControl.tgcScript.TouchActive();
			}
			else if(tootStage == 5 && subStage == 0)
			{
				subStage = 1;
				pwrTutDrag.SetActive(false);
			}
			else if(tootStage == 6 && subStage == 1)
			{
				subStage = 0;
				pwrTutDrag.SetActive(false);

			}
			else if(tootStage == 7 && subStage == 0)
			{
				viewsPnlObj.SetActive(true);
				if(!OverthrowCollider.overThrowScript.GetOverthrow())
				{
					subStage = 1;
					topTootTxt[0].text = "You've earned a tip! Use tips to bet on other games!";
					xpBtnObj.SetActive(true);
					xpSliderObj.SetActive(true);
					headerTxt[0].enabled = true;
					headerTxt[1].enabled = true;
					headerTxt[4].enabled = true;
					headerTxt[5].enabled = true;
					patronScript.ToggleTouch();
					patronScript.SetIdleAnim();
					newToot++;
				}
				else
				{
					tootStage = 6;
					topTootTxt[0].text = "You threw it too hard. Try again, but keep the power in the green.";
				}
				continueBtn.gameObject.SetActive(true);
				topTootPnl.SetActive(true);
				topTootTxt[1].text = "Touch to continue...";
			}
			else if(tootStage == 8 && subStage == 1)
			{
				subStage = 0;
				menuBtn.interactable = true;
				rulesBtn.interactable = true;
				topTootPnl.SetActive (false);
				Score.scoreScript.StartTimer();
			}
			else if(tootStage == 9 && subStage == 0)
			{
				GameManager.manager.barTossToot = false;
				subStage = 1;
			}
		}
	}

	// Activates the initial tutorial: used for a delay
	private void TutTouchActive()
	{
		tutTouchObj.SetActive(true);
		viewsPnlObj.SetActive(true);
	}

	private void Init()
	{
		foreach (Text t in headerTxt)
		{
			t.enabled = false;
		}
		patronScript = patrons [0].GetComponent<Patron> (); // Sets patronScript to rabbit
		patrons [0].SetActive (false);
		viewsPnlObj.SetActive(false);
		xpBtnObj.SetActive (false);
		xpSliderObj.SetActive(false);
		topTootPnl.SetActive (true);
		menuBtn.image.enabled = false;
		rulesBtn.image.enabled = false;
		continueBtn.gameObject.SetActive (true);
	}

	// Pass true to replay the tutorial or false to play the game
	public void TootBtn(bool play)
	{
		GameManager.manager.barTossToot = play;
		GameManager.manager.Save ();
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}

	// When the user touches anywhere to continue
	public void ContinueBtn()
	{
		if(newToot == 0)
		{
			topTootPnl.SetActive(false);
			midTootPnl.SetActive(true);
			rulesTutTouch.SetActive(true);
			continueBtn.gameObject.SetActive(false);
			rulesBtn.image.enabled = true;
			newToot++;
		}
		//else if(newToot == 3)
		//{
		//	midTootTxt[0].text = "This is the menu button. Push it.";
		//	midTootTxt[1].text = "Push the menu button...";
		//	continueBtn.gameObject.SetActive (false);
		//	topTootPnl.SetActive(false);
		//	midTootPnl.SetActive(true);
		//	menuTutTouch.SetActive(true);
		//	menuBtn.image.enabled = true;
		//	newToot++;
		//}
		else if(newToot == 6)
		{
			topTootTxt[0].text = "Press the right views button to go to the taps";
			topTootTxt[1].text = "Push the right views button...";
			tutTouchObj.SetActive(true);
			continueBtn.gameObject.SetActive (false);
			newToot++;
		}
		else if(newToot == 7)
		{
            // Camera 3: Introduce swipe power
            pwrTutDrag.SetActive(true);
            continueBtn.gameObject.SetActive(false);
            preContBtn.gameObject.SetActive(true);
            botTootTxt[0].text = "Touch anywhere, hold and slowly swipe down for power.";
			botTootTxt[1].text = "Touch, hold and swipe...";
            ThrowGlassControl.tgcScript.TouchActive();
            newToot++;
		}
		else if(newToot == 8)
		{
			//ThrowGlassControl.tgcScript.TouchActive();
			botTootPnl.SetActive(false);
			preContBtn.gameObject.SetActive(false);
			
			newToot++;
		}
		else if(newToot == 9)
		{
			continueBtn.gameObject.SetActive(false);
			topTootPnl.SetActive(false);
		}
		else if(newToot == 10)
		{
			topTootTxt[0].text = "At the end of each round, your tips will move to the bank.";
			newToot++;
		}
		else if(newToot == 11)
		{
			topTootTxt[0].text = "You can use your bank roll to bet on yourself in side games.";
			newToot++;
		}
        //else if(newToot == 12)
        //{
        //	midTootTxt[0].text = "Here is the experience and reputation button. Give it a press.";
        //	midTootTxt[1].text = "Push the \"XP\" button...";
        //	continueBtn.gameObject.SetActive (false);
        //	topTootPnl.SetActive(false);
        //	midTootPnl.SetActive(true);
        //	xpBtnObj.SetActive(true);
        //	xpSliderObj.SetActive(true);
        //	xpTutTouch.SetActive(true);
        //	newToot++;
        //}
        //else if(newToot == 14)
        //{
        //	topTootTxt[0].text = "Press the pong button to view your beer pong rep while bartending.";
        //	topTootTxt[1].text = "Push the rep button...";
        //	punchRepBtn.interactable = false;
        //	flipRepBtn.interactable = false;
        //	xpRepBtn.interactable = false;
        //	continueBtn.gameObject.SetActive(false);
        //	botTootPnl.SetActive(false);
        //	topTootPnl.SetActive(true);
        //	newToot++;
        //}
        else if (newToot == 12)
        {
            topTootTxt[0].text = "One last thing. Check out the timer on the top banner...";
            topTootTxt[1].text = "Touch to continue...";
            punchRepBtn.interactable = true;
            flipRepBtn.interactable = true;
            xpRepBtn.interactable = true;
            continueBtn.gameObject.SetActive(true);
            headerTxt[2].enabled = true;
            headerTxt[3].enabled = true;
            newToot++;
        }
        else if(newToot == 13)
		{
			topTootTxt[0].text = "See how many beers you can serve before your time runs out.";
			topTootTxt[1].text = "Touch to continue...";
			continueBtn.gameObject.SetActive(true);
			headerTxt[2].enabled = true;
			headerTxt[3].enabled = true;
			newToot++;
		}
		else if(newToot == 14)
		{
			topTootTxt[0].text = "Touch the rabbit to start timer.";
			topTootTxt[1].text = "Touch the rabbit...";
			continueBtn.gameObject.SetActive(false);
			patronScript.ToggleTouch();
			patronScript.SetBeerPlsAnim();
			newToot++;
		}
		else if(newToot == 18)
		{
//			topTootPnl.SetActive(false);
//			continueBtn.gameObject.SetActive(false);
//			GameManager.manager.barTossToot = false;
//			ThrowGlassControl.tgcScript.TouchActive();
//			Time.timeScale = 1;
//			newToot++;
		}
	}

	// When the user touches the rules button to continue
	public void RulesContBtn()
	{
		if(newToot == 1)
		{
			topTootTxt[0].text = "Use these secrets to improve your game.";
			topTootTxt[1].text = "Push \"Continue\" to continue...";
			rulesTutTouch.SetActive(false);
			midTootPnl.SetActive(false);
			topTootPnl.SetActive(true);
			rulesBtn.interactable = false;
			newToot++;
		}
		else if(newToot == 2)
		{
            //topTootTxt[0].text = "Lets discuss one more button before the customers start coming in.";
            //topTootTxt[1].text = "Touch to continue...";
            //continueBtn.gameObject.SetActive (true);
            midTootTxt[0].text = "This is the menu button. Push it.";
            midTootTxt[1].text = "Push the menu button...";
            continueBtn.gameObject.SetActive(false);
            topTootPnl.SetActive(false);
            midTootPnl.SetActive(true);
            menuTutTouch.SetActive(true);
            menuBtn.image.enabled = true;
            newToot += 2;
		}
	}

	// When the user touches the menu button to continue
	public void MenuContBtn()
	{
		if(newToot == 4)
		{
			topTootTxt[0].text = "This is the menu. Push the button again.";
			topTootTxt[1].text = "Push the menu button to continue...";
			midTootPnl.SetActive(false);
			topTootPnl.SetActive(true);
			menuTutTouch.SetActive(false);
			newToot++;
		}
		else if(newToot == 5)
		{
			topTootTxt[0].text = "Great Job! Touch the customer to get his order.";
			topTootTxt[1].text = "Touch the rabbit...";
			patrons[0].SetActive(true);
			menuBtn.interactable = false;
			newToot++;
		}
	}

	// If the options button is pushed during tutorial
	public void OptionsContBtn()
	{
		if(newToot == 5)
		{
			newToot = 4;
			topTootPnl.SetActive(false);
			botTootPnl.SetActive(true);
		}
		else if(newToot == 4)
		{
			topTootPnl.SetActive(true);
			botTootPnl.SetActive(false);
			newToot++;
		}
	}

	// When the user touches the xp button to continue
	public void XpContBtn()
	{
		if(newToot == 13)
		{
			botTootTxt[0].text = "You can trade rep or accept challenges that you've previously passed on.";
			botTootTxt[1].text = "Touch to continue...";
			midTootPnl.SetActive(false);
			botTootPnl.SetActive(true);
			continueBtn.gameObject.SetActive (true);
			xpTutTouch.SetActive(false);
			newToot++;
		}
		else if(newToot == 15)
		{
			topTootTxt[0].text = "One last thing. Check out the timer on the top banner...";
			topTootTxt[1].text = "Touch to continue...";
			punchRepBtn.interactable = true;
			flipRepBtn.interactable = true;
			xpRepBtn.interactable = true;
			continueBtn.gameObject.SetActive (true);
			headerTxt[2].enabled = true;
			headerTxt[3].enabled = true;
			newToot++;
		}
	}
}
