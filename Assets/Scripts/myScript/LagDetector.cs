using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LagDetector : MonoBehaviour
{
    public float lagThreshold = 0.05f; // Adjust this value to your desired threshold
    float timer;
    private void Update()
    {
        // Check if the frame rate is below the threshold
        if (Time.deltaTime > lagThreshold)
        {
            timer+=Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }

        if(timer>5f)
        {
            goToScene();
        }
    }

    public void goToScene()
    {
        // Return to the main menu scene
            SceneManager.LoadScene("MainMenu"); 
    }
}