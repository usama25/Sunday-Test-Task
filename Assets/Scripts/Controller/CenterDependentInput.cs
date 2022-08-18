using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterDependentInput : PointerInput
{

    public  float   value { get; private set; }
    private Vector2 direction = Vector2.zero;
    private Vector2 center    = Vector2.zero;
    private float   previous  = 0;


    public CenterDependentInput()
    {
        OnPointerDown   += PointerDown;
        OnPointerUp     += PointerUp;
        OnPointerUpdate += PointerUpdate;
    }

    void PointerDown(Vector3 mousePosition)
    {
        value     = 0;
        direction = Vector3.zero;
        center    = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }

    void PointerUp(Vector3 mousePosition)
    {
        direction = Vector3.zero;
        value     = 0;
    }

    void PointerUpdate(Vector3 mousePosition)
    {
        direction = ((Vector2) mousePosition - this.center) / (Screen.height);
        var degree = Mathf.Atan2(direction.x, direction.y);
        degree = Mathf.Deg2Rad * degree;
        if (Mathf.Abs(degree - previous) == 0f)
        {
            value = 0;
            return;
        }

        value    = Mathf.Sign(degree - previous);
        previous = degree;
    }

}

