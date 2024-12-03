using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Manager_Script : MonoBehaviour
{
    // A variable to control the time scale
    public float timeScale = 1.0f; // Default to normal speed (1.0)

    // Update is called once per frame
    void Update()
    {
        // Set the global time scale to the value of timeScale
        Time.timeScale = timeScale;
    }

    // A method to change the time scale dynamically (could be called from other scripts or UI)
    public void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
    }
}
