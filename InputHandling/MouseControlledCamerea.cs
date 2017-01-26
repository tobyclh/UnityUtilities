using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MouseControlledCamerea : MonoBehaviour
{

    // Use this for initialization
    private float xPos;
    private float yPos;
    private Vector3 startingPos;
    private int frameCount = 0;
    private List<R> rl = new List<R>();
    private bool shouldStop = false;
    private bool leaveTrace = true;
    private Vector3 mouse_pos;

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            rl.Add(new R());
        }
        Debug.Log("BANANA");
        startingPos = transform.position;
        Camera cam = Camera.main;
        cam.transform.position = transform.position + new Vector3(0,0,-5);
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (!shouldStop)
        {

            int _value = 0;
            foreach(var r in rl)
            {
                _value += r.r.Next();
            }
            Debug.Log(_value);
            if (_value % 2 == 0)
            {
                transform.position += new Vector3(0.01f, 0.1f, 0);
            }
            else
            {
                transform.position += new Vector3(0.01f, -0.1f, 0);
            }
            if (leaveTrace)
            {
                var traceDot = Instantiate(gameObject);
                traceDot.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                var rk = traceDot.GetComponent<RandomWalk>();
                if (rk != null)
                    Destroy(rk);
            }

        }
        else if (shouldStop)
        {
            Application.CaptureScreenshot("BANANA.png");
        }


        if (Input.GetKeyUp(KeyCode.Q))
        {
            shouldStop = true;
        }
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


    class R
    {
        public System.Random r = new System.Random();
    }
}
