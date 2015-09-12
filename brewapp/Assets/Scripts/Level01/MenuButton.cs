using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour 
{
	public Canvas menuCanvas;

	public void MenuToggle()
	{
		menuCanvas.gameObject.SetActive (!menuCanvas.gameObject.activeInHierarchy);
	}
}
