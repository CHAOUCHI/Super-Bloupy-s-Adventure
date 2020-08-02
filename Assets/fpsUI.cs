using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsUI : MonoBehaviour
{
    public Text fps;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString() + " fps";   //nb de frames ecouler en 1 secondes
    }
}
