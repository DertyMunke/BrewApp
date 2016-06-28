using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerMeter2 : MonoBehaviour {

    #region Variables
    public static PowerMeter2 powerMeter2Script;
    public GameObject meterObj_1;
    public GameObject meterObj_2;
    public GameObject meterBtn_1;
    public Transform leftBound_2;
    public Transform rightBound_2;
    public Transform center_2;
    public Image meter_1;
    public Image meterSprite_2;
    public Sprite[] meterSprites_1;

    private bool userTurn = false;
    private bool meterAnim_1 = true;
    private bool meterAnim_2 = true;
    private bool meterIncr_1 = true;
    private bool meterIncr_2 = true;
    private float meterDist_2 = 5;
    private float meterSpeed_1 = 0.04f;
    private float meterSpeed_2 = 0.04f;
    private int meterIndex_1;

    #endregion

    #region Public Methods
    public GameObject MeterObj_1 { get { return meterObj_1; } }
    public GameObject MeterObj_2 { get { return meterObj_2; } }
    public float MtrDist_2 { get { return Mathf.Abs(meterSprite_2.transform.localPosition.x - center_2.localPosition.x); } }
    public int MtrIndex_1 { get { return meterIndex_1; } }

    /// <summary>
    /// Sets the speed of the power meter according to the odds
    /// </summary>
    public void SetPowerDifficulty(int odds)
    {
        meterSpeed_1 -= (odds * 0.01f);
        meterSpeed_2 -= (odds * 0.01f);
        meterDist_2 = (rightBound_2.position.x - leftBound_2.position.x) / (90 - (odds * 10)); // 4 + odds;
    }

    /// <summary>
    /// Resets both power meters to the start position
    /// </summary>
    public void ResetMeters()
    {
        userTurn = !userTurn;
        //meterObj_1.SetActive(true);
        MeterObj_2.SetActive(false);

        if (userTurn)
            meterBtn_1.SetActive(true);
        else
            meterBtn_1.SetActive(false);

        meter_1.sprite = meterSprites_1[0];
        meterSprite_2.transform.position = rightBound_2.position;       
        meterAnim_1 = true;
        meterAnim_2 = true;
        meterIndex_1 = 0;
    }

    /// <summary>
    /// Starts power meter 1 animation on button press
    /// </summary>
    public void PowerBtnDown()
    {
        StartCoroutine(PowerMeterAnim_1());
    }

    /// <summary>
    /// Stops power meter 1 animation on button depress
    /// </summary>
    public void PowerBtnUp()
    {
        PunchToot.punchTootScript.CheckContinue(3);
        meterAnim_1 = false;
        StartCoroutine(PowerMeterAnim_2());
    }

    /// <summary>
    /// Stops the second power meter and calculates the total, final score
    /// </summary>
    public float FinalScore()
    {
        meterAnim_2 = false;

        // Calculate the final values
        float meterVal_2 = 100 - Mathf.Abs(meterSprite_2.transform.localPosition.x - center_2.localPosition.x);
        float meterVal_1 = 100 - (Mathf.Abs(21 - meterIndex_1) * 9.9f);

        return meterVal_1 + meterVal_2;
    }

    #endregion

    #region Private Methods
    private void Start()
    {
        powerMeter2Script = this;
    }

    private void FixedUpdate()
    {
        if(userTurn && meterAnim_2 && meterObj_2.activeInHierarchy)
        {
            // Stops the second meter when the user touches the screen
            if(Input.GetMouseButtonDown(0))
            {
                PunchToot.punchTootScript.CheckContinue(4);
                // Displays the results of the added final values
                PunchManager.pManagerScript.CompletePunch(FinalScore(), 200);
                Invoke("PunchPause", 0.5f);
            }
        }
    }

    /// <summary>
    /// Helper for a space between last power meter action and bag anim
    /// </summary>
    private void PunchPause()
    {
        PunchManager.pManagerScript.SetBag = true;
    }

    /// <summary>
    /// Animates power meter 2's power bar
    /// </summary>
    private IEnumerator PowerMeterAnim_2()
    {
        //meterSpeed_2 = maxMtrSpeed;

        yield return new WaitForSeconds(.5f);
        meterObj_1.SetActive(false);
        yield return new WaitForSeconds(.1f);
        meterObj_2.SetActive(true);

        float meterDelta; // The distance that the meter moves 
        while (meterAnim_2)
        {
            // Determins the direction of the meter bar
            meterDelta = meterIncr_2 ? -meterDist_2 : meterDist_2;

            // Set new position
            meterSprite_2.transform.position = new Vector2(meterSprite_2.transform.position.x + meterDelta, meterSprite_2.transform.position.y);

            // Switch direction if the bar passes the boundry
            if (meterIncr_2 && meterSprite_2.transform.position.x <= leftBound_2.position.x)
                meterIncr_2 = false;   
            else if (!meterIncr_2 && meterSprite_2.transform.position.x >= rightBound_2.position.x)
                meterIncr_2 = true;
    
            yield return new WaitForSeconds(meterSpeed_2);
        }

        yield return null;
    }

    /// <summary>
    /// Animates power meter 1's power sprites
    /// </summary>
    private IEnumerator PowerMeterAnim_1()
    {
        //meterSpeed_1 = maxMtrSpeed;

        while(meterAnim_1)
        {
            // If increasing then add index
            if (meterIncr_1 && meterIndex_1 < meterSprites_1.Length -1)
                meterIndex_1++;
            else if (meterIncr_1)
                meterIncr_1 = false;

            // If decreasing then subtract index
            if (!meterIncr_1 && meterIndex_1 > 0)
                meterIndex_1--;
            else if (!meterIncr_1)
                meterIncr_1 = true;

            // Set new power meter image
            meter_1.sprite = meterSprites_1[meterIndex_1];

            yield return new WaitForSeconds(meterSpeed_1);
        }
        yield return null;
    }

    #endregion
}
