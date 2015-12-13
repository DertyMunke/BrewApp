using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerMeter2 : MonoBehaviour {

    #region Variables
    public static PowerMeter2 powerMeter2Script;
    public GameObject meterObj_1;
    public GameObject meterObj_2;
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
    private float meterSpeed_1 = .03f;
    private float meterSpeed_2 = .02f;
    private int meterIndex_1;

    #endregion
    #region Public Methods
    public GameObject MeterObj_1 { get { return meterObj_1; } }
    public GameObject MeterObj_2 { get { return meterObj_2; } }
    public float MtrDist_2 { get { return Mathf.Abs(meterSprite_2.transform.localPosition.x - center_2.localPosition.x); } }
    public int MtrIndex_1 { get { return meterIndex_1; } }

    /// <summary>
    /// Resets both power meters to the start position
    /// </summary>
    public void ResetMeters()
    {
        meter_1.sprite = meterSprites_1[0];
        meterSprite_2.transform.position = rightBound_2.position;
        userTurn = !userTurn;
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
                // Displays the results of the added final values
                PunchManager.pManagerScript.CompletePunch(FinalScore(), 200);

                // Reset for the next turn
                ResetMeters();
            }
        }
    }

    /// <summary>
    /// Animates power meter 2's power bar
    /// </summary>
    private IEnumerator PowerMeterAnim_2()
    {
        yield return new WaitForSeconds(.5f);
        meterObj_1.SetActive(false);
        yield return new WaitForSeconds(.1f);
        meterObj_2.SetActive(true);

        float meterDelta = 5; // The distance that the meter moves 
        while (meterAnim_2)
        {
            // Determins the direction of the meter bar
            if (meterIncr_2)
                meterDelta = -5;
            else
                meterDelta = 5;

            // Set new position
            meterSprite_2.transform.position = new Vector2(meterSprite_2.transform.position.x + meterDelta, meterSprite_2.transform.position.y);

            // Switch direction if the bar passes the boundry
            if (meterSprite_2.transform.position.x <= leftBound_2.position.x)
                meterIncr_2 = false;
            else if (meterSprite_2.transform.position.x >= rightBound_2.position.x)
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
        while(meterAnim_1)
        {
            // If increasing then add index
            if (meterIncr_1 && meterIndex_1 < meterSprites_1.Length -1)
            {
                meterIndex_1++;
                meterIncr_1 = true;
            }
            else
                meterIncr_1 = false;

            // If decreasing then subtract index
            if (!meterIncr_1 && meterIndex_1 > 0)
            {
                meterIndex_1--;
                meterIncr_1 = false;
            }
            else
                meterIncr_1 = true;

            // Set new power meter image
            meter_1.sprite = meterSprites_1[meterIndex_1];

            yield return new WaitForSeconds(meterSpeed_1);
        }
        yield return null;
    }

    #endregion
}
