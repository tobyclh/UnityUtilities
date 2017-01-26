using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class PlayMovie : MonoBehaviour
{
    [SerializeField]
    private string filename;
    [SerializeField]
    private MovieTexture movie;
    private float movieDuration;
    private float startTime;

    [Tooltip("Frame rate")]
    [SerializeField]
    private float frameRate = 6f;

    private Transform OVRCamera;

    private List<Vector3> pointPosList;
    //[SerializeField]
    //PupilListener pupil_listener;
    [Tooltip("Pointer to show target position")]
    [SerializeField]
    private GameObject pointerPrefab;
    private Transform m_pointer;
    private Transform play_pointer;
    [Tooltip("Pointer dis to cam")]
    [SerializeField]
    private float pointerOffset;
    private Vector3 basePos;
    [SerializeField]
    private Transform gazeRender;

    private GameObject canvasPrefab;
    private Transform play_canvas;
    void Start()
    {
        OVRCamera = OculusManager.OVRCamera;
        basePos = OVRCamera.position;
        pointPosList = new List<Vector3>();


        gazeRender.GetComponent<Renderer>().material.color = Color.green;

    }

    void StartRecording()
    {
        movie.Play();
        movieDuration = movie.duration;
        //m_pointer.localPosition = new Vector3(0, 0, pointerOffset);
        gazeRender.GetComponent<Renderer>().material.color = Color.red;
        foreach (var renderer in play_canvas.GetComponents<Renderer>())
        {
            renderer.material.color = Color.red;
        }
        InvokeRepeating("RecordHeadOri", 0, 1 / frameRate);
        Invoke("StopRecord", movieDuration);
    }

    void UpdateGazePos()
    {
        //Vector3 GazePosFixedDistance = new Vector3(pupil_listener.GetGazePos().x, pupil_listener.GetGazePos().y, pointerOffset);
        //m_pointer.localPosition = pupil_listener.GetGazePos();
        //m_pointer.LookAt(OVRCamera);
    }

    void RecordHeadOri()
    {
        pointPosList.Add(gazeRender.position);
        ES2.Save<Vector3>(pointPosList[pointPosList.Count - 1], filename + "?tag=" + "pointPosList" + pointPosList.Count);
        ES2.Save<Vector3>(play_canvas.position, filename + "?tag=" + "headOri" + pointPosList.Count);
    }

    void StopRecord()
    {
        Debug.Log("Stop recording");
        CancelInvoke("RecordHeadOri");
        //SaveList<Vector3>(pointPosList, "pointPosList");
        ES2.Save<float>(frameRate, filename + "?tag=" + "frameRate");
        ES2.Save<float>(movieDuration, filename + "?tag=" + "movieDuration");
        gazeRender.GetComponent<Renderer>().material.color = Color.green;
        foreach (var material in play_canvas.GetComponent<Renderer>().materials)
        {
            material.color = Color.white;
        }

    }

    void StartPlaying()
    {
        if (!ES2.Exists(filename))
        {
            Debug.LogError("Save - Save Not Found");
            return;
        }
        play_pointer = Instantiate<GameObject>(pointerPrefab).transform;
        play_canvas = Instantiate<GameObject>(canvasPrefab).transform;
        foreach (var material in play_canvas.GetComponent<Renderer>().materials)
        {
            material.color = Color.blue;
        }
        play_pointer.GetComponent<Renderer>().material.color = Color.blue;
        basePos = OVRCamera.position;


        var m_frameRate = ES2.Load<float>(filename + "?tag=" + "frameRate");
        var m_movieDuration = ES2.Load<float>(filename + "?tag=" + "movieDuration");
        var frameCount = ES2.Load<int>(filename + "?tag=" + "listCount");
        LoadList<Vector3>(pointPosList, "pointPosList", frameCount);
        movie.Play();
        InvokeRepeating("PlayHeadOri", 0, 1 / m_frameRate);
    }

    void PlayHeadOri()
    {
        if (pointPosList.Count == 0)
        {
            Debug.Log("PlayMovie : No frame to play");
            StopPlay();
            return;
        }
        play_pointer.position = basePos + pointPosList[0];

        play_pointer.LookAt(OVRCamera);
        pointPosList.RemoveAt(0);
    }

    void StopPlay()
    {
        CancelInvoke("PlayHeadOri");
        Debug.Log("PlayMovie : Finish Playing");
        Destroy(play_pointer);
        Destroy(play_canvas);
    }

    void SaveList<T>(List<T> list, string tag)
    {
        for (int num = 0; num < list.Count; num++)
        {
            ES2.Save<T>(list[num], filename + "?tag=" + tag + num);
        }
        ES2.Save<int>(pointPosList.Count, filename + "?tag=" + "listCount");

    }
    void LoadList<T>(List<T> list, string tag, int listCount)
    {
        for (int num = 0; num < listCount; num++)
        {
            list.Add(ES2.Load<T>(filename + "?tag=" + tag + num));
        }
    }

private bool isMarking = false;
private float markUpStartTime = 0;
private float markUpEndTime = 0;
    void MarkUp()
    {
        if(isMarking)
        {
            
            isMarking = false;
        }
        else
        {

            isMarking = true;
        }


    }

    void Update()
    {
        UpdateGazePos();
        if (Input.GetKeyUp(KeyCode.R))
        {
            StartRecording();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            StartPlaying();
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            MarkUp();
        }

    }
}