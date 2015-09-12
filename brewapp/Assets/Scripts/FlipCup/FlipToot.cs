using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlipToot : MonoBehaviour 
{
	private int tootStage = 0;

	public static FlipToot flipTootScript;
	public GameObject topTootPnl;
	public GameObject midTootPnl;
	public GameObject botTootPnl;
	public GameObject flipBtnPnl;
	public GameObject flipArrowPnl;
	public GameObject chugTootTouch;
	public Canvas tootCnvs;
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
		flipTootScript = this;
	}

	public void StartFlipToot()
	{
		if(GameManager.manager.GetFlipToot())
		{
			tootCnvs.enabled = true;
			flipBtnPnl.SetActive(false);
			menuBtn.interactable = false;
			rulesBtn.interactable = false;
		}
	}

	// Tutorial walkthrough for flip cup
	public void ContinueBtn()
	{
		if (tootStage == 0) 
		{
			topTxt.text = "You want to chug your beers and flip the cups on their tops, before your opponent.";	
			tootStage++;
		}
		else if(tootStage == 1)
		{
			topTxt.text = "Push the chug button to pick up your cup, then tap it to drink the beer.";
			topInstTxt.text = "Tap the button to drink...";
			flipBtnPnl.SetActive(true);
			chugTootTouch.SetActive(true);
			contBtn.gameObject.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 2)
		{
			chugTootTouch.SetActive(false);
			topTootPnl.SetActive(false);
			tootStage++;
		}
		else if(tootStage == 3)
		{
			Time.timeScale = 0;
			topTxt.text = "Swipe up to flip the cup and swipe down to reset it to the start position.";
			topInstTxt.text = "Touch to continue...";
			flipArrowPnl.SetActive(true);
			topTootPnl.SetActive(true);
			contBtn.gameObject.SetActive(true);
			tootStage++;
		}
		else if(tootStage == 4)
		{
			Time.timeScale = 1;
			flipArrowPnl.SetActive(false);
			topTootPnl.SetActive(false);
			contBtn.gameObject.SetActive(false);
			GameManager.manager.SetFlipToot();
			menuBtn.interactable = true;
			rulesBtn.interactable = true;
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
