using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndWager : MonoBehaviour
{
	private bool dblNothin = false;
	private double myTotal = 0;
	private float myBetAmt = 0;
	private float payout = 0;
	private float animSpeed = 0;
	private int winLoseDraw = 2;

	public static EndWager endWagerScript;
	public GameObject endWagerPnl;
	public GameObject dblNillPnl;
	public GameObject loadingImg;
	public GameObject confirmBtn;
	public Text totalTxt;
	public Text betTxt;
	public Text payoutTxt;
	public Text oddsTxt;

	private void Awake()
	{
		endWagerScript = this;
	}

	// Shows the end wager panel and trigers the correct animation
	private IEnumerator EndWagerActive()
	{
		if(winLoseDraw == 0)
		{
			endWagerPnl.SetActive (true);
			yield return new WaitForSeconds (.5f);
			endWagerPnl.GetComponent<Animator>().SetBool("win", true);
			yield return new WaitForSeconds(2);
			endWagerPnl.GetComponent<Animator>().SetBool("win3", true);
			yield return new WaitForSeconds(.5f);

			float speed = Mathf.Pow(10, animSpeed);
			Debug.Log("winner " + speed);
			while(payout >= speed)
			{
				payout -= speed;
				myTotal += speed;

				payoutTxt.text = string.Format ("${0:F2}", payout);
				totalTxt.text = string.Format ("${0:F2}", myTotal);

				yield return new WaitForSeconds(.1f);
			}
			if(payout != 0)
			{
				myTotal += payout;
				payout -= payout;
				payoutTxt.text = string.Format ("${0:F2}", payout);
				totalTxt.text = string.Format ("${0:F2}", myTotal);
				yield return new WaitForSeconds(.1f);
			}

			yield return new WaitForSeconds(2);
			EndLevel();
		}
		else if(winLoseDraw == 1)
		{
			endWagerPnl.SetActive (true);
			yield return new WaitForSeconds (.5f);
			endWagerPnl.GetComponent<Animator>().SetBool("lose", true);
			yield return new WaitForSeconds(4);

			if(!dblNothin && myTotal >= myBetAmt)
			{
				dblNillPnl.SetActive(true);
				endWagerPnl.SetActive(false);
			}
			else
				EndLevel();
		}
		else
		{
			dblNillPnl.SetActive(true);
		}
	}

	// Calls the next scene
	public void EndLevel()
	{
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}

	// Sets all of the end wager panel values, saves the game state and starts the animation sequence
	public void InitEndWager(int winState, float betAmt, double myTtl, int odds)
	{
		int myOdds = odds;
		winLoseDraw = winState;

		if(GameManager.manager.GetDblNothing())
		{
			myBetAmt = GameManager.manager.GetMyBetAmt();
			payout = myBetAmt;
			myTotal = GameManager.manager.GetTotal();
			myOdds = GameManager.manager.GetLvlDiffBackup();
		}
		else 
		{
			myBetAmt = betAmt;
			payout = myBetAmt + myBetAmt * odds;
			myTotal = myTtl;
		}
		animSpeed = Mathf.Floor (Mathf.Log10 (payout)) - 1;
		if(animSpeed < 0)
			animSpeed = 0;
		oddsTxt.text = string.Format ("{0}:1", myOdds);
		betTxt.text = string.Format ("${0}", myBetAmt);
		payoutTxt.text = string.Format ("${0:F2}", payout);
		totalTxt.text = string.Format ("${0:F2}", myTotal);

		double newTotal = myTotal;
		if (winState == 0)
			newTotal = newTotal + payout;
		else if(winState == 2)
			newTotal += myBetAmt;

		if(GameManager.manager.GetDblNothing())
		{
			dblNothin = true;
			GameManager.manager.SetDblNothing(false);
		}
		GameManager.manager.SetLvlDifficulty (0);
		GameManager.manager.SetTotal (newTotal);
		GameManager.manager.Save (GameManager.manager.currProfileName);
		StartCoroutine ("EndWagerActive");
	}

	// Initiates the "double or nothing" scene
	public void DblNothinYesBtn(bool yesOrNo = true)
	{
		dblNothin = yesOrNo;
		confirmBtn.SetActive (true);
	}

	// Confirm players desire to play "double or nothing" or not
	public void ConfirmBtn()
	{
		if(dblNothin)
		{
			GameManager.manager.SetDblNothing (true);
			GameManager.manager.SetTotal (myTotal - myBetAmt);
			GameManager.manager.SetMyBetAmt (myBetAmt * 2);
			myBetAmt += myBetAmt;
			
			loadingImg.SetActive (true);
			GameManager.manager.NextScene (Application.loadedLevelName);
		}
		else
		{
			EndLevel();
		}
	}
}
