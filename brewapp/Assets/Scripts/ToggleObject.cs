using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleObject : MonoBehaviour 
{
	public void ToggleObj(GameObject obj)
	{
		if(obj.tag == "Button")
		{
			obj.GetComponent<Button>().interactable = !obj.GetComponent<Button>().interactable;
		}
		else if(obj.tag == "Canvas")
		{
			//obj.SetActive (!obj.activeInHierarchy);
			obj.GetComponent<Canvas>().enabled = !obj.GetComponent<Canvas>().enabled;
		}
		else
		{
			obj.SetActive(!obj.activeInHierarchy);
		}
	}

	public void ToggleInteract(GameObject obj)
	{
		if (obj.tag == "Button")
		{
			obj.GetComponent<Button>().interactable = !obj.GetComponent<Button>().interactable;
		}
	}
}
