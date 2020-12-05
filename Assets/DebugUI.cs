using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Text second;
    private animationHandlerBloupy animBloubpyScript;
    private Touch touche;

    private void Start()
    {
        animBloubpyScript = GetComponent<animationHandlerBloupy>();
    }

    void Update()
    {
        String textOutPut = "TOUCHE : ";
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touchCount < 1) textOutPut += "FALSE";
            textOutPut += i +"_" + Input.touches[i].position.ToString() + " ";
           // Debug.DrawLine(Vector3.zero,Camera.main.ScreenToWorldPoint(Input.touches[i].position),Color.red,0f,true);
            
        }
        /*if (Input.touchCount > 0)
        {
            textOutPut = "Touche : " + Input.GetTouch(0).position.ToString();
            if(Input.GetTouch(1))
            textOutPut += Input.GetTouch(1).position.ToString();
        }
        else
        {
            textOutPut = " NO TOUCH";
        }*/

        second.text = textOutPut;
        //   (Convert.ToInt32(Time.time)).ToString()
    }


}
