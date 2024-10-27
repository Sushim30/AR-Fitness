using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;  // Add this for UI interaction detection
using UnityEngine.SceneManagement;



public class ARYogaSessionController : MonoBehaviour
{
    // AR and yoga session objects
    public GameObject yogaTutorPrefab;
    private GameObject spawnedObject;
    public GameObject EndScreen;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>(); // Non-static list for raycast results

    // UI elements
    public Button deployButton;
    public Button startButton;
    public Button pauseButton;
    public Button resetButton;
    public Text timerText;

    // Session management
    public AudioSource yogaInstructionsAudio;
    private Animator yogaTutorAnimator;
    private bool isPaused = false;
    private bool sessionStarted = false;
    private bool canPlaceObject = false;
    private bool uiInteraction = false;  // Flag to check if UI was interacted with
    private float sessionTime = 60f;  // 1 minute session

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // Button click listeners
        deployButton.onClick.AddListener(OnDeployButtonClicked);
        startButton.onClick.AddListener(StartSession);
        pauseButton.onClick.AddListener(PauseSession);
        resetButton.onClick.AddListener(OnResetButtonClicked);

        // Initial button visibility
        resetButton.gameObject.SetActive(false);  // Hide reset button initially
        startButton.gameObject.SetActive(false);  // Hide start button initially
        pauseButton.gameObject.SetActive(false);  // Hide pause button initially
        EndScreen.SetActive(false);
    }

    void Update()
    {
        HandleARObjectPlacement();

        // Yoga session timer
        if (sessionStarted && !isPaused)
        {

            if (sessionTime > 0 )
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

    void HandleARObjectPlacement()
    {
        if (Input.touchCount > 0 && canPlaceObject && !uiInteraction)
        {
            Touch touch = Input.GetTouch(0);

            // Check if touch is over UI element, skip placement if it is
            if (IsTouchOverUI(touch)) return;

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(yogaTutorPrefab, hitPose.position, hitPose.rotation);
                    yogaTutorAnimator = spawnedObject.GetComponent<Animator>();
                    yogaTutorAnimator.speed = 0;
                    startButton.gameObject.SetActive(true);
                    resetButton.gameObject.SetActive(true);

                    canPlaceObject = false;  // Prevent placing another object
                }
            }
        }
    }

    void OnDeployButtonClicked()
    {
        canPlaceObject = true;
        deployButton.gameObject.SetActive(false);  // Hide deploy button after placing object

        // Set UI interaction flag and delay reset
        uiInteraction = true;
        StartCoroutine(ResetUIInteraction());
    }

    void OnResetButtonClicked()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);  // Remove the current object
        }

        // Reset the buttons and states
        sessionStarted = false;
        sessionTime = 60f;  // Reset session time
        deployButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        timerText.text = "";
    }

    void StartSession()
    {
        if (yogaTutorAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;  // Ensure the Animator is assigned
        }

        Debug.Log("Session started.");
        yogaTutorAnimator.speed = 1;

        // Start coroutine to play audio after a delay
        StartCoroutine(PlayAudioWithDelay(15f));  // Wait 15 seconds before playing audio

        startButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        sessionStarted = true;  // Mark session as started
        isPaused = false;  // Ensure the session is not paused
    }

    // Coroutine to play audio after a delay
    IEnumerator PlayAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yogaInstructionsAudio.Play();  // Play audio instructions after the delay
    }

    void PauseSession()
    {
        if (isPaused)
        {
            yogaTutorAnimator.speed = 1;
            yogaInstructionsAudio.UnPause();
            Debug.Log("Session resumed.");
            isPaused = false;

        }
        else
        {
            yogaTutorAnimator.speed = 0;
            yogaInstructionsAudio.Pause();
            Debug.Log("Session paused.");
            isPaused = true;
        }
    }

    void EndSession()
    {
        // End the session
        yogaTutorAnimator.speed=0;  // Stop animation
        yogaInstructionsAudio.Stop();
        Debug.Log("Session finished.");
        timerText.text="Done";

        // Reset buttons
        startButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);  // Hide pause button
        sessionStarted = false;
        sessionTime = 60f;  // Reset the session time
        EndScreen.SetActive(true); 

    }

    // Check if touch is over UI
    bool IsTouchOverUI(Touch touch)
    {
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return true;
        }
        return false;
    }

    // Coroutine to reset the UI interaction flag after a short delay
    IEnumerator ResetUIInteraction()
    {
        yield return new WaitForSeconds(0.2f);  // Small delay before allowing AR placement again
        uiInteraction = false;
    }
}
