using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BearAI : MonoBehaviour
{
	private Vector3 bearStartPos;
	private Vector3 punch2Pos = new Vector3 (8.7f, -.8f, 23.85f);
	private Vector3 punch1Pos = new Vector3 (7.77f, -.8f, 22.51f);
	private float oddsIncr = 0;
	private float gauge1Limit = 1f;
	private float gauge3Limit = .85f;
	private float gauge2Value = .3f;
	private int punchNum = 0;  // 1 = strong; 2 = medium; 3 = weak
	private int gaugeSpeed = 5;
    private int minStopMtr_1 = 17;
    private int maxStopMtr_1 = 24;
    private int minStopMtr_2;
    private int maxStopMtr_2 = 30;

	public static BearAI bearScript;
	public Camera bearCam;
	public Camera punchCam;
	public GameObject bag;
	public Image gauge1;
	public Image gauge2;
	public Image gauge3;
	public Image gaugeEnd;

	private void Awake()
	{
		bearScript = this;
	}

	private void Start()
	{
		bearStartPos = transform.position;
	}


	// Returns Bear to his start position
	public void RestartBear()
	{
		transform.position = bearStartPos;
	}

	// Starts coroutine that steps through the AI for the bears turn
	public void BearIt()
	{
		StartCoroutine ("BearsTurn");
	}

    /// <summary>
    /// Determines the bears behavior during his turn to punch
    /// </summary>
    private IEnumerator BearsTurn()
    {
        // Switch to the AI camera
        bearCam.enabled = true;
        punchCam.enabled = false;

        // Start the first power meter
        yield return new WaitForSeconds(.5f);
        PowerMeter2.powerMeter2Script.PowerBtnDown();
        gameObject.GetComponent<Animator>().SetBool("PunchIdle", true);
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        // Randomly find a value for power meter 1 within the allowable range
        int mtrIndex = PowerMeter2.powerMeter2Script.MtrIndex_1;
        while (mtrIndex <= minStopMtr_1 || mtrIndex >= maxStopMtr_1)
        {
            yield return new WaitForSeconds(Random.Range(0f, .5f));
            mtrIndex = PowerMeter2.powerMeter2Script.MtrIndex_1;
        }

        // Stop power meter 1 and start power meter 2
        PowerMeter2.powerMeter2Script.PowerBtnUp();
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        // Randomly find a value for power meter 2 within the allowable range
        float dist = PowerMeter2.powerMeter2Script.MtrDist_2;
        while(dist > maxStopMtr_2)
        {
            yield return new WaitForSeconds(Random.Range(0f, .5f));
            dist = PowerMeter2.powerMeter2Script.MtrDist_2;
        }

        // Stop power meter 2, calculate score and store the score for to determine bear animation
        float score = PowerMeter2.powerMeter2Script.FinalScore();

        // Time the punch animations with the bag hit animation
        float punchNum = score >= 80 ? 1 : score >= 60 ? 2 : 3;
        string punchAnim = "Punch" + punchNum.ToString();
        gameObject.GetComponent<Animator>().SetTrigger(punchAnim);

        if (punchNum == 1)
            yield return new WaitForSeconds(0.6f);
        else if (punchNum == 2)
            yield return new WaitForSeconds(0.72f);
        else
            yield return new WaitForSeconds(0.7f);

        BagHit();
        gameObject.GetComponent<Animator>().SetBool("PunchIdle", false);

        yield return new WaitForSeconds(.1f);
        PunchManager.pManagerScript.CompletePunch(score, 200);
        PowerMeter2.powerMeter2Script.ResetMeters();
        yield return null;
    }

    /// <summary>
    /// Old bear method
    /// </summary>
	private IEnumerator BearsTurn_Old()
    {
        bearCam.enabled = true;
        punchCam.enabled = false;

        // Save all the start positions of the gauges
        Vector2 g1Pos = gauge1.transform.position;
        Vector2 g2Pos = gauge2.transform.position;
        Vector2 g3Pos = gauge3.transform.position;
        Vector2 gEndPos = gaugeEnd.transform.position;

        float g1 = (g2Pos.x - g1Pos.x) * gauge1Limit;
        float g2 = (g3Pos.x - g2Pos.x);
        float g3 = (gEndPos.x - g3Pos.x) * gauge3Limit;

        // Determins the gauge values for bear punch
        g1 = Random.Range(g1 * oddsIncr, g1);
        g2 = Random.Range(g2 * oddsIncr, g2);
        g3 = Random.Range(g3 * oddsIncr, g3);

        // Predict the final gauge3 value to know what punch to setup for
        float g1Temp = g1;
        float g2Temp = g2;
        float total = g3;
        while (g2Temp > 0)
        {
            g2Temp -= 3;
            total += gauge2Value;
        }
        while (g1Temp > 0)
        {
            g1Temp -= 3;
            total += .9f;
        }

        if (total > gaugeEnd.transform.position.x)
        {
            total = gaugeEnd.transform.position.x;
        }

        g1 = g1Pos.x + g1;
        g2 = g2Pos.x + g2;
        g3 = g3Pos.x + g3;

        float max = gEndPos.x - g3Pos.x;
        float blockSize = (max * .5f) / 8;

        // Use total prediction to setup punch
        if (total < (max * .5f) + blockSize * 2)
        {
            punchNum = 3;
            transform.position = punch1Pos;
        }
        else if (total < (max * .5f) + blockSize * 6)
        {
            punchNum = 2;
            transform.position = punch2Pos;
        }
        else
        {
            punchNum = 1;
            transform.position = punch1Pos;
        }

        yield return new WaitForSeconds(2);

        // Animate the gauges increasing
        while (gauge1.transform.position.x <= g1 - gaugeSpeed)
        {
            gauge1.transform.position = new Vector2(gauge1.transform.position.x + gaugeSpeed, g1Pos.y);
            yield return new WaitForSeconds(.05f);
        }

        gameObject.GetComponent<Animator>().SetBool("PunchIdle", true);

        while (gauge2.transform.position.x <= g2 - gaugeSpeed)
        {
            gauge2.transform.position = new Vector2(gauge2.transform.position.x + gaugeSpeed, g2Pos.y);
            yield return new WaitForSeconds(.01f);
        }

        // Time the punch animations with the bag hit animation
        string punchAnim = "Punch" + punchNum.ToString();
        gameObject.GetComponent<Animator>().SetTrigger(punchAnim);
        if (punchNum == 1)
        {
            Invoke("BagHit", .6f);
        }
        else if (punchNum == 2)
        {
            Invoke("BagHit", .72f);
        }
        else
        {
            Invoke("BagHit", .70f);
        }

        while (gauge3.transform.position.x <= g3 - gaugeSpeed)
        {
            gauge3.transform.position = new Vector2(gauge3.transform.position.x + gaugeSpeed, g3Pos.y);
            yield return new WaitForSeconds(.01f);
        }

        gameObject.GetComponent<Animator>().SetBool("PunchIdle", false);
        PowerMeter.pmeterScript.AddBtnPower();
    }

    // Triggers the bag hit anim
    private void BagHit()
	{
        bag.GetComponent<Animator>().SetBool("punch", true);
    }

	// Set the difficulty of the Bear's AI
	public void SetBearDiff(int diff)
	{
		float multplr = diff - 1;

		oddsIncr = multplr * .2f + .1f;
		gauge1Limit = gauge1Limit - multplr * .025f;
		gauge3Limit = gauge3Limit + multplr * .025f;
		gauge2Value = gauge2Value - multplr * .05f;
	}
}
