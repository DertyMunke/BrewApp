using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreAnims : MonoBehaviour
{
	public static ScoreAnims scoreAnimsScript;
	public GameObject tipChingAnim;
	public GameObject xpChingAnim;
	public GameObject repAnim;
	public GameObject tipAnim;
	public Text tipText;
	public Text repText;
	public bool scoreAnimTrig;
	public float tipValue = 0;
	public float resetChingDelay = 2;
	public int repValue = 0;

	void Awake ()
	{
		scoreAnimsScript = this;
	}

	void Start () 
	{
		tipChingAnim.SetActive(false);
		xpChingAnim.SetActive(false);
		repAnim.SetActive (false);
		tipAnim.SetActive (false);
	}
	
	void Update () 
	{
		if(scoreAnimTrig)
		{	
			tipText.text = string.Format("${0:F2}",tipValue);
			tipText.enabled = true;
			tipText.GetComponent<Animator>().enabled = true;
			repText.text = string.Format ("xp {0}", repValue);
			repText.enabled = true;
			repText.GetComponent<Animator>().enabled = true;
			Chinger ();
			Invoke ("ResetChingAnim", resetChingDelay);

			scoreAnimTrig = false;
		}
	}

	void ResetChingAnim()
	{
		xpChingAnim.SetActive(false);
		tipChingAnim.SetActive(false);
		repAnim.SetActive (false);
		tipAnim.SetActive (false);
	}

	void Chinger()
	{
		xpChingAnim.SetActive(true);
		tipChingAnim.SetActive(true);
		repAnim.SetActive (true);
		tipAnim.SetActive (true);
	}
}
