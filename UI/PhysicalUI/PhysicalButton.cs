using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class PhysicalButton : PhysicalUI
{

    [SerializeField] private Color pressedColor;
    [SerializeField] private List<Transform> _reactants = new List<Transform>();
    private List<Transform> reactants = new List<Transform>();
    private double holdTime = 0;
    private PrimitiveType shape;
    // Use this for initialization
    public event PhysicalUIEventHandler onClick;
    public event PhysicalUIEventHandler onHold;
    public event PhysicalUIEventHandler onRelease;
    public bool initilized = false;

    void Start()
    {

        foreach (var reactant in _reactants)
        {
            AddReactant(reactant);
        }
        if (transform == null)
        {
            transform = GetComponent<Transform>();
        }
        if (!initilized)
        {
            Init();
        }
    }
    public void Setup(PrimitiveType _shape, Color _color, Color _pressedColor)
    {
        transform = GameObject.CreatePrimitive(_shape).transform;
        shape = _shape;
        color = _color == null ? Color.white : _color;
        pressedColor = _pressedColor;
        Init();
    }

    private void Init()
    {
        transform.GetComponent<Renderer>().material.color = color == null ? Color.white : color;
        initilized = true;
    }

    ///Who can press this button
    public void AddReactant(Transform reactant)
    {
        var transforms = reactant.GetComponentsInChildren<Transform>().ToList<Transform>();
        Debug.Log("reactant count " + transforms.Count);
        reactants.AddRange(transforms);
        // foreach(var _reactant in reactants)
        // {
        //     Debug.Log(_reactant.name);
        // }
    }


    private void OnTriggerEnter(Collider other)
    {
        // if (onClick.GetInvocationList().Count() == 0)
        // {
        //     return;
        // }
        Debug.Log("OnTriggerEnter");
        if (!reactants.Contains(other.transform))
        {
            return;
        }
        transform.GetComponent<Renderer>().material.color = pressedColor;
        if (onClick != null)
        {
            onClick.Invoke(other.transform, null);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // if (onHold.GetInvocationList().Count() == 0)
        // {
        //     return;
        // }
        if (!reactants.Contains(other.transform))
        {
            return;
        }
        if (onHold != null)
        {
            onHold.Invoke(other.transform, null);
        }


    }

    private void OnTriggerExit(Collider other)
    {


        if (!reactants.Contains(other.transform))
        {
            //discard if we dont need it
            return;
        }
        transform.GetComponent<Renderer>().material.color = color;
        if (onRelease != null)
        {
            onRelease.Invoke(other.transform, null);
        }

    }




}
