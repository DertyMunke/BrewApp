using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeerTossCanvas : MonoBehaviour 
{
	private GameManager managerScript;
	private GameObject tradeRepFrom = null;
	private GameObject tradeRepTo = null;
	private GameObject challenge = null;
	private Color fromColor;
	private Color toColor;
	private Color chalColor;
	private bool expanded = false;
	private bool tradeSelected = false;
	private bool acceptChal = false;
	private bool activateTimer = false;
	private bool repSelectActive = true;
	private float timer = 0;
	private float timeDelay = 5;
	private int tradeRepAmt = 0;

	public static BeerTossCanvas tossCanvasScript;
	public GameObject fox;
	public GameObject bear;
	public GameObject loadingImg;
	public GameObject tradeRepPnl;
	public GameObject expRepPnl_1;
	public GameObject expRepPnl_2;
	public GameObject expRepPnl_3;
	public GameObject pongRep;
	public GameObject pongRepExp;
	public GameObject flipRep;
	public GameObject flipRepExp;
	public GameObject punchRep;
	public GameObject punchRepExp;
	public GameObject startPanel;
	public GameObject tradePanel;
	public GameObject finalPanel;
	public GameObject worldBlocker;
	public Button expandButton;
	public string expand = "expand1";
	public Text tradeRepTxt;
	public Text pongTxt;
	public Text flipTxt;
	public Text punchTxt;

	private void Awake()
	{
		tossCanvasScript = this;
	}

	private void Start()
	{
		managerScript = GameManager.manager; 
	}

	private void Update()
	{
		if(activateTimer)
		{
			if(timer < Time.time)
			{
				activateTimer = false;
//				ChooseXP();
			}
		}
	}

	// Chooses the rep button that will be shown during the game
	private void SelectRepMain(string select)  // "pong", "flip", "punch", "xp"
	{
		CancelBtn();

		Time.timeScale = 1; // Unpause the game
		expandButton.gameObject.SetActive (true);
		pongRepExp.SetActive (false);
		flipRepExp.SetActive (false);
		punchRepExp.SetActive (false);
		tradeRepPnl.SetActive (false);
		expRepPnl_3.SetActive (false);
		worldBlocker.SetActive (false);

		if(select == "pong")
		{
			pongRep.SetActive (true);
			pongRep.GetComponentInChildren<Slider> ().value = managerScript.GetPongRep();
		}
		else if(select == "flip")
		{
			flipRep.SetActive (true);
			flipRep.GetComponentInChildren<Slider> ().value = managerScript.GetFlipRep();
		}
		else if(select == "punch")
		{
			punchRep.SetActive (true);
			punchRep.GetComponentInChildren<Slider> ().value = managerScript.GetPunchRep();
		}
	}

	// Expand reputaion slider buttons
	public void ExpandRepSliders()
	{
		fromColor = pongTxt.color;   // Initializes the color
		expandButton.gameObject.SetActive (false);

		pongRep.SetActive (false);
		flipRep.SetActive (false);
		punchRep.SetActive (false);
		pongRepExp.SetActive (true);
		flipRepExp.SetActive (true);
		punchRepExp.SetActive (true);
		tradeRepPnl.SetActive (true);
		expRepPnl_3.SetActive (true);

		pongRepExp.GetComponentInChildren<Slider> ().value = managerScript.GetPongRep();
		flipRepExp.GetComponentInChildren<Slider> ().value = managerScript.GetFlipRep();
		punchRepExp.GetComponentInChildren<Slider> ().value = managerScript.GetPunchRep();
		activateTimer = true;
		timer = Time.time + timeDelay;
	}

	// Helper function for ExpandRepSliders: Pause after the animation completes
	private void PauseAfterAnim()
	{
		Time.timeScale = 0;
	}

	// Selects the XP button for the main screen
	public void ChooseXP()
	{
		SelectRepMain ("xp");
	}

	// Selects the Pong button for the main screen or chooses pong to trade rep from or to
	public void ChoosePong()
	{
		if(repSelectActive)
		{
			SelectRepMain ("pong");
		}
		else if (acceptChal)
		{
			if(managerScript.GetPongRep () >= 100 && challenge == null)
			{
				if(managerScript.GetPongMsg())
				{
					challenge = pongRepExp;
					chalColor = pongRepExp.GetComponentInChildren<Text>().color;
					pongRepExp.GetComponentInChildren<Text>().color = Color.green;
					tradePanel.SetActive(false);
					finalPanel.SetActive(true);
				}
				else
				{
					tradeRepTxt.text = "You havn't been challenged yet";
				}
			}
		}
		else if (tradeSelected && tradeRepFrom != pongRepExp)
		{
			tradeRepTo = pongRepExp;
			toColor = pongRepExp.GetComponentInChildren<Text>().color;
			pongRepExp.GetComponentInChildren<Text>().color = Color.green;
			tradePanel.SetActive(false);
			finalPanel.SetActive(true);
		}
		else if (!tradeSelected)
		{
			tradeSelected = true;
			fromColor = pongTxt.color;
			pongTxt.color = tradeRepTxt.color;
			tradeRepTxt.color = Color.green;
			tradeRepTxt.text = "Select rep reciever";
			tradeRepFrom = pongRepExp; 
		}
	}

	// Selects the Flip Cup button for the main screen or chooses flip to trade rep from or to
	public void ChooseFlip()
	{
		if(repSelectActive)
		{
			SelectRepMain ("flip");
		}
		else if (acceptChal)
		{
			if(managerScript.GetFlipRep () >= 100 && challenge == null)
			{
				if(managerScript.GetFlipMsg())
				{
					challenge = flipRepExp;
					chalColor = flipRepExp.GetComponentInChildren<Text>().color;
					flipRepExp.GetComponentInChildren<Text>().color = Color.green;
					tradePanel.SetActive(false);
					finalPanel.SetActive(true);
				}
				else
				{
					tradeRepTxt.text = "You havn't been challenged yet";
				}
			}
		}
		else if (tradeSelected && tradeRepFrom != flipRepExp  && fox.activeInHierarchy)
		{
			tradeRepTo = flipRepExp;
			toColor = flipRepExp.GetComponentInChildren<Text>().color;
			flipRepExp.GetComponentInChildren<Text>().color = Color.green;
			tradePanel.SetActive(false);
			finalPanel.SetActive(true);
		}
		else if (!tradeSelected && fox.activeInHierarchy)
		{
			tradeSelected = true;
			fromColor = flipTxt.color;
			flipTxt.color = tradeRepTxt.color;
			tradeRepTxt.color = Color.green;
			tradeRepTxt.text = "Select rep reciever";
			tradeRepFrom = flipRepExp;
		}
	}

	// Selects the Punch button for the main screen or chooses punch to trade rep from or to
	public void ChoosePunch()
	{
		if(repSelectActive)
		{
			SelectRepMain ("punch");
		}
		else if (acceptChal)
		{
			if(managerScript.GetPunchRep () >= 100 && challenge == null)
			{
				if(managerScript.GetPunchMsg())
				{
					challenge = punchRepExp;
					chalColor = punchRepExp.GetComponentInChildren<Text>().color;
					punchRepExp.GetComponentInChildren<Text>().color = Color.green;
					tradePanel.SetActive(false);
					finalPanel.SetActive(true);
				}
				else
				{
					tradeRepTxt.text = "You havn't been challenged yet";
				}
			}
		}
		else if (tradeSelected && tradeRepFrom != punchRepExp && bear.activeInHierarchy)
		{
			tradeRepTo = punchRepExp;
			toColor = punchRepExp.GetComponentInChildren<Text>().color;
			punchRepExp.GetComponentInChildren<Text>().color = Color.green;
			tradePanel.SetActive(false);
			finalPanel.SetActive(true);
		}
		else if(!tradeSelected && bear.activeInHierarchy)
		{
			tradeSelected = true;
			fromColor = punchTxt.color;
			punchTxt.color = tradeRepTxt.color;
			tradeRepTxt.color = Color.green;
			tradeRepTxt.text = "Select rep reciever";
			tradeRepFrom = punchRepExp;
		}
	}

	// Allows you to trade reputation with a penalty
	public void TradeBtn()
	{
		startPanel.SetActive (false);
		tradePanel.SetActive (true);

		if(fox.activeInHierarchy || bear.activeInHierarchy)
		{
			repSelectActive = false;
		}
		else
		{
			tradeRepTxt.text = "No trades available";
		}
	}

	// Cancels a trade attempt
	public void CancelBtn()
	{
		tradeSelected = false;
		acceptChal = false;
		startPanel.SetActive (true);
		tradePanel.SetActive (false);
		finalPanel.SetActive (false);

		if (challenge != null)
			challenge.GetComponentInChildren<Text> ().color = chalColor;
		if(tradeRepTo != null)
			tradeRepTo.GetComponentInChildren<Text> ().color = toColor;
		if(tradeRepFrom != null)
			tradeRepFrom.GetComponentInChildren<Text> ().color = fromColor;
		tradeRepTo = null;
		tradeRepFrom = null;
		challenge = null;
		tradeRepTxt.color = Color.red;
		tradeRepTxt.text = "Select rep to trade";
		repSelectActive = true;
	}

	// Finishes a trade or challenge select
	public void DoItBtn()
	{
		if(challenge)
		{
			Time.timeScale = 1;
			if(challenge == pongRepExp)
			{
				loadingImg.SetActive (true);
				GameManager.manager.NextScene ("BeerPong");
			}
			else if(challenge == flipRepExp)
			{
				loadingImg.SetActive (true);
				GameManager.manager.NextScene ("FlipCup");
			}
			else if(challenge == punchRepExp)
			{
				loadingImg.SetActive (true);
				GameManager.manager.NextScene ("Level01_Punch");
			}
		}

		if(tradeSelected)
		{
			tradeRepTo.GetComponentInChildren<Slider> ().value += tradeRepFrom.GetComponentInChildren<Slider> ().value * .5f;
			tradeRepFrom.GetComponentInChildren<Slider> ().value = 0;
			if(tradeRepTo == pongRepExp)
			{
				managerScript.pongSliderValue = tradeRepTo.GetComponentInChildren<Slider> ().value;
			}
			else if(tradeRepTo == flipRepExp)
			{
				managerScript.flipSliderVal = tradeRepTo.GetComponentInChildren<Slider> ().value;
			}
			else if(tradeRepTo == punchRepExp)
			{
				managerScript.punchSliderVal = tradeRepTo.GetComponentInChildren<Slider> ().value;
			}

			if(tradeRepFrom == pongRepExp)
			{
				managerScript.pongSliderValue = 0;
			}
			else if(tradeRepFrom == flipRepExp)
			{
				managerScript.flipSliderVal = 0;
			}
			else if(tradeRepFrom == punchRepExp)
			{
				managerScript.punchSliderVal = 0;
			}
		}

		CancelBtn ();
	}

	// Allows you to accept a challenge if you have received a challenge message
	public void AcceptChalBtn()
	{
		startPanel.SetActive (false);
		tradePanel.SetActive (true);

		if(managerScript.GetPongMsg() || managerScript.GetFlipMsg() || managerScript.GetPunchMsg())
		{
			tradeRepTxt.color = Color.green;
			tradeRepTxt.text = "Select challenge";
			acceptChal = true;
//			gameObject.GetComponent<Animator> ().enabled = false;
			repSelectActive = false;
		}
		else
		{
			tradeRepTxt.text = "No challenged";
		}
	}
}
