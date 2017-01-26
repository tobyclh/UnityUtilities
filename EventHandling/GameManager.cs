using UnityEngine;
using System.Collections;
using Leap;
public class GameManager : MonoBehaviour
{
    
    // Use this for initialization
	Leap.Unity.LeapHandController controller;
    void Awake()
    {
        Screen.fullScreen = true;
        
    }
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Q))
        {
            Application.CaptureScreenshot(Time.time + ".png");
        }
		
    }
}
