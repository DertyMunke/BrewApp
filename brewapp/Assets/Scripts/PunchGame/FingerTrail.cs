using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FingerTrail : MonoBehaviour 
{
	private GameObject swipeTrail;
	private LineRenderer lineRenderer;
	private Vector2 deltaTouch;
	private bool goodPunchStart = false;
	private bool goodPunchEnd = false;
	private bool swipe1Active = false;
	private bool swipe2Active = false;
	private float lineSize = 0;
	private int bonusPower = 0;
	private int i = 0;
	private int deltaTouchDist = 20;

	public static FingerTrail fingerScript;
	public GameObject bag;
	public GameObject button1;
	public GameObject button2;
	public GameObject b2Path;
	public GameObject b2Ring;
	public GameObject bagPath;
	public GameObject bagRing;
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public bool tester = false; // Used to trigger things in the hiarchy when testing

	private void Awake()
	{
		fingerScript = this;
	}

	private void Start()
	{
		swipeTrail = new GameObject("Line");
		swipeTrail.AddComponent<LineRenderer>();
		lineRenderer = swipeTrail.GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive Culled"));
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.5F, 0);
		lineRenderer.SetVertexCount(0);
	}
	
	private void FixedUpdate()
	{
//		Debug.Log (bagRing.GetComponent<Image> ().rectTransform.localPosition);
		if (tester)
		{

		}

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if(touch.phase == TouchPhase.Began)
			{
				deltaTouch = touch.position;
			}

			if(touch.phase == TouchPhase.Moved)
			{
//				Debug.Log(touch.position.x + " " + Screen.width * .7f);
				lineRenderer.SetVertexCount(i+1);
				Vector3 mPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 33);
				lineRenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(mPosition));
				i++;

				PowerChecker(touch);
			}
			else if(touch.phase == TouchPhase.Stationary)
			{
				PowerChecker(touch);
			}
			
			if(touch.phase == TouchPhase.Ended)
			{
				if(swipe1Active && !button2.activeInHierarchy)
				{
					swipe1Active = false;
					b2Path.SetActive(false);
					b2Ring.SetActive(false);
					button2.SetActive(true);
				}

				lineRenderer.SetVertexCount(0);
				i = 0;
				bonusPower = 0;

				TotalPunch();
				goodPunchStart = false;
				goodPunchEnd = false;
			}
		}
	}

	// Activates the total power after the last swipe
	private void TotalPunch()
	{
		if(swipe2Active)
		{
			swipe2Active = false;
			button1.SetActive(false);
			button2.SetActive(false);
			b2Path.SetActive(false);
			b2Ring.SetActive(false);
			bagPath.SetActive(false);
			bagRing.SetActive(false);
			bag.GetComponent<Animator>().SetBool("punch", true);

			PowerMeter.pmeterScript.AddBtnPower();
		}
	}

	// Determins if the swipe is big enough to be intentional
	private bool CheckDeltaTouch(Touch touch)
	{
		Vector2 touchPos = touch.position;

		if(deltaTouch.x - touchPos.x > deltaTouchDist || deltaTouch.x - touchPos.x < -deltaTouchDist  || 
		   deltaTouch.y - touchPos.y > deltaTouchDist  || deltaTouch.y - touchPos.y < -deltaTouchDist )
		{
			return true;
		}

		return false;
	}

	// The the swipe is big enough then add the bonus power from the swipe
	private void PowerChecker (Touch touch)
	{
		bonusPower ++;

		if(CheckDeltaTouch(touch) && bagPath.activeInHierarchy)
		{
			swipe2Active = true;
			BonusSwipe(11);
		}
		
		if(CheckDeltaTouch(touch) && b2Path.activeInHierarchy)
		{
			swipe1Active = true;
			BonusSwipe(11);
		}
	}

	// Applies the bonus swipe
	private void BonusSwipe (int power)
	{
		if(bonusPower*power < 230)
		{
			if(bonusPower > 5)
			{
				PowerMeter.pmeterScript.BonusPower2 (bonusPower*power);
			}
		}
		else
		{
			PowerMeter.pmeterScript.PenaltyPower2(5);
		} 
	}

	// Allows another script to access the line size
	public float GetLineSize()
	{
		return lineSize;
	}

	// Allows another script to reset the round
	public void ResetRound(bool myTurn)
	{
		if(myTurn)
		{
			button1.SetActive(true);
		}
		PowerMeter.pmeterScript.GaugeDecayActive(true);
		bag.GetComponent<Animator>().SetBool("punch", false);
	}
}

