using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PhysicalCalendar : PhysicalUI
{

    public class CalendarEvent
    {
        public const String EventUIPrefabString = "CalendarEvent";
        public DateTime StartTime;
        public DateTime EndTime;
        public float duration;//hours
        public Color color;
        public Transform calendarEventTrans;
        public bool isSecret = true;
        public CalendarEvent(DateTime _startTime, DateTime _endTime, Color _color, Transform parent)
        {
            StartTime = _startTime;
            EndTime = _endTime;
            duration = EndTime.Hour - StartTime.Hour;
            color = _color;
            var UI = Resources.Load(EventUIPrefabString) as GameObject;
            calendarEventTrans = Instantiate<GameObject>(UI).transform;
            calendarEventTrans.SetParent(parent);
            //calendarEventTrans.GetComponent<Material>().color = _color;
            calendarEventTrans.localScale = new Vector3(1, duration, 1);
        }
        public void Hide()
        {
            calendarEventTrans.gameObject.SetActive(false);
        }

        public void Show()
        {
            calendarEventTrans.gameObject.SetActive(true);
        }
    }

    public class CalendarPage
    {
        public int day = 1;
        public int month = 1;
        public int year = 2017;
        public List<CalendarEvent> eventList = new List<CalendarEvent>();
        public void HidePage()
        {
            foreach (var Event in eventList)
            {
                Event.Hide();
            }
        }
        public void ShowPage()
        {
            foreach (var Event in eventList)
            {
                Event.Show();
            }
        }
    }

    public DateTime Today { get; private set; }
    private List<CalendarPage> pages;
    private const int pageToLoad = 3;
    private int pageIndex = 1;
    // Use this for initialization
    void Start()
    {
        pages = new List<CalendarPage>();
        DateTime realWorldTime = DateTime.Now;
        Today = DateTime.Today;
        //Create a few demo event and populate them
        System.Random ra = new System.Random();
        for (int j = -5; j < 5; j++)
        {
            CalendarPage page = new CalendarPage();
            for (int i = 0; i < 3; i++)
            {
                int a = ra.Next() % 18;
                int b = ra.Next() % 4 + 1;

                //Debug.Log("a" + a + " b " + b);
                DateTime TS = new DateTime(Today.Year, Today.Month, Today.Day + j, a, 0, 0);
                DateTime TE = new DateTime(Today.Year, Today.Month, Today.Day + j, a + b, 0, 0);
                var _event = new CalendarEvent(TS, TE, elementColor, UIElement);
                page.eventList.Add(_event);

            }
            page.HidePage();
            pages.Add(page);
            Debug.Log("page created");
        }

    }

    public void NextPage()
    {
        pages[pageIndex].HidePage();
        pageIndex++;
        pages[pageIndex].ShowPage();
    }

    public void PreviousPage()
    {
        pages[pageIndex].HidePage();
        pageIndex--;
        pages[pageIndex].ShowPage();
    }


}
