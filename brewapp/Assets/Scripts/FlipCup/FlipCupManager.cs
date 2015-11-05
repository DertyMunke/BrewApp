using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FlipCupManager : MonoBehaviour 
{
	private GameObject[] myCups = new GameObject[7];
	private GameObject[] hisCups = new GameObject[7];
	private Animator foxAnims;
	private bool heDone;
	private bool meDone;
	private bool pickUpCup = true;
	private bool pickUpDone = false;
	private bool putDownCup = false;
	private bool putDownDone = false;
	private bool startHisGame = false;
	private float foxAnimSpacing = 1; // The amount of time between flip cup anims 
	private int winState = 2; // 0 = win, 1 = lose, 2 = draw
	private int myCupIndex = 0;
	private int hisCupIndex = 0;

	public static FlipCupManager managerScript;
	public GameObject fox;
	public GameObject foxHand;
	public GameObject foxCup;
	public GameObject loadingImg;
	public GameObject theWorld;
	public GameObject camFlipView;
	public GameObject camMain;
	public GameObject camPickUP;
	public GameObject camPutDown;
	public GameObject animCup;
	public GameObject myCupSpawn;
	public GameObject hisCupSpawn;
	public GameObject myCup;
	public GameObject hisCup;
	public GameObject flipCup;
	public GameObject chugButton;
	public GameObject wagerPnl;
	public Image[] myCupIms = new Image[6];
	public Image[] hisCupIms = new Image[6];
	public Image[] myDoneCheck = new Image[6];
	public Image[] hisDoneCheck = new Image[6];
	public Text totalTxt;
	public Text wagerTxt;
	public Text oddsTxt;
	public Text gameOverTxt;
	public Slider beerLvl;
	public Quaternion chugCamRot;
	public Vector3 chugCamPos;
	public Vector3 chugCupPos;
	public float tiltAngle = 30;
	public int numCups = 0;

	public int GetMyCupIndex { get { return myCupIndex; } }
	public int GetNumCups { get { return numCups; } }

	/// <summary>
	/// Initializations
	/// </summary>
	private void Start()
	{
		managerScript = this;
		LevelSetup ();
		chugCamPos = camMain.transform.position;
		chugCamRot = camMain.transform.rotation;
		chugCupPos = animCup.transform.position;

		foxAnims = fox.GetComponent<Animator> ();
	}

	private void FixedUpdate()
	{
//		if(startHisGame)
//			HisGame ();  //Runs the AI game
	
		// Checks for the end of the put cup down animation once it starts
		if(putDownCup || putDownDone)
		{
			PutCupDown();
		}
	}

	// Initializes the level variables and objects
	private void LevelSetup()
	{
		GameManager.manager.SetFlipMsg (false);
		int flipCupLevel = GameManager.manager.flipCupLevel;

		if(flipCupLevel <= 3)
		{
			numCups = flipCupLevel + 3; 
		}
		else
			numCups = 6;

		// Use the same difficulty and bet amounts in case of double or nothing 
		if(GameManager.manager.GetDblNothing())
		{
			numCups = 1;
			wagerPnl.SetActive(false);
			oddsTxt.text = string.Format ("{0}:1", GameManager.manager.GetDifficulty ());
			wagerTxt.text = string.Format("{0:F2}", GameManager.manager.GetMyBetAmt ());
			totalTxt.text = string.Format ("{0:F2}", GameManager.manager.GetTotal ());
		}

		// Determines the number of cups for each player, dependant on the difficulty level
		for(int i = 0; i < numCups; i++)
		{
			Vector3 myPos = new Vector3(myCupSpawn.transform.position.x, myCupSpawn.transform.position.y, 
			                            myCupSpawn.transform.position.z - (i*3));
			Vector3 hisPos = new Vector3(hisCupSpawn.transform.position.x, hisCupSpawn.transform.position.y, 
			                             hisCupSpawn.transform.position.z - (i*3));
			GameObject cup1 = (GameObject)(Instantiate(myCup, myPos, Quaternion.Euler(270, 90, -90)));
			GameObject cup2 = (GameObject)(Instantiate(hisCup, hisPos, Quaternion.Euler(270, 270, -90)));
			cup1.transform.parent = theWorld.transform;
			cup2.transform.parent = theWorld.transform;
			myCups[i] = cup1;
			hisCups[i] = cup2;
			myCupIms[i].enabled = true;
			hisCupIms[i].enabled = true;
		}
		myCups [myCupIndex].SetActive (false);
	}

	#region FoxAI

	/// <summary>
	/// Fox AI behavior
	/// </summary>
	private IEnumerator FoxAI()
	{
        Debug.Log("fox");
        foxAnims.SetBool("FlipCupActive", true);
		int diff = flipCup.GetComponent<FlipCup> ().Difficulty;
		while (hisCupIndex < numCups) 
		{
			if (myCupIndex >= numCups) 
			{
				StartCoroutine (GameOver (true));
				yield break;
			}

			foxAnims.SetTrigger ("PickUpCup");
			yield return new WaitForSeconds (6.5f); // Time til put down
			hisCup.SetActive (true);
			foxCup.SetActive(false);

			do
			{
				hisCup.GetComponent<HisCup>().SetFlipCupPos(myCupIndex);
				if (myCupIndex >= numCups) 
				{
					StartCoroutine (GameOver (true));
					yield break;
				}

				yield return new WaitForSeconds (foxAnimSpacing);
				if (myCupIndex >= numCups) 
				{
					StartCoroutine (GameOver (true));
					yield break;
				}

				foxAnims.SetTrigger ("FlipCup");
				yield return new WaitForSeconds (1.6f); // Time til flip
				if (myCupIndex >= numCups) 
				{
					StartCoroutine (GameOver (true));
					yield break;
				}

				hisCup.GetComponent<HisCup> ().ApplyForce (new Vector2 (Random.Range(-10 + 5 - diff, -10 - 5 + diff), 
				                                                        Random.Range(20 -5 + diff, 20 + 5 - diff)));
				if (myCupIndex >= numCups) 
				{
					StartCoroutine (GameOver (true));
					yield break;
				}
				yield return new WaitForSeconds (3.5f);
			}
			while(!hisCup.GetComponent<HisCup> ().CheckSuccess ());

			if (hisCup.GetComponent<HisCup> ().CheckSuccess ()) 
			{
				hisCupIndex++;

				yield return new WaitForSeconds (foxAnimSpacing);

				if (myCupIndex >= numCups) 
				{
					StartCoroutine (GameOver (true));
					yield break;
				}
				else if(hisCupIndex >= numCups)
				{
					StartCoroutine(GameOver(false));
					break;
				}

				Vector3 targetPos = new Vector3 (fox.transform.position.x, fox.transform.position.y, fox.transform.position.z - 4);
				while (fox.transform.position.z > targetPos.z + 1) 
				{
					if (myCupIndex >= numCups) 
					{
						StartCoroutine (GameOver (true));
						yield break;
					}
					fox.transform.position = Vector3.Lerp (fox.transform.position, targetPos, foxAnimSpacing * Time.deltaTime);
					yield return null;
				}
				foxHand.SetActive(true);
			}
		}
		yield return null;
	}

	public void GetFoxCurrCup(GameObject currCup)
	{
		hisCup = currCup;
		foxHand.SetActive (false);
		foxCup.SetActive (true);
	}
	
	#endregion

	// Pushing the chug button allows you to tilt the cup back to chug the beer
	public void ChugButton()
	{
		if(!startHisGame)
		{
			FlipToot.flipTootScript.CheckContinue(2);
			StartCoroutine(FoxAI());
			startHisGame = true;
		}

		if(myCups[myCupIndex] != null)
		{
			if(PickUpBeer())
			{
				beerLvl.value --;

				if(beerLvl.value > 0)
				{
					camMain.transform.Rotate(Vector3.left * 3);
				}
				else
				{
					putDownCup = true;
				}
			}
			else
			{
				camPickUP.GetComponent<Animator> ().SetBool ("startAnim", true);
			}
		}
		else
			StartCoroutine(GameOver(false));
	}

	// After a succesfull flip, moves you to the next cup
	public void NextCupReset()
	{
		myDoneCheck [myCupIndex].enabled = true;
		myCupIndex++;
		if(myCupIndex < numCups)
		{
			camMain.transform.position = chugCamPos;
			camMain.transform.rotation = chugCamRot;
			beerLvl.value = beerLvl.maxValue;
			chugButton.SetActive(true);
			pickUpCup = true;
			pickUpDone = false;
			putDownCup = false;
			putDownDone = false;
			camPickUP.GetComponent<Animator> ().SetBool ("startAnim", false);
			camPutDown.GetComponent<Animator>().enabled = true;
			camPickUP.GetComponent<Animator> ().enabled = true;
			myCups[myCupIndex].SetActive(false);
			camPickUP.SetActive (true);
			camFlipView.SetActive(false);
			Vector3 worldPos = theWorld.transform.position;
			theWorld.transform.position = new Vector3 (worldPos.x, worldPos.y, worldPos.z + 3);
		}
	}

	// Triggers the animation that puts the cup down to prepare for the flip
	private void PutCupDown()
	{
		if(putDownCup)
		{
			putDownCup = false;
			putDownDone = true;
			chugButton.SetActive(false);
			camPutDown.SetActive(true);
			camMain.SetActive(false);
		}
		else if(putDownDone)
		{
			if(camPutDown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PutDownDone"))
			{
				putDownDone = false;
				camPutDown.GetComponent<Animator>().enabled = false;
				camFlipView.SetActive(true);
				camPutDown.SetActive(false);
				chugButton.SetActive(false);
			}
		}
	}

	// Picks up the beer to drink after the chug button is hit
	private bool PickUpBeer()
	{
		if(pickUpCup)
		{
			pickUpCup = false;
			camPickUP.GetComponent<Animator>().enabled = true;

			return false;
		}
		else if(!pickUpDone)
		{
			if(camPickUP.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PickUpDone"))
			{
				pickUpDone = true;
				camPickUP.GetComponent<Animator>().enabled = false;
				camMain.SetActive(true);
				camPickUP.SetActive(false);
			}
			return false;
		}
		return true;
	}

	// Called on game over: triggers the end wager and double or nothing
	private IEnumerator GameOver(bool youWin)
	{
		if (winState == 2) 
		{
			if (youWin) 
			{
				winState = 0;
				foxAnims.SetTrigger ("BadFlip");
				yield return new WaitForSeconds(2.25f);

				GameManager.manager.IncReRoll ();
				if (GameManager.manager.GetFlipLvl () == GameManager.manager.GetDifficulty () - 1)
					GameManager.manager.flipCupLevel ++;
			} 
			else 
			{
				winState = 1;
				foxAnims.SetTrigger("GoodFlip");
				yield return new WaitForSeconds(2.667f);

				if (chugButton.activeInHierarchy == true)
					chugButton.GetComponentInChildren<Button> ().enabled = false;
			}

			GameManager.manager.flipSliderVal = 0;
			Wager.wagerScript.InitEndWager (winState);
		}
	}

	// Starts end level animation and calls the BeerToss scene
	private void EndLevel()
	{
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}

	// Sets the final values for the top banner -> Used with start game button
	public void TopBannerValue()
	{
		oddsTxt.text = string.Format ("{0}:1", GameManager.manager.GetDifficulty ());
		wagerTxt.text = string.Format("{0:F2}", GameManager.manager.GetMyBetAmt ());
		totalTxt.text = string.Format ("{0:F2}", GameManager.manager.GetTotal ());
	}
	
}
