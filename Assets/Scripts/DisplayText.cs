using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] private string message;

    void Start()
    {
        displayText.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



