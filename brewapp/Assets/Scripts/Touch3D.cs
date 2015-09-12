using UnityEngine;
using System.Collections;

public class Touch3D : MonoBehaviour 
{
	public GameObject focusObj = null;
	public static int currTouch;

	public void TouchController () 
	{
		if(Input.touchCount > 0)
		{
			for(int i = 0; i < Input.touchCount; i++)
			{
				currTouch = i;
				
				if(Input.GetTouch(i).phase == TouchPhase.Began)
				{
					focusObj = null;
					Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
					RaycastHit hit = new RaycastHit();
					
					if(Physics.Raycast(ray, out hit, 1000))
					{
						focusObj = hit.transform.gameObject;
					}
					
					if(focusObj == this.gameObject)
					{
						OnTouchBegan();
					}
				}
				if(Input.GetTouch(i).phase == TouchPhase.Moved)
				{
					if(focusObj == this.gameObject)
					{
						OnTouchMoved();
					}
				}
				
//				if(Input.GetTouch(i).phase == TouchPhase.Stationary)
//				{
//					if(focusObj == this.gameObject)
//					{
//						OnTouchStationary();
//					}
//				}
				
				if(Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					if(focusObj == this.gameObject)
					{
						OnTouchEnded();
					}
				}
				
			}
		}
	}
	public virtual void OnTouchBegan(){}
	public virtual void OnTouchMoved(){}
	//public virtual void OnTouchStationary(){}
	public virtual void OnTouchEnded(){}
}
