using UnityEngine;
using System.Collections;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public class touchHandle : MonoBehaviour
{
    public RightScreenHandle righttouch;
    public LeftScreenHandle lefttouch;
    // Use this for initialization
    void Start()
    {
        lefttouch = GameObject.Find("Bloupy").GetComponent<LeftScreenHandle>();
        righttouch = GameObject.Find("Bloupy").GetComponent<RightScreenHandle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Touch[] GetTouches()
    {
        if (Input.touchCount > 0)
        {
            return Input.touches;
        }
        return null;
    }

    public bool IsInLeftSide(Vector2 pos)
    {
        if (pos.x < Screen.width / 2)
            return true;
        else
            return false;
    }

    public bool IsInRightSide(Vector2 pos)
    {
        if (pos.x >= Screen.width / 2)
            return true;
        else
            return false;
    }

    /*Test Input.touchCount > 1 before runing this method*/
    public Touch GetLastTouch()     
    {
        return GetTouches()[Input.touchCount - 1];
    }
}
