using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Manager_Script : MonoBehaviour
{
    public float timeScale = 1.0f;
    private float lastTimeScale;

    private void Start()
    {
        lastTimeScale = timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the global time scale to the value of timeScale
        Time.timeScale = timeScale;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timeScale == 0)
            {
                timeScale = lastTimeScale;
                Debug.Log("Resume Time");
            }
            else
            {
                lastTimeScale = timeScale;
                timeScale = 0;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            timeScale = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            timeScale = 30;
        }
    }

    // A method to change the time scale dynamically (could be called from other scripts or UI)
    public void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
    }
}
