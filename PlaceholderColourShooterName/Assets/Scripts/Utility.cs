using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {

    public static Vector3 GetWorldPointFromScreenPoint (Vector3 screenPoint, float height)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, height, 0f));
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
