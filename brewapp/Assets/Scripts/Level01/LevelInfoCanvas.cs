using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoCanvas : MonoBehaviour 
{
	public static LevelInfoCanvas levInfoScript;
	public GameObject loadingImg;
	public GameObject msgsCanvas;
	public Image bearMsg;
	public Image hareMsg;
	public Image foxMsg;
	public bool pong = false;
	public bool flip = false;
	public bool punch = false;

	private void Awake()
	{
		levInfoScript = this;
	}

	public void PongSelect()
	{
		hareMsg.enabled = true;
		pong = true;
		flip = false;
		punch = false;
	}

	public void FlipSelect()
	{
		foxMsg.enabled = true;
		pong = false;
		flip = true;
		punch = false;
	}

	public void PunchSelect()
	{
		bearMsg.enabled = true;
		pong = false;
		flip = false;
		punch = true;
	}

	public void ContinueBtn()
	{
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}

	public void MaybeLaterButton()
	{
		if(pong)
		{
			hareMsg.enabled = false;
		}
		else if(flip)
		{
			foxMsg.enabled = false;
		}
		else if(punch)
		{
			bearMsg.enabled = false;
		}

		pong = false;
		flip = false;
		punch = false;

	}

	public void AcceptChallengeButton()
	{
		if(pong)
		{
			loadingImg.SetActive (true);
			GameManager.manager.NextScene ("BeerPong");
		}
		else if(flip)
		{
			loadingImg.SetActive (true);
			GameManager.manager.NextScene ("FlipCup");
		}
		else if(punch)
		{
			loadingImg.SetActive (true);
			GameManager.manager.NextScene ("Level01_Punch");
		}
	}
}
