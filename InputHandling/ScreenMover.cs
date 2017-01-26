using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ScreenMover : MonoBehaviour
{

    // Use this for initialization
    private float xPos;
    private float yPos;
    private Vector3 startingPos;
    private Vector3 mouse_pos;

    void Start()
    {
        startingPos = transform.position;
        Camera cam = Camera.main;
        cam.transform.position = transform.position + new Vector3(0,0,-5);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0))
        {
            if(mouse_pos!= Vector3.zero)
            {
                Camera.main.transform.position+= (Input.mousePosition - mouse_pos)/-100;
            }
            mouse_pos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            mouse_pos = Vector3.zero;
        }
    }
    
}
