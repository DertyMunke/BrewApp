using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PongToot : MonoBehaviour 
{
	private int tootStage = 0;

	public static PongToot tootScript;
	public GameObject contBtnObj;
	public GameObject viewsPnl;
	public GameObject powerMtr;
	public GameObject dpadPnl;
	public GameObject topTxtPnl;
	public GameObject midTxtPnl;
	public GameObject botTxtPnl;
	public GameObject viewsTouch;
	public GameObject powerTouch;
	public GameObject dpadTouch;
	public GameObject pwrBarPoint;
	public Button rulesBtn;
	public Button menuBtn;
	public Image rerackTouch;
	public Text topTxt;
	public Text midTxt;
	public Text botTxt;
	public Text topInstTxt;
	public Text midInstTxt;
	public Text botInstTxt;

	public void Awake()
	{
		tootScript = this;
	}

	public void Start()
	{
		if(GameManager.manager.PongTootActive())
		{
			rulesBtn.interactable = false;
			menuBtn.interactable = false;
			topTxtPnl.SetActive(true);
			contBtnObj.SetActive(true);
			viewsPnl.SetActive (false);
			powerMtr.SetActive (false);
			dpadPnl.SetActive(false);
		}
		else if(GameManager.manager.GetPongRackToot())
		{
			tootStage = 15;
		}
		else
			gameObject.SetActive(false);
	}

	/// <summary>
	/// Continues to the next stage of the tutorial
	/// </summary>
	public void ContinueBtn()
	{
		if(tootStage == 0)
		{
			botTxt.text = "Use the green ball path to predict where your ball will land.";
			topTxtPnl.SetActive(false);
			botTxtPnl.SetActive(true);
			tootStage++;
		}
		else if (tootStage == 1)
		{
			// Introduce views buttons
			botTxt.text = "You can use the views buttons to get a side perspective as well.";
			tootStage++;
		}
		else if (tootStage == 2)
		{
			botTxt.text = "Try it by pressing and holding one of the views buttons.";
			botInstTxt.text = "Press and hold a views button...";
			viewsTouch.SetActive(true);
			viewsPnl.SetActive (true);
			contBtnObj.SetActive(false);
			tootStage ++;
		}
		else if(tootStage == 3)
		{
			viewsTouch.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 4)
		{
			botTxt.text = "Perfect! Now lets cover the direction controller. The d-pad will help you aim the ball.";
			contBtnObj.SetActive (true);
			tootStage++;
		}
		else if(tootStage == 5)
		{
			// Introduce d-pad
			botTxt.text = "Now press any direction on the d-pad before we move on.";
			botInstTxt.text = "Press any d-pad button...";
			dpadTouch.SetActive(true);
			dpadPnl.SetActive (true);
			contBtnObj.SetActive (false);
			tootStage++;
		}
		else if(tootStage == 6)
		{
			dpadTouch.SetActive (false);
			tootStage++;
		}
		else if(tootStage == 7)
		{
			topTxt.text = "You did it! Now that you are an expert at aiming, lets learn how to throw.";
			topInstTxt.text = "Touch to continue...";
			dpadTouch.SetActive(false);
			botTxtPnl.SetActive(false);
			topTxtPnl.SetActive(true);
			contBtnObj.SetActive (true);
			tootStage++;
		}
		else if(tootStage == 8)
		{
			// Introduce power button
			botTxt.text = "The white bar on the power meter is the power needed to follow the green path.";
			botInstTxt.text = "Touch to continue...";
			pwrBarPoint.SetActive(true);
			topTxtPnl.SetActive(false);
			botTxtPnl.SetActive(true);
			powerMtr.SetActive(true);
			tootStage++;
		}
		else if(tootStage == 9)
		{
			topTxt.text = "Adjust the direction then hold down the power button until it's almost where you want it, then let go.";
			topInstTxt.text = "Hold power button, then release...";
            topTxtPnl.SetActive(true);
            botTxtPnl.SetActive(false);
			pwrBarPoint.SetActive(false);
			powerTouch.SetActive(true);
			contBtnObj.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 10)
		{
			botTxtPnl.SetActive (false);
			powerTouch.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 11)
		{
			Time.timeScale = 0;
			botTxt.text = "Well, you threw it. You will take turns throwing now.";
			botInstTxt.text = "Touch to continue...";
			botTxtPnl.SetActive(true);
            topTxtPnl.SetActive(false);
			contBtnObj.SetActive (true);
			tootStage++;
		}
		else if(tootStage == 12)
		{
			botTxt.text = "The first one to make all their opponents cups, wins. Good luck!";
			tootStage++;
		}
		else if(tootStage == 13)
		{
			botTxt.text = "Hint: If he throws it and it bounces, you can swipe the ball to keep it from going in!";
			tootStage++;
		}
		else if(tootStage == 14)
		{
			Time.timeScale = 1;
			GameManager.manager.SetPongToot();
			botTxtPnl.SetActive(false);
			contBtnObj.SetActive(false);
			rulesBtn.interactable = true;
			menuBtn.interactable = true;
			tootStage++;
		}
		else if(tootStage == 15)
		{
            Debug.Log("15");

            // Introduce re-rack button
            botTxt.text = "You have an option to re-rack the cups! You only get one and can use it at 4, 3, or 2 cups.";
			botTxtPnl.SetActive(true);
			topTxtPnl.SetActive(false);
			dpadPnl.SetActive(false);
			powerMtr.SetActive (false);
			contBtnObj.SetActive (true);
			rulesBtn.interactable = false;
			menuBtn.interactable = false;
			tootStage++;
		}
		else if(tootStage == 16)
		{
			botTxt.text = "Press the ReRack button to see your options.";
			botInstTxt.text = "Press the ReRack button...";
			contBtnObj.SetActive(false);
			dpadTouch.SetActive(true);
			rerackTouch.rectTransform.localScale = new Vector3(1, -1, 1);
			tootStage++;
		}
		else if(tootStage == 17)
		{
			botTxt.text = "You can choose any available option or press the ReRack button again to minimize the window.";
			botInstTxt.text = "Press a ReRack button...";
			dpadTouch.SetActive (false);
			tootStage++;
		}
		else if(tootStage == 18)
		{
			GameManager.manager.SetPongRackToot();
			botTxtPnl.SetActive(false);
			dpadPnl.SetActive(true);
			powerMtr.SetActive(true);
			rulesBtn.interactable = true;
			menuBtn.interactable = true;
			tootStage++;
		}
	}

	// Returns the current stage number of tootStage
	public int GetTootStage()
	{
		return tootStage;
	}

	// Sets the new stage number of tootStage
	public void SetTootStage(int stage)
	{
		tootStage = stage;
	}

	// If the current tootStage is the same as stage, then calls ContinueBtn for stage action
	public void CallTootStage(int stage)
	{
		if(tootStage == stage)
		{
			ContinueBtn();
		}
	}
}
