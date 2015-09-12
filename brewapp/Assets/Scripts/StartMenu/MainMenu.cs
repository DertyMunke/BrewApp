using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour 
{
	//private bool lookForInput = false;
	private FileStream file = null;
	private String userName;  //This is the selected profile player name
	private String defaultName;	 //This is the selected profile default name

	public Canvas startMenu;
//	public Canvas optionsMenu;
	public InputField createName;
	public Button createButton;
	public Button deleteButton;
	public Button loadButton;
	public Button playButton;
	public Button profile_1;
	public Button profile_2;
	public Button profile_3;
	public Button profile_4;
	public Button profile_5;
	public Text currText;
	public bool profile_1_active = false;
	public bool profile_2_active = false;
	public bool profile_3_active = false;
	public bool profile_4_active = false;
	public bool profile_5_active = false;
	
	//This function is for testing purposes
	private	void ResetProfilesTestingFunction()
	{
		currText.text = "Choose Profile";
		profile_1.GetComponentInChildren<Text> ().text = "Profile 1";
		profile_2.GetComponentInChildren<Text> ().text = "Profile 2";
		profile_3.GetComponentInChildren<Text> ().text = "Profile 3";
		profile_4.GetComponentInChildren<Text> ().text = "Profile 4";
		profile_5.GetComponentInChildren<Text> ().text = "Profile 5";
		profile_1_active = false;
		profile_2_active = false;
		profile_3_active = false;
		profile_4_active = false;
		profile_5_active = false;
		SaveProfiles ();
	}

	private void Awake()
	{
#if UNITY_IOS
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
	}

	private void Start()
	{

		userName = null;
		defaultName = null;

		//for testing only
		//ResetProfilesTestingFunction ();

		StartingProfiles ();
		SetStartMenuButtons ();
	}

	private void Update()
	{
		//Debug.Log (Application.persistentDataPath);
		//createName.onEndEdit.AddListener (OnSubmit);
	}

	// Submits a new profile name in the next available spot and saves the new profile 
	public void OnSubmit(Text line)
	{
		if(!profile_1_active )
		{
			profile_1.GetComponentInChildren<Text>().text = line.text;
			profile_1_active = true;
			profile_1.interactable = true;
		}
		else if(!profile_2_active)
		{
			profile_2.GetComponentInChildren<Text>().text = line.text;
			profile_2_active = true;
			profile_2.interactable = true;
		}
		else if(!profile_3_active)
		{
			profile_3.GetComponentInChildren<Text>().text = line.text;
			profile_3_active = true;
			profile_3.interactable = true;
		}
		else if(!profile_4_active)
		{
			profile_4.GetComponentInChildren<Text>().text = line.text;
			profile_4_active = true;
			profile_4.interactable = true;
		}
		else if(!profile_5_active)
		{
			profile_5.GetComponentInChildren<Text>().text = line.text;
			profile_5_active = true;
			profile_5.interactable = true;
		}
		else
		{
			createButton.interactable = false;
			return;
		}

		currText.text = line.text;
		SaveProfiles ();
		GameManager.manager.Save (line.text);
		playButton.interactable = true;
		createName.text = "";
		createName.interactable = false;
	}

	// Creates the profile account if it doesn't exist or loads the profiles if it does
	private void StartingProfiles()
	{
		if(!File.Exists(Application.persistentDataPath + "/profiles.dat"))
		{
			SaveProfiles();
		}
		else
		{
			LoadProfiles(); 
		}

	}

	// Saves all of the varibles for the currently selected profile
	private void SaveProfiles()
	{
		try
		{
			BinaryFormatter bf = new BinaryFormatter ();
			file = File.Create (Application.persistentDataPath + "/profiles.dat"); 
			ProfilesData data = new ProfilesData();
			
			data.current = currText.text;
			data.profile_1 = profile_1.GetComponentInChildren<Text>().text;
			data.profile_2 = profile_2.GetComponentInChildren<Text>().text;
			data.profile_3 = profile_3.GetComponentInChildren<Text>().text;
			data.profile_4 = profile_4.GetComponentInChildren<Text>().text;
			data.profile_5 = profile_5.GetComponentInChildren<Text>().text;
			data.profile_1_active = profile_1_active;
			data.profile_2_active = profile_2_active;
			data.profile_3_active = profile_3_active;
			data.profile_4_active = profile_4_active;
			data.profile_5_active = profile_5_active;
			
			bf.Serialize (file, data);
			file.Close ();
		}
		finally
		{
			if(file != null)
				file.Close();
		}

	}

	// Loads all of the varibles for the currently selected profile
	private void LoadProfiles()
	{
		try
		{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.persistentDataPath + "/profiles.dat", FileMode.Open);
			ProfilesData data = (ProfilesData)bf.Deserialize(file); //without cast, makes generic obj
			file.Close();
			
			currText.text = data.current;
			profile_1.GetComponentInChildren<Text>().text = data.profile_1;
			profile_2.GetComponentInChildren<Text>().text = data.profile_2;
			profile_3.GetComponentInChildren<Text>().text = data.profile_3;
			profile_4.GetComponentInChildren<Text>().text = data.profile_4;
			profile_5.GetComponentInChildren<Text>().text = data.profile_5;
			profile_1_active = data.profile_1_active;
			profile_2_active = data.profile_2_active;
			profile_3_active = data.profile_3_active;
			profile_4_active = data.profile_4_active;
			profile_5_active = data.profile_5_active;
		}
		finally
		{
			if(file != null)
				file.Close();
		}

		GameManager.manager.currProfileName = currText.text;
	}

	// Determines which buttons should be active
	private void SetStartMenuButtons()
	{
		int isActive = 0;

		if(profile_1_active)
		{
			profile_1.interactable = true;
			isActive++;
		}
		if(profile_2_active)
		{
			profile_2.interactable = true;
			isActive++;
		}
		if(profile_3_active)
		{
			profile_3.interactable = true;
			isActive++;
		}
		if(profile_4_active)
		{
			profile_4.interactable = true;
			isActive++;
		}
		if(profile_5_active)
		{
			profile_5.interactable = true; 
			isActive++;
		}

		if(isActive > 0)
		{
			playButton.interactable = true;
			deleteButton.interactable = true;
			loadButton.interactable = true;
		}
		else
		{
			playButton.interactable = false;
			deleteButton.interactable = false;
			loadButton.interactable = false;
		}

		if(isActive < 5)
		{
			createButton.interactable = true;
		}

//		Debug.Log(isActive);
	}

	// Allows you to enter a new profile name when the create button is pushed
	public void CreateButton()
	{ 
		createName.interactable = true;
		createName.ActivateInputField ();
	}

	// Allows you to delete the currently selected profile and the profile name
	public void DeleteButton()
	{
		GameManager.manager.Delete (userName);

		if(defaultName == "Profile 1")
		{
			profile_1.GetComponentInChildren<Text>().text = defaultName;
			profile_1_active = false;
			profile_1.interactable = false;
		}
		else if(defaultName == "Profile 2")
		{
			profile_2.GetComponentInChildren<Text>().text = defaultName;
			profile_2_active = false;
			profile_2.interactable = false;
		}
		else if(defaultName == "Profile 3")
		{
			profile_3.GetComponentInChildren<Text>().text = defaultName;
			profile_3_active = false;
			profile_3.interactable = false;
		}
		else if(defaultName == "Profile 4")
		{
			profile_4.GetComponentInChildren<Text>().text = defaultName;
			profile_4_active = false;
			profile_4.interactable = false;
		}	
		else if(defaultName == "Profile 5")
		{
			profile_5.GetComponentInChildren<Text>().text = defaultName;
			profile_5_active = false;
			profile_5.interactable = false;
		}

		if(currText.text == userName)
		{
			currText.text = "Choose Profile";
		}
		SetStartMenuButtons ();
		SaveProfiles ();
	}

	// Simulates loading a profile: The actual loading is done when play is pressed
	public void LoadButton()
	{
		if(userName != null)
		{
			currText.text = userName;
		}
	}

	// Loads the currently selected profile and begins the game
	public void PlayButton(GameObject myImage)
	{
		myImage.SetActive (true);
		GameManager.manager.Load (currText.text);
		GameManager.manager.NextScene ("BeerToss");
	}
	
	// Sets this profile as the currently selected profile
	public void SetProfileSelected(Button selected)
	{
		userName = selected.GetComponentInChildren<Text>().text;
		defaultName = selected.name;
	}
}

//data container that allows you to write the data to a file
[Serializable]
class ProfilesData
{
	public string current;
	public string profile_1;
	public string profile_2;
	public string profile_3;
	public string profile_4;
	public string profile_5;
	public bool profile_1_active;
	public bool profile_2_active;
	public bool profile_3_active;
	public bool profile_4_active;
	public bool profile_5_active;
}
