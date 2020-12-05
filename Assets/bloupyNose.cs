using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloupyNose : MonoBehaviour
{
    private movementBloupy mouvementBloupy;

    // Start is called before the first frame update
    void Start()
    {
        mouvementBloupy = GetComponentInParent<movementBloupy>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ground")
        {
            mouvementBloupy.SetIsSplashMur(true);
 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            mouvementBloupy.SetIsSplashMur(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            mouvementBloupy.SetIsSplashMur(false);
        }
    }


}
