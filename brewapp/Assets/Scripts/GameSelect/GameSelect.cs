using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameSelect : MonoBehaviour {

    public static GameSelect selectScript;
    public GameObject loadingIcon;
    public Button tossBtn;
    public Button pongBtn;
    public Button flipBtn;
    public Button punchBtn;
    public Text totalTxt;

    private void Start()
    {
        selectScript = this;
    }

    /// <summary>
    /// Enables the available game buttons
    /// </summary>
    public void SetGameBtns()
    {
        totalTxt.text = string.Format("{0:F2}", GameManager.manager.baseTotal);

        if (GameManager.manager.total > 0)
        {
            if(GameManager.manager.barTossLevel > 3)
                punchBtn.interactable = true;

            if (GameManager.manager.barTossLevel > 2)
                flipBtn.interactable = true;

            if (GameManager.manager.barTossLevel > 1)
                pongBtn.interactable = true;
        }
    }

    /// <summary>
    /// Loads the currently selected profile and begins the game
    /// </summary>
    public void PlayBeerToss()
    {
        loadingIcon.SetActive(true);
        GameManager.manager.Load();
        GameManager.manager.NextScene("BeerToss");
    }

    /// <summary>
    /// Loads the currently selected profile and begins the game
    /// </summary>
    public void PlayBeerPong()
    {
        loadingIcon.SetActive(true);
        GameManager.manager.Load();
        GameManager.manager.NextScene("BeerPong");
    }

    /// <summary>
    /// Loads the currently selected profile and begins the game
    /// </summary>
    public void PlayFlipCup()
    {
        loadingIcon.SetActive(true);
        GameManager.manager.Load();
        GameManager.manager.NextScene("FlipCup");
    }

    /// <summary>
    /// Loads the currently selected profile and begins the game
    /// </summary>
    public void PlayPunch()
    {
        loadingIcon.SetActive(true);
        GameManager.manager.Load();
        GameManager.manager.NextScene("Level01_Punch");
    }

    /// <summary>
    /// Loads the start scene
    /// </summary>
    public void LoadMainMenu()
    {
        loadingIcon.SetActive(true);
        GameManager.manager.Load();
        GameManager.manager.NextScene("StartMenu");
    }
}
