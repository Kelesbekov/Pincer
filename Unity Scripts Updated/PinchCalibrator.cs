using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchCalibrator : MonoBehaviour
{
    public GameObject childThumbObject, childIndexObject;

    void Start()
    {
        // Make sure to assign your objects in the Unity Editor
        if (childThumbObject == null || childIndexObject == null)
        {
            Debug.LogError("Assign GameObjects in the inspector!");
            return;
        }
    }



    public void ApplyPositionOffset()
    {
        Vector3 localPos = childIndexObject.transform.parent.transform.InverseTransformPoint(transform.position);
        Debug.Log(localPos);
        childIndexObject.transform.localPosition = localPos;
        Vector3 localPos2 = childThumbObject.transform.parent.transform.InverseTransformPoint(transform.position);
        Debug.Log(localPos2);
        childThumbObject.transform.localPosition = localPos2;
    }


}
