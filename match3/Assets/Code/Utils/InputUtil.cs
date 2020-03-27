using UnityEngine;

public static class InputUtil
{
    public static bool GetInputDown()
    {
        if (Input.mousePresent)
            return Input.GetMouseButtonDown(0);

        return Input.touchCount > 0 &&
               Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public static bool GetInputUp()
    {
        if (Input.mousePresent)
            return Input.GetMouseButtonUp(0);

        return Input.touchCount > 0 &&
               Input.GetTouch(0).phase == TouchPhase.Ended;
    }

    public static Vector2 GetInputPosition()
    {
        if (Input.mousePresent)
            return Input.mousePosition;

        return Input.touchCount > 0 ? Input.GetTouch(0).position : Vector2.zero;
    }
}