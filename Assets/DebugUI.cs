using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Text second;
    private animationHandlerBloupy animBloubpyScript;

    private void Start()
    {
        animBloubpyScript = GetComponent<animationHandlerBloupy>();

    }

    void Update()
    {
        second.text =
             (Convert.ToInt32(Time.time)).ToString()
            + " Any Key:"+ Input.anyKey.ToString() 
            +" AxisHori:"+((int)Input.GetAxis("Horizontal")).ToString();      //Timer
    }


}
