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
		menuBtn.enabled = false;
		rulesBtn.enabled = false;
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
			topTxt.text = "There are 3 meters. One to psych yourself up, one for focus, and one for power.";
			powerPointer.SetActive(true);
			tootStage++;
		}
		else if(tootStage == 2)
		{
			midTxt.text = "Tap the button to get psyched. The bottom left meter will tell you how psyched you are.";
			botTxt.text = "When you're plenty psyched, swipe down to get a focus bonus and reveal the focus button.";
			botInstTxt.text = "Tap the button...";
			topTootPnl.SetActive(false);
			midTootPnl.SetActive(true);
			botTootPnl.SetActive(true);
			btn1Touch.SetActive(true);
			powerPointer.SetActive(false);
			contBtn.gameObject.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 3)
		{
//			btn1Touch.SetActive(false);
//			midTootPnl.SetActive(false);
//			botTootPnl.SetActive(false);
//			btn1Touch.SetActive(false);
			menuBtn.enabled = true;
			rulesBtn.enabled = true;
			GameManager.manager.SetPunchToot();
			gameObject.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 4)
		{
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
