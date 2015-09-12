using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour 
{	
	public GameObject loadingImg;

	void Awake () 
	{
		loadingImg.SetActive (true);
		GameManager.manager.NextScene ("BeerToss");
	}
}
