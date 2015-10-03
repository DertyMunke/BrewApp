using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour 
{
	private FileStream file = null;
	private string nextLevel = "";
	private bool loadBackup = false;
	private bool dblNothing = false;
	private bool gotPongMsg = false;
	private bool gotFlipMsg = false;
	private bool gotPunchMsg = false;
	private bool pongToot = true;
	private bool pongRackToot = true;
	private bool flipToot = false;
	private bool punchToot = true;
	private bool restartLvl = false;
	private float highTips = 0;
	private float myBet = 0;
	private int lvlDifficulty = 0;  // 1-5
	private int lvlDiffBackup = 0;  // Saves the difficulty level when doing practice or "double or nothing"
	private int highThrown = 0;
	private int numReRolls = 0;
	private int reRollsBackup = 0;

	public static GameManager manager;
	public String currProfileName = "";
	public bool barTossToot = true;
	public double total = 0;
	public float beerTossXP = 0;
	public float pongSliderValue = 0;
	public float xpSliderValue = 0;
	public float flipSliderVal = 0;
	public float punchSliderVal = 0;
	public int barTossLevel = 1;
	public int flipCupLevel = 1;
	public int beerPongLevel = 1;
	public int punchLevel = 1;
	
	private void Awake()
	{
		if(manager == null)
		{
			DontDestroyOnLoad(gameObject);
			manager = this;
		}
		else if(manager != this)
		{
			Destroy(gameObject);
		}
	}

	// Loads the next level: Need unity pro to finish this
	private IEnumerator Loading()
	{
//		Save (currProfileName);  // took this out for testing, might need later

//		Debug.Log ("loading started: Level " + nextLevel);
		// Async requires unity pro
//		AsyncOperation async = Application.LoadLevelAsync ("Level01");
//		yield return async;
		yield return new WaitForSeconds(1.5f);
//		Debug.Log ("loading complete");

		Application.LoadLevel (nextLevel);
	}

	// Allows other scripts to start the loading level coroutine
	public void NextScene(String lvl)
	{
		nextLevel = lvl;
		StartCoroutine ("Loading");
	}

	// Saves the current profiles data to PlayerData class
	public void Save(string playerName)
	{
		try
		{
			BinaryFormatter bf = new BinaryFormatter ();
			file = File.Create (Application.persistentDataPath + "/" + playerName + ".dat"); 
			PlayerData data = new PlayerData();

			data.punchToot = punchToot;
			data.flipToot = flipToot;
			data.pongToot = pongToot;
			data.pongRackToot = pongRackToot;
			data.gotPongMsg = gotPongMsg;
			data.gotFlipMsg = gotFlipMsg;
			data.gotPunchMsg = gotPunchMsg;
			data.total = total;
			data.highTips = highTips;
			data.highThrown = highThrown;
			data.numReRolls = numReRolls;
			data.beerTossXP = beerTossXP;
			data.barTossLevel = barTossLevel;
			data.barTossToot = barTossToot;
			data.beerPongLevel = beerPongLevel;
			data.flipCupLevel = flipCupLevel;
			data.punchLevel = punchLevel;
			data.pongSliderValue = pongSliderValue;
			data.xpSliderValue = xpSliderValue;
			data.flipSliderVal = flipSliderVal;
			data.punchSliderVal = punchSliderVal;

			bf.Serialize (file, data);
			file.Close ();
		}
		finally
		{
			if(file != null)
				file.Close();
		}
	}

	// Loads the data from PlayerData class
	public void Load(string playerName)
	{
		if(File.Exists(Application.persistentDataPath + "/" + playerName + ".dat"))
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				file = File.Open(Application.persistentDataPath + "/" + playerName + ".dat", FileMode.Open);
				PlayerData data = (PlayerData)bf.Deserialize(file); //without cast, makes generic obj
				file.Close();
			
				punchToot = data.punchToot;
				flipToot = data.flipToot;
				pongToot = data.pongToot;
				pongRackToot = data.pongRackToot;
				gotPongMsg = data.gotPongMsg;
				gotFlipMsg = data.gotFlipMsg;
				gotPunchMsg = data.gotPunchMsg;
				total = data.total;
				highTips = data.highTips;
				highThrown = data.highThrown;
				numReRolls = data.numReRolls;
				beerTossXP = data.beerTossXP;
				barTossLevel = data.barTossLevel;
				barTossToot = data.barTossToot;
				beerPongLevel = data.beerPongLevel;
				punchLevel = data.punchLevel;
				flipCupLevel = data.flipCupLevel;
				pongSliderValue = data.pongSliderValue;
				xpSliderValue = data.xpSliderValue;
				flipSliderVal = data.flipSliderVal;
				punchSliderVal = data.punchSliderVal;
			}
			finally
			{
				if(file != null)
					file.Close();
			}

			currProfileName = playerName;
		}
	}

	// Deletes player data
	public void Delete(string playerName)
	{
		if(File.Exists(Application.persistentDataPath + "/" + playerName + ".dat"))
		{
			File.Delete(Application.persistentDataPath + "/" + playerName + ".dat");
		}
	}

	// Return currently selected difficulty level
	public int GetLvlDifficulty()
	{
		return lvlDifficulty;
	}

	// Set the current difficulty level
	public void SetLvlDifficulty(int dif)
	{
		lvlDifficulty = dif;
	}

	// Set the current difficulty level when going to practice scene
	public void SetLvlDiffBackup(int dif)
	{
		lvlDiffBackup = dif;
	}

	// Return currently selected difficulty level when coming back from practice scene
	public int GetLvlDiffBackup()
	{
		return lvlDiffBackup;
	}

	// Get the current bet amount
	public float GetMyBetAmt()
	{
		return myBet;
	}

	// Set my bet amount
	public void SetMyBetAmt(float bet)
	{
		myBet = bet;
	}

	// Set total dollars
	public void SetTotal(double tot)
	{
		total = tot;
	}

	// Get total dollars
	public double GetTotal()
	{
		return total;
	}

	// Sets the value of dblNothing: set to true when "double or nothing" is selected
	public void SetDblNothing(bool dblNoth)
	{
		dblNothing = dblNoth;
	}

	// Returns a bool that determines if the level should be setup for "double or nothing" or for a regular game
	public bool GetDblNothing()
	{
		return dblNothing;
	}

	// Set loadBackup when you go to practice scene and the difficulty level is changed
	public void SetLoadBackup(bool set = true)
	{
		loadBackup = set;
		reRollsBackup = numReRolls;
	}

	// Returns load backup: Determines if we should load the backup difficuly level
	public bool GetLoadBackup()
	{
		numReRolls = reRollsBackup;
		return loadBackup;
	}

	// Returns the beer pong level
	public int GetPongLvl()
	{
		return beerPongLevel;
	}

	// Returns the flip cup level
	public int GetFlipLvl()
	{
		return flipCupLevel;
	}

	// Returns the punch game level
	public int GetPunchLvl()
	{
		return punchLevel;
	}

	// Returns the bar toss level
	public int GetBarTossLevel()
	{
		return barTossLevel;
	}

	// Returns the players highest tips earned in 1 round
	public float GetHighTips()
	{
		return highTips;
	}

	// Sets the players highest tips earned in 1 round
	public void SetHighTips(float high)
	{
		highTips = high;
	}

	// Returns the players highest number of patrons served in 1 round
	public int GetHighThrown()
	{
		return highThrown;
	}

	// Sets the players highest number of patrons served in 1 round
	public void SetHighThrown(int high)
	{
		highThrown = high;
	}

	// Returns the number of re-rolls available
	public int GetReRolls()
	{
		return numReRolls;
	}

	// Adds a re-roll to the number of re-rolls
	public void IncReRoll()
	{
		numReRolls ++;
	}

	// Subtracts a re-roll to the number of re-rolls
	public void DecReRoll()
	{
		numReRolls --;
	}

	// Sets the the backup value to the number of re-rolls: used when restarting a level
	public void SetReRollsBackup()
	{
		reRollsBackup = numReRolls;
	}

	// Sets the number of re-rolls to the backup value: used when restarting a level
	public void GetReRollsBackup()
	{
		numReRolls = reRollsBackup;
	}

	// Returns the correct difficulty level
	public int GetDifficulty()
	{
		if(dblNothing)
		{
			return lvlDiffBackup;
		}
		return lvlDifficulty;
	}

	// Returns true if the user has recieved the beer pong challenge
	public bool GetPongMsg()
	{
		return gotPongMsg;
	}

	// Set to true when the user recieves the beer pong challenge
	public void SetPongMsg(bool set = true)
	{
		gotPongMsg = set;
	}

	// Returns true if the user has recieved the flip cup challenge
	public bool GetFlipMsg()
	{
		return gotFlipMsg;
	}
	
	// Set to true when the user recieves the flip cup challenge
	public void SetFlipMsg(bool set = true)
	{
		gotFlipMsg = set;
	}

	// Returns true if the user has recieved the punch game challenge
	public bool GetPunchMsg()
	{
		return gotPunchMsg;
	}
	
	// Set to true when the user recieves the punch game challenge
	public void SetPunchMsg(bool set = true)
	{
		gotPunchMsg = set;
	}

	// Sets the beer pong slider value
	public void SetPongRep(float rep)
	{
		pongSliderValue = rep;
	}

	// Gets the beer pong slider value
	public float GetPongRep()
	{
		return pongSliderValue;
	}

	// Sets the flip cup slider value
	public void SetFlipRep(float rep)
	{
	 	flipSliderVal = rep;
	}
	
	// Gets the flip cup slider value
	public float GetFlipRep()
	{
		return flipSliderVal;
	}

	// Sets the punch game slider value
	public void SetPunchRep(float rep)
	{
		punchSliderVal = rep;
	}
	
	// Gets the punch game slider value
	public float GetPunchRep()
	{
		return punchSliderVal;
	}

	// Returns true if the beer pong tutorial hasn't been complete
	public bool PongTootActive()
	{
		return pongToot;
	}

	// Set the value of pongToot to false after tutorial complete
	public void SetPongToot(bool active = false)
	{
		pongToot = active;
	}

	// Returns true if the re-rack button hasn't been introduced in beer pong
	public bool GetPongRackToot()
	{
		return pongRackToot;
	}

	// Sets the pongRackToot to false after the tutorial has been complete
	public void SetPongRackToot(bool active = false)
	{
		pongRackToot = active;
	}

	// Set to true when restarting level in mini game
	public void SetRestartLvl(bool restart = true)
	{
		restartLvl = restart;
	}

	// Returns true if level was restarted
	public bool GetRestartLvl()
	{
		return restartLvl;
	}

	// Sets flipToot to false once the flip cup tutorial is complete
	public void SetFlipToot(bool active = false)
	{
		flipToot = active;
	}

	// Returns true if the flip cup tutorial has not been complete
	public bool GetFlipToot()
	{
		return flipToot;
	}

	// Sets punchToot to false once the punch game tutorial is complete
	public void SetPunchToot(bool active = false)
	{
		punchToot = active;
	}
	
	// Returns true if the punch game tutorial has not been complete
	public bool GetPunchToot()
	{
		return punchToot;
	}
}

//data container that allows you to write the data to a file
[Serializable]
class PlayerData
{
	public bool barTossToot;
	public bool gotPongMsg;
	public bool gotFlipMsg;
	public bool gotPunchMsg;
	public bool pongToot;
	public bool pongRackToot;
	public bool flipToot;
	public bool punchToot;
	public Double total;
	public float beerTossXP;
	public float pongSliderValue;
	public float xpSliderValue;
	public float flipSliderVal;
	public float punchSliderVal;
	public float highTips;
	public int highThrown;
	public int barTossLevel;
	public int flipCupLevel;
	public int beerPongLevel;
	public int punchLevel;
	public int numReRolls;
}