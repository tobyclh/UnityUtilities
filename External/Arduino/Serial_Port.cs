using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;
using System.Collections.Generic;

public class Serial_Port : MonoBehaviour
{
    public string portName = "COM3";
    public float frameRate = 15;
    private PhysicalCalendar _calendar;
    public PhysicalCalendar Calendar
    {
        get
        {
            if(_calendar == null)
            {
                _calendar = FindObjectOfType<PhysicalCalendar>();
            }
            return _calendar;
        }
    }
    static SerialPort _serialPort;
    static bool _continue;
    public bool shouldSwitch = false;
    Thread readThread;
    public List<GameObject> privateEvents = new List<GameObject>();
    public List<GameObject> publicEvents = new List<GameObject>();
    public float serialDelay = 0.01f;

    // Use this for initialization
    void Start()
    {
        
        _serialPort = new SerialPort();
        readThread = new Thread(Read);
        // Allow the user to set the appropriate properties.
        _serialPort.PortName = portName;
        _serialPort.BaudRate = 57600; //SetPortBaudRate(96700);
        _serialPort.Parity = Parity.None;// SetPortParity(_serialPort.Parity);
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        //_serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);



        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;
        _serialPort.Encoding = System.Text.Encoding.ASCII;
        _serialPort.Open();
        _continue = true;
        readThread.Start();
        StartCoroutine(Write());

        Debug.Log("Name: ");
        //name = Console.ReadLine();

        //Console.WriteLine("Type QUIT to exit");
        bool isPublic = false;
        double lastRefreshTime = Time.time;

    }
    public void OnApplicationQuit()
    {
        _continue = false;
        readThread.Join();
        _serialPort.Close();

    }
    public static void Read()
    {
        while (_continue)
        {
            try
            {
                string message = _serialPort.ReadLine();
                Debug.Log(message);
            }
            catch (TimeoutException) { }
        }
    }

    public IEnumerator Write()
    {
        bool isPublic = false;
        byte[] zero = { 0, 0, 0, 0, 0, 0, 0, 0 };
        byte[] one = { 0, 0, 0, 0, 0, 0, 0, 1 };

        while (_continue)
        {
            
            Debug.Log("Writing");
            if (isPublic)
            {
                //_serialPort.Write();
                _serialPort.Write(zero, 0, 8);
                if (shouldSwitch)
                {
                    StartCoroutine(OnPrivateView());
                }
                else
                {
                    StartCoroutine(OnPublicView());
                }

            }
            else
            {
                //_serialPort.Write(1.ToString());
                _serialPort.Write(one, 0, 8);
                //OnPrivateView();
                //Invoke("OnPrivateView", serialDelay);
                if (!shouldSwitch)
                {
                    StartCoroutine(OnPrivateView());
                }
                else
                {
                    StartCoroutine(OnPublicView());
                }

            }
            isPublic = !isPublic;

            yield return new WaitForSeconds(1 / frameRate);
        }

    }

    private IEnumerator OnPrivateView()
    {
        yield return new WaitForSecondsRealtime(serialDelay/1000);
        foreach (var gobj in publicEvents)
        {
            gobj.SetActive(false);
        }
        foreach (var gobj in privateEvents)
        {
            gobj.SetActive(true);
        }

    }

    private IEnumerator OnPublicView()
    {
        yield return new WaitForSecondsRealtime(serialDelay/1000);

        foreach (var gobj in privateEvents)
        {
            gobj.SetActive(false);
        }
        foreach (var gobj in publicEvents)
        {
            gobj.SetActive(true);
        }

    }


    // Display Port values and prompt user to enter a port.
    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }
    // Display BaudRate values and prompt user to enter a value.
    //public static int SetPortBaudRate(int defaultPortBaudRate)
    //{
    //    string baudRate;

    //    Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
    //    baudRate = Console.ReadLine();

    //    if (baudRate == "")
    //    {
    //        baudRate = defaultPortBaudRate.ToString();
    //    }

    //    return int.Parse(baudRate);
    //}

    // Display PortParity values and prompt user to enter a value.
    public static Parity SetPortParity(Parity defaultPortParity)
    {
        string parity;

        Console.WriteLine("Available Parity options:");
        foreach (string s in Enum.GetNames(typeof(Parity)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
        parity = Console.ReadLine();

        if (parity == "")
        {
            parity = defaultPortParity.ToString();
        }

        return (Parity)Enum.Parse(typeof(Parity), parity, true);
    }
    // Display DataBits values and prompt user to enter a value.
    public static int SetPortDataBits(int defaultPortDataBits)
    {
        string dataBits;

        Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
        dataBits = Console.ReadLine();

        if (dataBits == "")
        {
            dataBits = defaultPortDataBits.ToString();
        }

        return int.Parse(dataBits.ToUpperInvariant());
    }

    // Display StopBits values and prompt user to enter a value.
    public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
    {
        string stopBits;

        Console.WriteLine("Available StopBits options:");
        foreach (string s in Enum.GetNames(typeof(StopBits)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter StopBits value (None is not supported and \n" +
         "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
        stopBits = Console.ReadLine();

        if (stopBits == "")
        {
            stopBits = defaultPortStopBits.ToString();
        }

        return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
    }
    public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
    {
        string handshake;

        Console.WriteLine("Available Handshake options:");
        foreach (string s in Enum.GetNames(typeof(Handshake)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
        handshake = Console.ReadLine();

        if (handshake == "")
        {
            handshake = defaultPortHandshake.ToString();
        }

        return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
    }
    //}
}
