using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PracticeDifPnl : MonoBehaviour
{	
	private void Start()
	{
		PauseManager.pauseScript.Pause();
	}

	public void DifBtn(GameObject difBtn)
	{
		int dif = 0;
		if(!int.TryParse(difBtn.name, out dif))
		{
			Debug.Log("Error: Failed Parse to int -> Practice Difficulty Button");
		}
		PauseManager.pauseScript.Pause();
		GameManager.manager.SetLvlDifficulty(dif);
	}
}
