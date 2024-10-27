using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class YogaSessionController : MonoBehaviour
{
    public Button startButton;
    public Button pauseButton;
    public Animator yogaTutorAnimator;  // Make sure this is assigned via Inspector or code
    public AudioSource yogaInstructionsAudio;
    public Text timerText;

    private bool isPaused = false;
    private bool sessionStarted = false;
    private float sessionTime = 60f;  // 1 minute session

    void Start()
    {
        startButton.onClick.AddListener(StartSession);
        pauseButton.onClick.AddListener(PauseSession);

        pauseButton.gameObject.SetActive(false);

    }

    void Update()
    {
        if (sessionStarted && !isPaused)
        {
            if (sessionTime > 0)
            {
                sessionTime -= Time.deltaTime;
                timerText.text = "Time left: " + Mathf.FloorToInt(sessionTime).ToString();
            }
            else
            {
                EndSession();  // End session when time runs out
            }
        }
    }

    void StartSession()
    {
        if (yogaTutorAnimator == null)
        {
            Debug.LogError("Animator not assigned!"); 
            return;  // Make sure the Animator is assigned
        }

        Debug.Log("Session started.");

        yogaTutorAnimator.SetBool("isDoingYoga", true);  // Start yoga animation
        yogaInstructionsAudio.Play();  // Play audio instructions

        // Hide start button and show pause button
        startButton.gameObject.SetActive(false);  
        pauseButton.gameObject.SetActive(true);  

        sessionStarted = true;  // Mark session as started
        isPaused = false;  // Ensure the session is not paused
    }

    void PauseSession()
    {
        if (isPaused)
        {
            // Resume animation and audio
            yogaTutorAnimator.speed = 1;
            yogaInstructionsAudio.UnPause();
            Debug.Log("Session resumed.");
            isPaused = false;
        }
        else
        {
            // Pause animation and audio
            yogaTutorAnimator.speed = 0;
            yogaInstructionsAudio.Pause();
            Debug.Log("Session paused.");
            isPaused = true;
        }
    }

    void EndSession()
    {
        // Session ended
        yogaTutorAnimator.SetBool("isDoingYoga", false);  // Stop animation
        yogaInstructionsAudio.Stop();
        Debug.Log("Session finished.");

        // Reset buttons
        startButton.gameObject.SetActive(true);  
        pauseButton.gameObject.SetActive(false);  // Hide pause button
        sessionStarted = false;  // Reset session state
        sessionTime = 60f;  // Reset the session time
    }
}

