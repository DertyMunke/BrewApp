using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwipeBall : MonoBehaviour
{
	public Text swipeBallText;

	private Vector2 deltaTouch = new Vector2(0, 0);
	private Vector2 swipeAngle = new Vector2(0, 0);
	private bool ballBounced = false;
	private float swipeDelay = .5f;
	private float swipeTimer = 0;
	
	private void FixedUpdate()
	{
		if((swipeTimer < Time.time && swipeBallText.enabled) || !renderer.enabled)
		{
			swipeBallText.enabled = false;
		}

		EnableBallSwipe ();
	}

	// Allows you to swipe the ball after it bounces, when thrown by the computer character
	private void EnableBallSwipe()
	{
		if(ballBounced && renderer.enabled)
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				
				if(touch.phase == TouchPhase.Began)
				{
					deltaTouch = touch.position;
				}
				
				if(touch.phase == TouchPhase.Moved)
				{
					Vector3 ballPos = Camera.main.WorldToScreenPoint(transform.position);
					Vector3 swipePos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, ballPos.z);
					
					if(swipePos.x > ballPos.x - 150 && swipePos.x < ballPos.x  + 150 && swipePos.y > ballPos.y - 150 && swipePos.y < ballPos.y + 150)
					{
						ballBounced = false;
						deltaTouch = touch.position - deltaTouch;
						Swipe ();
						deltaTouch = new Vector2(0,0);
					}
				}
			}
		}
	}

	// Adds a force to the ball to simulate swatting the ball
	private void Swipe()
	{
		rigidbody.AddForce (deltaTouch.x, deltaTouch.y, 10);
	}

	// On collision, checks to see if "swipe ball" text can be displayed
	private void OnCollisionEnter(Collision other)
	{
		Invoke ("SwipeTextEnabled", .1f); 
	}

	// Helper funcion for OnCollisionEnter: Allows a pause using the Invoke() method
	private void SwipeTextEnabled()
	{
		if(!PingPongBall.ppBallScript.myTurn && renderer.enabled)
		{
			ballBounced = true;
			swipeBallText.enabled = true;
			swipeTimer = Time.time + swipeDelay;
		}
		else
		{
			ballBounced = false;
		}
	}
}


