using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour 
{
	private GameManager manager;
	private Color green = new Color(.48f, .82f, .07f);
	private Color yellow = new Color(.85f, .91f, .04f);
	private Color myColor;
	private float highScore;
	private float timeDelay = 1;
	private int timer;
	private bool onlyOnce = false;
	private bool timerActivated = false;

	public static Score scoreScript;
	public GameObject endTootPnl;
	public GameObject loadingImg;
	public GameObject msgsCanvas;
	public GameObject worldBlocker;
	public GameObject xpRepPnl;
	public GameObject endLvlPnl;
	public Canvas beerTossCanvas;
	public Slider xpSlider;
	public Slider pongSlider;
	public Slider pongSlider_2;
	public Slider flipSlider;
	public Slider flipSlider_2;
	public Slider punchSlider;
	public Slider punchSlider_2;
	public Button bearMsgBtn;
	public Button hareMsgBtn;
	public Button foxMsgBtn;
	public Button acceptNowButton;
	public Button maybeLaterButton;
	public Text levelNumTxt;
	public Text numMsgs;
	public Text tips;
	public Text bonus;
	public Text total;
	public Text levelTips;
	public string miniGame = null;
	public bool tootBonusSwitch = false;
	public bool flipCup = false;
	public bool darts = false;
	public bool dasBoot = false;
	public float score;
//	public float timer;
	public float levelScore = 0;
	public int reputation = 0;

	private void Awake()
	{
		scoreScript = this;
	}

	private void Start () 
	{
		manager = GameManager.manager;
		xpSlider.value = manager.xpSliderValue;
		pongSlider.value = manager.pongSliderValue;
		flipSlider.value = manager.flipSliderVal;
		punchSlider.value = manager.punchSliderVal;
		levelNumTxt.text = string.Format ("{0}", manager.GetBarTossLevel ());
		myColor = bonus.color;
		reputation = 0;
		timer = 60;
//		if(!manager.barTossToot)
//			StartCoroutine ("Timer");
	}

	private void Update () 
	{
		if(xpSlider.value >= xpSlider.maxValue)
		{
			xpSlider.value = 0;
			manager.xpSliderValue = 0;
			manager.barTossLevel ++;
//			manager.maxBeerTossXP = manager.maxBeerTossXP * 2;
			levelNumTxt.text = string.Format ("{0}", manager.GetBarTossLevel());
			ThrowGlassControl.tgcScript.SetPerIncLvl();
		}
		ScoreGUI ();
	}

	// Allows you to start the timer from another script
	public void StartTimer()
	{
		timerActivated = true;
		StartCoroutine ("Timer");
	}

	// Animates the countdown for the end of the level and determines when to end the level
	private IEnumerator Timer()
	{
		while(timer > 0)
		{
			timer --;
			if(bonus.color != green)
			{
				if(timer <= 10 && bonus.color != Color.red)
				{
					bonus.color = Color.red;
				}
				else if(timer < 20 && bonus.color != yellow)
				{
					bonus.color = yellow;
				}
			}

			timeDelay -= (timeDelay * .01f);
			yield return new WaitForSeconds (timeDelay);
		}

		if(!ThrowGlassControl.tgcScript.isMoving)
		{
			// Ends the game if the glass has not been thrown
			ThrowGlassControl.tgcScript.EndLevelPause ();
		}
		else
		{
			// If a beer has been thrown, allow it to finish before quitting
			ThrowGlassControl.tgcScript.EndLevel ();
		}

	}

	// Displays the score in the top banner
	private void ScoreGUI()
	{
		tips.text = string.Format("{0:F2}", score);
		bonus.text =  string.Format("{0}", timer);
		total.text = string.Format("{0:F2}", manager.total);

		if(ThrowGlassControl.tgcScript.endLevelPause && !onlyOnce)
		{
			xpRepPnl.SetActive(false);
			manager.total += score;

			if(score > manager.GetHighTips())
			{
				manager.SetHighTips(score);
			}

			int numThrown = ThrowGlassControl.tgcScript.GetNumThrown(); //BarController.barControlScript.numOrders;
			if(numThrown > manager.GetHighThrown())
			{
				manager.SetHighThrown(numThrown);
			}

			if(!manager.barTossToot)
			{	
				levelTips.text = string.Format("Your Tips   = ${0:F2}\n\nHigh Tips   = ${1:F2}\n\nYou Served  = {2}\n\nHigh Served = {3}", 
				                               score, manager.GetHighTips(), numThrown, manager.GetHighThrown()); 
				endLvlPnl.SetActive (true);
			}
			manager.Save(manager.currProfileName);
			onlyOnce = true;
		}
	}

	// Checks if you have enough rep to get challenged to a mini game
 	public void nextLevel()
	{	
		if(manager.GetPongRep() >= 100 || manager.GetFlipRep() >= 100 || manager.GetPunchRep() >= 100)
		{
			int count = 0;

			hareMsgBtn.interactable = false;
			foxMsgBtn.interactable = false;
			bearMsgBtn.interactable = false;
			endLvlPnl.SetActive (false);
			msgsCanvas.SetActive(true);

			if(manager.GetPongRep() >= 100)
			{
				Debug.Log("hare");
				manager.SetPongMsg();
				hareMsgBtn.interactable = true;
				count++;
			}

			if(manager.GetFlipRep() >= 100)
			{
				Debug.Log("fox");
				manager.SetFlipMsg();
				foxMsgBtn.interactable = true;
				count++;
			}

			if(manager.GetPunchRep() >= 100)
			{
				Debug.Log("bear");
				manager.SetPunchMsg();
				bearMsgBtn.interactable = true;
				count++;
			}

			numMsgs.text = string.Format("You have {0} new messages!", count);
		}
		else
		{
			loadingImg.SetActive (true);
			manager.NextScene ("BeerToss");
		}
	}

	// Allows another script to get the time of the timer
	public float GetTimerTime()
	{
		return timer;
	}

	// Adds extra bonus time to the timer with perfect throw
	public void BonusTime(int extra)
	{

		if(timer != 0)
		{
			bonus.color = green;
			timer += extra;
		}

		Invoke ("BonusPause", 1);
	}

	// Helper function for BonusTime(): Set origional color to bonus txt
	private void BonusPause()
	{
		bonus.color = myColor;
	}

	// Sets the reputation values for beer pong 
	public void PongSliderVal(float val)
	{
		manager.SetPongRep (manager.GetPongRep () + val);
		pongSlider.value = manager.GetPongRep();
		pongSlider_2.value = manager.GetPongRep ();

	}

	// Sets the reputation values for flip cup
	public void FlipSliderVal(float val)
	{
		manager.SetFlipRep (manager.GetFlipRep () + val);
		flipSlider.value = manager.GetFlipRep ();
		flipSlider_2.value = manager.GetFlipRep ();
	}

	// Sets the reputation values for the punch game
	public void PunchSliderVal(float val)
	{
		manager.SetPunchRep (manager.GetPunchRep () + val);
		punchSlider.value = manager.GetPunchRep ();
		punchSlider_2.value = manager.GetPunchRep ();
	}

	// Returns true if the timer is still running
	public bool GetTimerActivated()
	{
		return timerActivated;
	}
}
	