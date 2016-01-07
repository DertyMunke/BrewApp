using UnityEngine;
using System.Collections;

public class PunchGameUI : MonoBehaviour {

    public static PunchGameUI punchGameUIScript;
    public SpriteRenderer title;
    public SpriteRenderer player;
    public SpriteRenderer punch;
    public Sprite[] players;
    public Sprite[] punches;
    public Sprite[] scoreTitles;
    public bool punchDone;

    private Animator animator;

    public bool AnimDone { get { return punchDone; } set { punchDone = value; } }
    public int SetPlayer { set { player.sprite = players[value]; } }
    public int SetPunch { set { punch.sprite = punches[value]; } }
    public int SetTitle { set { title.sprite = scoreTitles[value]; } }

    private void Start()
    {
        punchGameUIScript = this;
        animator = gameObject.GetComponent<Animator>();
    }

    public void PunchTrigger(string strength)
    {
        animator.SetTrigger(strength);
        animator.SetTrigger("Punch");
        animator.SetBool("Go", false);
    }

    public void GoEnabled(bool lower = false)
    {
        animator.SetBool("Go", true);

        if(lower)
            Invoke("LowerBag", 0.5f);
    }

    private void LowerBag()
    {
        PunchManager.pManagerScript.SetBag = true;
        //PowerMeter2.powerMeter2Script.MeterObj_1.SetActive(true);
    }
}
