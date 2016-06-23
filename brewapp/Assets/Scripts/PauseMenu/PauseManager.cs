using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour 
{		
	public static PauseManager pauseScript;
	public GameObject loadingImg;
    public Slider tint;
    public Slider vol;
	public Text stopPractice;
	public Text stopMiniGame;
	public bool isPaused = false;

	private void Awake()
	{
		pauseScript = this;
	}
    
    private void Start()
    {
		tint.value = GameManager.manager.Tint;
		vol.value = GameManager.manager.Volume;
    }

    /// <summary>
    /// Changes the tint when the slider is changed
    /// </summary>
    public void ScreenTint(Slider newTint)
    {
		if(GameManager.manager)
			GameManager.manager.TintScreen(newTint.value);
    }

    /// <summary>
    /// Changes the volume when the slider is changed
    /// </summary>
    public void FXVolume(Slider newVol)
    {
		if(GameManager.manager)
			GameManager.manager.VolumeLvl(newVol.value);
    }

	public void OptionsEdit()
	{
		GameManager.manager.SaveOptions();
	}

	// Pauses and unpauses the game
	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		isPaused = !isPaused;
	}

	// Quits the application
	public void Quit()
	{
		#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
		#else 
		Application.Quit();
		#endif
	}

	// Restarts the current level
	public void ReloadLevel()
	{
		Time.timeScale = 1;
		loadingImg.SetActive (true);
		GameManager.manager.SetLoadBackup ();
		GameManager.manager.Load ();
		GameManager.manager.NextScene (Application.loadedLevelName);
	}

	// Returns to the BeerToss scene without finishing the mini game
	public void BarReturn()
	{
		Time.timeScale = 1;
		loadingImg.SetActive(true);
		GameManager.manager.NextScene("BeerToss");
	}

	// Goes back to the mini game from its practice scene
	public void QuitPractice()
	{
		Pause ();
		loadingImg.SetActive(true);

		if(Application.loadedLevelName == "FlipPractice")
		{
			GameManager.manager.NextScene("FlipCup");
		}
		else if(Application.loadedLevelName == "PongPractice")
		{
			GameManager.manager.NextScene("BeerPong");
		}
		else if(Application.loadedLevelName == "PunchPractice")
		{
			GameManager.manager.NextScene("Level01_Punch");
		}
		else
		{
			Debug.Log("Error: PauseManager.cs -> QuitPractice() Buttton failed to load scene");
		}
	}

	// Unpauses the game and enables the minigame without practicing
	public void StartGame()
	{
//		GameManager.manager.SetLvlDiffBackup(0);
		Pause();
	}

	// Takes you to the practice game from the main game
	public void Practice()
	{
		Pause ();
		loadingImg.SetActive(true);
		GameManager.manager.SetLoadBackup ();

		if(Application.loadedLevelName == "FlipCup")
		{
			GameManager.manager.NextScene("FlipPractice");
		}
		else if(Application.loadedLevelName == "BeerPong")
		{
			GameManager.manager.NextScene("PongPractice");
		}
		else if(Application.loadedLevelName == "Level01_Punch")
		{
			GameManager.manager.NextScene("PunchPractice");
		}
	}
}