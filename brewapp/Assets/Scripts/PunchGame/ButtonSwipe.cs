using UnityEngine;
using System.Collections;

public class ButtonSwipe : MonoBehaviour
{
	private GameObject focusObj = null;

	void Update()
	{
		if(Input.touchCount > 0)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
//				Debug.Log("began");
			}

			if(Input.GetTouch(0).phase == TouchPhase.Moved)
			{
//				Debug.Log("moved");
				focusObj = null;
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit = new RaycastHit();

				if(Physics.Raycast(ray, out hit, 10, 0))
				{
					focusObj = hit.transform.gameObject;
				}

				if(focusObj == this.gameObject)
				{
					Debug.Log("hit");
				}
			}

			if( Input.GetTouch(0).phase == TouchPhase.Ended)
			{
//				Debug.Log("ended");
			}
		}
	}

}
