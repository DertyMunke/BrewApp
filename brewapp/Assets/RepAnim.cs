using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RepAnim : MonoBehaviour 
{
	public static RepAnim repAnimScript;
	Animator repAnim;

	void Awake()
	{
		repAnimScript = this;
	}
		

	void Start () 
	{

		repAnim = GetComponent<Animator>();
	
	}
	
	void Update () 
	{
		if(ScoreAnims.scoreAnimsScript.scoreAnimTrig == true)
		{
			repAnim.SetBool("RepOn", true);
	

		}

	}
}
