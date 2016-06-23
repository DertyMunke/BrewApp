using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BarController : MonoBehaviour 
{
	private GameObject tapHandleClones;
	private CameraManager cameraScript;
	private Patron patronScript;
	private ThrowGlassControl tgcScript;
	private GUITexture[] labels;
	private bool tootBack = true;
	private bool stage1Once = false;
	private bool stage2Once = false;
	private bool stage3Once = false;
	private float blinkDelay = 1;
	private float blinkTimer;

	public static BarController barControlScript;
	public GameObject[] patrons  = new GameObject[3]; // 0 = rabbit, 1 = fox, 2 = bear
	public GameObject patronSpawn1;
	public GameObject patronSpawn2;
	public GameObject patronSpawn3;
	public GameObject patronSpawn4;
	public GameObject tap1Handle;
	public GameObject tap2Handle;
	public GameObject tap3Handle;
	public GameObject tapSpawn;
	public GameObject tapBlocker;
	public GameObject glass = null;
	public List<GameObject> servOrder = new List<GameObject>();
	public Camera cam1_2;
	public Camera cam1_3;
	public Transform teleport;
	public GUITexture fingerPtr;
	public Button leftViewBtn;
	public Button rightViewBtn;
	public Sprite[] leftIms = new Sprite[3];
	public Sprite[] rightIms = new Sprite[3];
	public bool tapSwipeEnded = true;
	public bool stage3ViewEnabled = false;
	public bool slingLabel = true;
	public bool gotOrder = false;
	public bool targetEnable = false;
	public float camViewStart;
	public float cameraStart2 = 1.0f;
	public float cameraSpeed = 10.0f;
	public float fingerBlinkTime = .3f;
	public int stage = 1;
//	public int numOrders = 0;
	
	private void Awake()
	{
		barControlScript = this;
	}
	
	private void Start () 
	{
		SetupLevel ();
		labels = tapHandleClones.GetComponentsInChildren<GUITexture> ();
		camViewStart = Camera.main.fieldOfView;
		stage = 1;

		cameraScript = CameraManager.camManScript;
		tgcScript = ThrowGlassControl.tgcScript;
	}

	private void FixedUpdate()
	{
		StageController ();

		// tootBack allows the tutorial to backtrack
		if (Input.touchCount == 0 && !tootBack) 
		{
			tootBack = true;
		}
	}

	// Determins what stage the game is in and controls what is seen in each stage by the camera
	private void StageController()
	{
		if(stage == 1)
		{
			if(!stage1Once)
			{
				stage1Once = true;
				
				if(targetEnable)
				{
					targetEnable = false;
				}
				
				if(!tapBlocker.GetComponent<Collider>().enabled)
				{
					tapBlocker.GetComponent<Collider>().enabled = true;
				}
			}
		}
		else if(stage1Once)
		{
			stage1Once = false;
			if(tapBlocker.GetComponent<Collider>().enabled)
			{
				tapBlocker.GetComponent<Collider>().enabled = false;
			}
		}

		if(stage == 2)
		{
			if(!stage2Once)
			{
				stage2Once = true;

				foreach(GUITexture label in labels)
				{
					label.enabled = true;
				}
			}
		}
		else if(stage2Once)
		{
			stage2Once = false;
			foreach(GUITexture label in labels)
			{
				label.enabled = false;
			}
		}

		if(stage == 3)
		{
			if(!stage3Once)
			{
				stage3Once = true;
				
				targetEnable = true;
			}
		}
		else if(stage3Once)
		{
			stage3Once = false;
			if(targetEnable)
			{
				targetEnable = false;
			}
		}

		
		if(stage != 3 && tgcScript.glassParented)
		{
			if(glass)
			{
				glass.transform.parent = null;
			}

			tgcScript.glassParented = false;
			
			tgcScript.ResetArm();
		}
	}

	// Initializes the variables and objects for the scene
	private void SetupLevel()
	{
		float ranNum = Random.Range (1, 6);
		patronSpawn1.transform.position = new Vector3(patronSpawn1.transform.position.x - ranNum, 
		                                              patronSpawn1.transform.position.y, patronSpawn1.transform.position.z);
		patrons [0].SetActive (true);

		if(GameManager.manager.barTossLevel <= 1)
		{
			tapHandleClones = (GameObject)(Instantiate (tap1Handle, tapSpawn.transform.position, Quaternion.identity ));

		}
		else if(GameManager.manager.barTossLevel == 2)
		{
			tapHandleClones = (GameObject)(Instantiate(tap2Handle, tapSpawn.transform.position, Quaternion.identity));
		}
		else if(GameManager.manager.barTossLevel == 3)
		{
			BeerTossCanvas.tossCanvasScript.expand = "expand2";
			CameraManager.camManScript.camera1 = cam1_2;
			tapHandleClones = (GameObject)(Instantiate(tap2Handle, tapSpawn.transform.position, Quaternion.identity));

			patrons [1].SetActive (true);
            ranNum = Random.Range(1, 6);
            patronSpawn2.transform.position = new Vector3(patronSpawn2.transform.position.x - ranNum, 
			                                              patronSpawn2.transform.position.y, patronSpawn2.transform.position.z);
		}
		else if(GameManager.manager.barTossLevel == 4)
		{
			BeerTossCanvas.tossCanvasScript.expand = "expand2";
			CameraManager.camManScript.camera1 = cam1_2;
			tapHandleClones = (GameObject)(Instantiate(tap3Handle, tapSpawn.transform.position, Quaternion.identity));

			patrons [1].SetActive (true);
            ranNum = Random.Range(1, 6);
            patronSpawn2.transform.position = new Vector3(patronSpawn2.transform.position.x - ranNum, 
			                                              patronSpawn2.transform.position.y, patronSpawn2.transform.position.z);
		}
		else
		{
			BeerTossCanvas.tossCanvasScript.expand = "expand3";
			CameraManager.camManScript.camera1 = cam1_3;
			tapHandleClones = (GameObject)(Instantiate(tap3Handle, tapSpawn.transform.position, Quaternion.identity));

			patrons [1].SetActive (true);
			patrons [2].SetActive (true);
            ranNum = Random.Range(1, 6);
            patronSpawn2.transform.position = new Vector3(patronSpawn2.transform.position.x - ranNum, 
			                                              patronSpawn2.transform.position.y, patronSpawn2.transform.position.z);
            ranNum = Random.Range(3, 6);
            patronSpawn3.transform.position = new Vector3(patronSpawn3.transform.position.x - ranNum, 
			                                              patronSpawn3.transform.position.y, patronSpawn3.transform.position.z);

		}

		CameraManager.camManScript.camSwitch1 = true;
	}

	// Animation for the tutorial finger pointer
	private void FingerBlinky(Vector3 position)
	{
		fingerPtr.transform.position = position;
		fingerPtr.enabled = true;

		if(blinkTimer - Time.time > fingerBlinkTime)
		{
			fingerPtr.enabled = false;
		}
		else if(blinkTimer - Time.time > 0)
		{
			fingerPtr.enabled = true;
		}
		else
		{
			blinkTimer = Time.time + blinkDelay;
		}
	}

	// Determines how the left views button behaves dependant on the current stage
	public void LeftViewBtn()
	{
		rightViewBtn.image.sprite = rightIms [0];

		if(stage == 2) 
		{
			if(targetEnable)
			{
				targetEnable = false;
			}

			if(tootBack)
			{
				TootText.tootScript.tootStage -= 1;
				tootBack = false;
			}
			
			if(glass)
			{
				rightViewBtn.interactable = false;
				glass.GetComponent<DestructionController>().goBoom = true;
				glass.GetComponent<GlassControl>().glassDestroy = true;
				glass = null;
			}
			else 
			{
				leftViewBtn.interactable = false;
				rightViewBtn.interactable = true;
				tapBlocker.GetComponent<Collider>().enabled = true;
				cameraScript.camSwitch1 = true;
				stage = 1;
			}
		}
		else if(stage == 3)
		{
			if(tootBack)
			{
				TootText.tootScript.tootStage -= 2;
				tootBack = false;
			}

			glass.GetComponent<DestructionController>().goBoom = true;
			glass.GetComponent<GlassControl>().glassDestroy = true;
			glass.transform.parent = null;
			glass = null;

			rightViewBtn.interactable = false;
			CameraManager.camManScript.camSwitch2 = true;
			stage = 2;
			ThrowGlassControl.tgcScript.downArrows.enabled = false;
			ThrowGlassControl.tgcScript.powerMeter.enabled = false;
			ThrowGlassControl.tgcScript.pwrMeterSkin.enabled = false;
			ThrowGlassControl.tgcScript.powerMeterPanel.enabled = false;
			ThrowGlassControl.tgcScript.armWithPivot.SetActive (false);

		}
	}

	// Determines how the right views button behaves dependant on the current stage
	public void RightViewBtn()
	{
		leftViewBtn.image.sprite = leftIms [0];
		if(stage == 1)
		{
			leftViewBtn.interactable = false;

			if(TootText.tootScript.tootStage == 1)
			{
				TootText.tootScript.tootStage = 2;
			}
			if(!glass)
			{
				rightViewBtn.interactable = false;
			}
			leftViewBtn.interactable = true;
			tapBlocker.GetComponent<Collider>().enabled = false;
			cameraScript.camSwitch2 = true;
			stage = 2;
		}
		else if(stage == 3)
		{
			cameraScript.camSwitch3_view = false;
			tapBlocker.GetComponent<Collider>().enabled = false;
			tgcScript.enabled = true;
			stage3ViewEnabled = false;
			cameraScript.camSwitch3 = true;
			leftViewBtn.interactable = true;
		}
	}

	// Behavior for right views button initialization on down values
	public void RightOnDown()
	{
		if(rightViewBtn.interactable)
		{
			leftViewBtn.interactable = true;
			leftViewBtn.image.sprite = leftIms [1];
			if(stage == 3)
			{
				//			TootText.tootScript.tootEnabled = false;
				tapBlocker.GetComponent<Collider>().enabled = true;
				CameraManager.camManScript.camSwitch3_view = true;
				ThrowGlassControl.tgcScript.downArrows.enabled = false;
				ThrowGlassControl.tgcScript.powerMeter.enabled = false;
				ThrowGlassControl.tgcScript.pwrMeterSkin.enabled = false;
				ThrowGlassControl.tgcScript.powerMeterPanel.enabled = false;
				ThrowGlassControl.tgcScript.armWithPivot.SetActive(false);
				ThrowGlassControl.tgcScript.enabled = false;

			}
		}
	}

	// Behavior for left views button initialization on down values
	public void LeftOnDown()
	{
		if(leftViewBtn.interactable)
		{
			rightViewBtn.interactable = true;
			rightViewBtn.image.sprite = rightIms [1];
		}
	}

	// Moves the glass into position to be thrown down the bar
	public void MoveToThrow(GameObject myGlass)
	{
		if(myGlass != null && myGlass.activeInHierarchy)
		{
			glass = myGlass;
			myGlass.transform.position = teleport.position;
			cameraScript.camSwitch3 = true;
			stage = 3;
//			rightViewBtn.interactable = true;

			if(TootText.tootScript.tootStage == 3)
				TootText.tootScript.tootStage = 4;
			else if(TootText.tootScript.tootStage == 8)
				TootText.tootScript.tootStage = 9;
		}
	}
}
