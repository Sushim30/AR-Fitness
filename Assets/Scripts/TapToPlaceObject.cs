using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.UI;
public class TapToPlaceObject : MonoBehaviour
{
    public GameObject yogaTutorPrefab;
    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Button deployButton; 
    public Button startButton; 
    public Button resetButton; 
    private bool canPlaceObject = false; // Flag for deployment control
    private Animator yogaTutorAnimator;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // Add button click event listeners
        deployButton.onClick.AddListener(OnDeployButtonClicked);
        resetButton.onClick.AddListener(OnResetButtonClicked);

        resetButton.gameObject.SetActive(false); // Hide reset button initially
        startButton.gameObject.SetActive(false);  // Hide start button initially
    }

    void Update()
    {
        if (Input.touchCount > 0 && canPlaceObject)
        {
            Touch touch = Input.GetTouch(0);

            if (IsTouchOverUI(touch)) return;

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(yogaTutorPrefab, hitPose.position, hitPose.rotation);
                    Debug.Log("Object spawned at: " + hitPose.position);

                    // Get the Animator component from the spawned object
                    yogaTutorAnimator = spawnedObject.GetComponent<Animator>();

                    // Ensure the Animator stays idle when deployed
                    if (yogaTutorAnimator != null)
                    {
                        yogaTutorAnimator.SetBool("isDoingYoga", false); // Ensure it's in the idle state
                    }

                    startButton.gameObject.SetActive(true);
                    canPlaceObject = false;
                    resetButton.gameObject.SetActive(true);
                }
            }
        }
    }

    void OnDeployButtonClicked()
    {
        canPlaceObject = true;
        deployButton.gameObject.SetActive(false);     
    }

    void OnResetButtonClicked()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject); // Remove current object
        }
        canPlaceObject = false;
        deployButton.gameObject.SetActive(true); // Allow new placement
        startButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false); // Hide reset button again
    }

    bool IsTouchOverUI(Touch touch)
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }
}

