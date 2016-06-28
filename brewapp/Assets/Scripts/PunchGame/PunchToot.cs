using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PunchToot : MonoBehaviour 
{
	private int tootStage = 0;
	
	public static PunchToot punchTootScript;
	public GameObject topTootPnl;
	public GameObject midTootPnl;
	public GameObject botTootPnl;
	public GameObject powerPointer;
	public GameObject btn1Touch;
	public GameObject btn2Touch;
	public GameObject swipe1Arrow;
	public GameObject swipe2Arrow;
	public Button contBtn;
	public Button menuBtn;
	public Button rulesBtn;
	public Text topTxt;
	public Text midTxt;
	public Text botTxt;
	public Text topInstTxt;
	public Text midInstTxt;
	public Text botInstTxt;
	
	private void Start()
	{
		punchTootScript = this;
		//menuBtn.enabled = false;
		//rulesBtn.enabled = false;
	}
	
	// Tutorial walkthrough for flip cup
	public void ContinueBtn()
	{
		if (tootStage == 0) 
		{
			topTxt.text = "You and your opponent will take turns hitting the hive and the strongest 2 out of 3 wins!";	
			tootStage++;
		}
		else if(tootStage == 1)
		{
			topTxt.text = "There are 2 meters. One for focus and one for power.";
			tootStage++;
		}
		else if(tootStage == 2)
		{
			botTxt.text = "Push the power button on the right and release it when the power is on the line.";
			botInstTxt.text = "Push and hold the button...";
			topTootPnl.SetActive(false);
			botTootPnl.SetActive(true);
			btn1Touch.SetActive(true);
			contBtn.gameObject.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 3)
		{
            botTxt.text = "Touch anywhere to stop the focus meter on the center line.";
            botInstTxt.text = "Touch anywhere...";
            btn1Touch.SetActive(false);
            menuBtn.enabled = true;
			rulesBtn.enabled = true;
			tootStage++;
		}
		else if(tootStage == 4)
		{
            GameManager.manager.SetPunchToot();
            gameObject.SetActive(false);
            tootStage++;
		}
		else if(tootStage == 5)
		{
			tootStage++;
		}
	}
	
	// Checks if the stage is correct and continues the tutorial 
	public void CheckContinue(int stage)
	{
		if (stage == tootStage)
		{
			ContinueBtn();
		}
	}
}
