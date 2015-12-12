using UnityEngine;
using System.Collections;

public class FlipCup : MonoBehaviour
{
	private Quaternion flipCupRot;
	private Vector3 flipCupPos;
	private Vector2 swipeDist;      // Delta dist-> Determins distance of flipped cup
	private bool swipeActive = true;
	private bool startTimer = false;
	private float swipeTime = 0;    // Delta time-> Determins height of flipped cup
	private float timer = 0;
	private float maxHeight = .17f;  // Maximum value for swipeTime   //30
	private float minHeight = .13f;  // Minimum value for swipeTime   // 9
	private float maxDist = 212;    // Max value for ySwipe  (each unit = .033)  //212 ~ 7
	private float minDist = 90;     // Min value for ySwipe  (each unit = .033)  // 90 ~ 3
	private int timeDelta = 3;
	private int difficulty = 0;

	public GameObject theWorld;
	public GameObject cup;
	public GameObject winTrigger;
	public GameObject forcePoint;
	public GameObject flipArrow;
	public GameObject flipArrowDown;

	public int Difficulty{ get{ return difficulty; }}

	private void Start()
	{
		if(Application.loadedLevelName == "FlipPractice")  // Show the swipe images
		{
			swipeActive = true;
		}

        // Save cup start position
		flipCupPos = transform.position;
		flipCupRot = transform.rotation;
	}

	private void FixedUpdate()
	{
		if(transform.position.y < flipCupPos.y - 2)
		{
			resetTurn();
		}

		if(difficulty == 0)
		{
			difficulty = GameManager.manager.GetDifficulty();

			if(difficulty != 0)
			{
				SetMyDiff();
			}
		}

		// Allows swipe
		if(swipeActive)
		{
//			flipArrow.SetActive(true);
			FlipToot.flipTootScript.CheckContinue(3);
			if(Application.loadedLevelName == "FlipPractice")
			{
				flipArrowDown.SetActive(false);
			}
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			FlipSwipe ();
		}
		else 
		{
			// Allows touch reset cup
			SwipeReset();
		}
        
        // When cup is flipped timer starts
		if(startTimer)
		{
			if(Time.time > timer)
			{
				// Checks for a successful flip
				if(CheckSuccess())
				{
					GameObject newobj = Instantiate(cup, transform.position, transform.rotation) as GameObject;
					newobj.transform.parent = theWorld.transform;

					// Success! move to the next cup position
					FlipCupManager.managerScript.NextCupReset();
				}
			
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; 
			
				resetTurn(); // Reset to beginning position 
			} 
		}
	}

	private void FlipCheckCup()
	{
		if(CheckSuccess())
		{
			GameObject newobj = Instantiate(cup, transform.position, transform.rotation) as GameObject;
			newobj.transform.parent = theWorld.transform;

			// Success! move to the next cup position
			FlipCupManager.managerScript.NextCupReset();
		}

		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; 
		resetTurn(); // Reset to beginning position 
	}

	// Sets the difficulty for this level
	private void SetMyDiff()
	{
		float heightDif = difficulty * .01f;
		float distDif = difficulty * 18;

		minDist = minDist - distDif;
		maxDist = maxDist + distDif;
		minHeight = minHeight - heightDif;
		maxHeight = maxHeight + heightDif;

//		Debug.Log(minDist + "  minDist + maxDist  " + maxDist);
//		Debug.Log(minHeight + "  minH + maxH  " + maxHeight);
	}

	// Allows you to swipe down on the screen to reset the cup to its starting flip position
	private void  SwipeReset()
	{
		if(Input.touchCount > 0)
		{
			if(Input.GetTouch (0).phase == TouchPhase.Began)
			{
				swipeDist = swipeDist * 0;
				swipeDist = Input.GetTouch (0).position;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				swipeDist = Input.GetTouch (0).position - swipeDist;
				
				if(swipeDist.y < 10)
				{
					swipeDist = swipeDist * 0;
					startTimer = false;
					resetTurn();
				}
			}
		}
	}

    // Reset cup position to start
	void resetTurn()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; 
		gameObject.SetActive(false);
		transform.rotation = flipCupRot;
		transform.position = flipCupPos;
		if(FlipCupManager.managerScript.GetMyCupIndex < FlipCupManager.managerScript.GetNumCups)
			gameObject.SetActive(true);
		startTimer = false;
		swipeActive = true;
	}

    // Checks if you landed the cup correctly
	bool CheckSuccess()
	{
		if(winTrigger.GetComponent<WinTrigger>().collideName == "TableCollider" )
		{
			return true;
		}

		return false;
	}

    // Calculates distance and time of swipe
	void FlipSwipe()
	{
		if(Input.touchCount > 0)
		{
			if(Input.GetTouch (0).phase == TouchPhase.Began)
			{
				swipeTime = Time.time;
				swipeDist = Input.GetTouch (0).position;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				swipeTime = (Time.time - swipeTime);
				swipeDist = Input.GetTouch (0).position - swipeDist;

				// Activates the timer for the cup to automatically reset
				startTimer = true;
				timer = Time.time + timeDelta;

				// Determines if there's a good swipe or just bump the cup
				if(swipeActive && swipeDist.y > 30 && swipeTime < .5f)	
				{
					GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

					swipeActive = false;
					ApplyForce(swipeDist);
				}
				else
				{
					Bump();
				}
			}
		}
	}

	// When there's a "bad" flip, bump the cup to show the game is still responsive
	private void Bump()
	{
		GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, 1.5f, 0), forcePoint.transform.position);
	}

    // Applies the appropriate amount of force and direction to the cup
	public void ApplyForce(Vector2 dist)
	{
		float ySwipe = dist.y; // Delta dist y dir

        // If the time length of the swipe is > or < the max or min values, set them to the max or min values respectively 
        if (swipeTime > maxHeight)
            swipeTime = maxHeight;
        else if (swipeTime < minHeight)
            swipeTime = minHeight;

        // If the distance length of the swipe is > or < the max or min values, set them to the max or min values respectively 
        if (ySwipe < minDist)
			ySwipe = minDist;  
		else if(ySwipe > maxDist)
			ySwipe = maxDist;

        // Calculates the total force from time and distance of swipe
        //Debug.Log(ySwipe + " dist : time " + swipeTime + " ratio " + ySwipe/(swipeTime * 100));
        float forceDist = (ySwipe * .035f);
        float forceHeight = ((ySwipe / (swipeTime * 100)) * 2f) - swipeTime * 50f;
        //float forceHeight = 15 - (swipeTime * 100 - 15) + Mathf.CeilToInt(ySwipe / swipeTime * .001f) * 2;

        Vector3 force = new Vector3(forceDist, forceHeight, 0);

        // Apply force to cup then start game timer
        GetComponent<Rigidbody>().AddForceAtPosition(force, forcePoint.transform.position);

		timer = Time.time + timeDelta;
	}
}
