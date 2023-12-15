using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fpsChecker : MonoBehaviour
{
    private float updateInterval = 0.5f; // Update interval for displaying FPS
    private float accumDeltaTime = 0f;
    private int frames = 0;
    private float fps = 0f;
    public  Text fpsText;
    LagDetector LagDetector;
    bool jump;
    public Text coin;

    private void Start()
    {
        // Set the target frame rate to a high value for accurate FPS calculation
        //Application.targetFrameRate = 30;
        LagDetector = GetComponent<LagDetector>();
    }

    private void Update()
    {
        // Calculate FPS
        accumDeltaTime += Time.deltaTime;
        frames++;

        if (accumDeltaTime >= updateInterval)
        {
            // Calculate average FPS over the update interval
            fps = frames / accumDeltaTime;

            // Reset counters
            accumDeltaTime = 0f;
            frames = 0;
        }

    fpsText.text = ("FPS: " + Mathf.Round(fps)).ToString();

    if(fps<10 && !jump)
    {
        jump = true;
        LagDetector.goToScene();
    }

        coinCheker();

    }

    void LateUpdate()
    {
            string sceneName = SceneManager.GetActiveScene().name;
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                //Destroy(gameObject);
            }
    }


    void coinCheker()
    {
        coin.text = PlayerDataManager.Coins.ToString();
    }

}
