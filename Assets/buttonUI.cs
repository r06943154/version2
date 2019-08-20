using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buttonUI : MonoBehaviour {

    [DllImport("Lab_Final.dll", EntryPoint = "Call1", CallingConvention = CallingConvention.Cdecl)]

    private static extern void Call1(bool matlab);
    
    //[UnmanagedFunctionPointer(CallingConvention.StdCall)]

    [DllImport("LVS_Back.dll", EntryPoint = "Call2", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Call2(string a, string b, double cE, double lE, double mc, double ml, string c);

    public Button btn1, btn2, btn3;
    public Text btn1t, btn2t;
    public Camera cam;
    public Toggle Matlab;

    public Text cError, lError, minC, minL;
    double cE, lE, Mc, Ml;
    int bt1Timer = 0, bt2Timer = 0;
    // Use this for initialization
    void Start () {
        /*btn1 = GetComponent<Button>();
        btn2 = GetComponent<Button>();
        btn3 = GetComponent<Button>();*/
        cam = GetComponent<Camera>();
        //Matlab = GetComponent<Toggle>();
    }
	
	// Update is called once per frame
	void Update () {
        /*if (bt1Timer > 0)
        {
            //btn1.interactable = false;
            bt1Timer--;
            
        }
        else
        {
            btn1.interactable = true;
        }
        if (bt2Timer > 0)
        {
            //btn2.interactable = false;
            bt2Timer--;

        }
        else
        {
            btn2.interactable = true;
        }
        Debug.Log(bt1Timer);*/
    }

    public void Button1()
    {
        //Debug.Log(enter1.text + " " + enter2.text);
        Call1(Matlab.isOn);
        btn1t.text = "LVS前段 done";
        //btn1.gameObject.SetActive(false);
        if (Matlab.isOn)
        {
            Matlab.isOn = false;
            Matlab.interactable = false;
        }
        bt1Timer = 600;
        Debug.Log(bt1Timer);
        //btn1.interactable = false;
    }

    public void Button2()
    {
        string outd = "";
        string net = "";
        string layout = "";
        cE = Convert.ToDouble(cError.text);
        lE = Convert.ToDouble(lError.text);
        Mc = Convert.ToDouble(minC.text);
        Ml = Convert.ToDouble(minL.text);
        Call2(net, layout, cE, lE, Mc, Ml, outd);
        btn2.interactable = false;
        btn2t.text = "LVS後段 done";
        bt2Timer = 300;
        //btn2.gameObject.SetActive(false);
    }
    public void Button3()
    {
        btn3.interactable = false;
        btn1.gameObject.SetActive(false);
        btn2.gameObject.SetActive(false);
        btn3.gameObject.SetActive(false);
        transform.Translate(new Vector3(1360, 0, 0));
        //Application.LoadLevel("mainScene");
        SceneManager.LoadScene("mainScene");
    }
    
}
