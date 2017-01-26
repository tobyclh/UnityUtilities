using UnityEngine;
using System.Collections;

public class KeyboardControlledCamera : MonoBehaviour
{

    public Camera main;
    public float factor;
    // Use this for initialization
    void Start()
    {
        main.backgroundColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            main = Camera.current;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            main.transform.position += Vector3.up * factor;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            main.transform.position -= Vector3.up * factor;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            main.transform.position += Vector3.left * factor;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            main.transform.position -= Vector3.left * factor;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            main.transform.position += Vector3.forward * factor;
        }
        if (Input.GetKey(KeyCode.W))
        {
            main.transform.position -= Vector3.forward * factor;
        }
        if (Input.GetKey(KeyCode.E))
        {
            main.fieldOfView += 2;
        }
        if (Input.GetKey(KeyCode.R))
        {
            main.fieldOfView -= 2;
        }
    }
    /* baud rate 96700 send 0 and 1*/
}
