using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pythonTest : MonoBehaviour
{
    private float time = 0.0f;
    public float interpolationPeriod = 1f;
    int count = 0;


    void Update()
    {
        /*time += Time.deltaTime;
        if (time >= interpolationPeriod)
        {
            time = 0.0f;
            saveSS();
        }*/
    }

    private void saveSS()
    {
        ScreenCapture.CaptureScreenshot("myss.png");


    }





}
