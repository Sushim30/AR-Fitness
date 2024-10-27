using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isPaused = false;

    void Start()
    {
        // Get the Animator component attached to the prefab
        animator = GetComponent<Animator>();
        
        // Debug to confirm Animator component is found
        if (animator != null)
        {
            Debug.Log("Animator found!");
        }
        else
        {
            Debug.LogError("Animator not found! Make sure the Animator component is attached.");
        }
    }

    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed!");

            if (isPaused)
            {
                Debug.Log("Resuming animation.");
                ResumeAnimation();
            }
            else
            {
                Debug.Log("Pausing animation.");
                PauseAnimation();
            }
        }
    }

    void PauseAnimation()
    {
        // Pause the animation by setting the speed to 0
        if (animator != null)
        {
            animator.speed = 0;
            isPaused = true;
            Debug.Log("Animation paused.");
        }
        else
        {
            Debug.LogError("Animator is null, can't pause the animation.");
        }
    }

    void ResumeAnimation()
    {
        // Resume the animation by setting the speed back to 1
        if (animator != null)
        {
            animator.speed = 1;
            isPaused = false;
            Debug.Log("Animation resumed.");
        }
        else
        {
            Debug.LogError("Animator is null, can't resume the animation.");
        }
    }
}

