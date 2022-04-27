using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmallButton
{
    public string name;
    public BaseDemo.ButtonRun action;
    public SmallButton(string name, BaseDemo.ButtonRun action)
    {
        this.name = name;
        this.action = action;
    }
}

public abstract class BaseDemo : MonoBehaviour {

    public delegate void ButtonRun();

    private int count;
    private List<string> logs = new List<string>() { };
    int maxLogsCount = 30;

    //int startPaymentIndex = 0;
    int buttonWith = 500;
    int buttonHeight = 80;
    string myGameContentVersion = "1.0";

    static public bool onFront = true;

    int screenWidth;
    int screenHeight;
    private Vector2 btnScrollPosition;
    private Vector2 logScrollPosition;
    private int totalScrollHeight = 1000;

    protected List<SmallButton> buttons = new List<SmallButton>();

    protected abstract void InnerStart();

    void Start()
    {
        screenWidth = UnityEngine.Screen.width;
        screenHeight = UnityEngine.Screen.height;
#if !UNITY_EDITOR
    	buttonWith = screenWidth / 2 ;
    	buttonHeight = screenHeight / 13;
#endif
        btnScrollPosition = Vector2.zero;
        logScrollPosition = Vector2.zero;

        //the sdkOrderIds should save in the local or cloud
        HashSet<string> hasSendedSdkOrders = new HashSet<string>();

        InnerStart();

    }


    void OnGUI()
    {
        if (!onFront)
        {
            //if it is not on front stop draw the gui
            //Debug.Log("no on front return");
            return;
        }
        while (logs.Count > maxLogsCount)
        {
            logs.RemoveAt(0);
        }

        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;
        fontStyle.normal.textColor = new Color(1, 1, 1);
        fontStyle.fontSize = 30;
        fontStyle.wordWrap = true;

        GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.alignment = TextAnchor.MiddleCenter;
        btnStyle.fontSize = 28;
        btnStyle.normal.textColor = Color.white;
        btnStyle.wordWrap = true;


        string l = "";
        for (int i = logs.Count - 1; i >= 0; i--)
        {
            l += logs[i] + "\n*************\n";
        }

        logScrollPosition = GUI.BeginScrollView(new Rect(5, 0, Screen.width - 5, Screen.height / 2), logScrollPosition, new Rect(5, 0, Screen.width - 10, 2000), false, false);
        GUI.Label(new Rect(5, 0, Screen.width - 15, Screen.height / 2), l, fontStyle);
        GUI.EndScrollView();


        int y = Screen.height / 2 + 5;

        //GUI.Label(new Rect(buttonWith + 10, y + 10, Screen.width - buttonWith - 20, 150), playerItemStr, fontStyle);

        btnScrollPosition = GUI.BeginScrollView(new Rect(0, y, Screen.width, Screen.height - Screen.height / 2), btnScrollPosition, new Rect(0, y, Screen.width, totalScrollHeight), false, false);

        foreach(var one in buttons)
        {
            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), one.name, btnStyle))
            {
                one.action();
            }
        }


        GUI.EndScrollView();

        totalScrollHeight = y + 100;


    }

    protected void GuiLog(string log)
    {
        logs.Add(log);
    }
    protected void AddButtion(string name, BaseDemo.ButtonRun action)
    {
        buttons.Add(new SmallButton(name, action));
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.position.y < Screen.height / 4 * 3)
                {
                    btnScrollPosition.y += touch.deltaPosition.y;
                }
                else
                {
                    logScrollPosition.x -= touch.deltaPosition.x;
                    logScrollPosition.y += touch.deltaPosition.y;
                }
            }
        }
    }
}