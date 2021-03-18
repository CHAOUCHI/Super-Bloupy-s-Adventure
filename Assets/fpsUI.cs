using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsUI : MonoBehaviour
{
    public Text fps;
    private RightScreenHandle rightTouch;
    private LeftScreenHandle leftTouch;


    // Start is called before the first frame update
    void Start()
    {
        rightTouch = GameObject.Find("Bloupy").GetComponent<RightScreenHandle>();
        leftTouch = GameObject.Find("Bloupy").GetComponent<LeftScreenHandle>();

    }

    // Update is called once per frame
    void Update()
    {
            
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString() + " fps" ;   //nb de frames ecouler en 1 secondes
       // fps.text = Time.time.ToString() + " ";   //nb de frames ecouler en 1 secondes
    }
}
