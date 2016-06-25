using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
	private ThrowGlassControl tgcScript;
	private Vector3 camStart3_1;

	public static CameraManager camManScript;
	public GameObject touchBlocker;
	public Camera camera1;
	public Camera camera2;
	public Camera camera3;
	public Camera camera3_follow;
	public Camera camera3_view;
	public bool camSwitch1 = false;
	public bool camSwitch2 = false;
	public bool camSwitch3 = false;
	public bool camSwitch3_follow = false;
	public bool camSwitch3_view = false;

	private void Awake()
	{
		camManScript = this;
	}

	private void Start()
	{
		camStart3_1 = camera3_follow.transform.position;
		tgcScript = GameObject.Find ("ThrowGlassTrigger").GetComponent<ThrowGlassControl> ();
	}

	private void Update () 
	{
		if(camSwitch1)
		{
			Camera.main.enabled = false;
			camera1.enabled = true;

			camSwitch1 = false;
		}
		else if(camSwitch2)
		{
			Camera.main.enabled = false;
			camera2.enabled = true;
            
			camSwitch2 = false;
		}
		else if(camSwitch3)
		{
			camera3_follow.transform.position = camStart3_1;
			Camera.main.enabled = false;
			tgcScript.powerMeter.enabled = false;
			tgcScript.pwrMeterSkin.enabled = false;
			tgcScript.powerMeterPanel.enabled = false;
			tgcScript.armWithPivot.SetActive (false);
			camera3.enabled = true;

			camSwitch3 = false;
		}
		else if(camSwitch3_follow)
		{
			Camera.main.enabled = false;
 			camera3_follow.enabled = true;
		
			camSwitch3_follow = false;
		}
		else if(camSwitch3_view)
		{
			Camera.main.enabled = false;
			camera3_view.enabled = true;
			camera3_view.GetComponent<Animator>().SetBool("ViewEnabled", true);
			//camSwitch3_view = false;
		}
		else
		{
			camera3_view.GetComponent<Animator>().SetBool("ViewEnabled", false);
		}
	
		if (camera3_follow.enabled)
			touchBlocker.SetActive (true);
		else if(touchBlocker.activeInHierarchy)
			touchBlocker.SetActive(false);
	}
}

