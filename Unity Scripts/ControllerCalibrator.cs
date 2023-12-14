using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCalibrator : MonoBehaviour
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

    void Update()
    {
        // Check for key press (in this case, the "2" key)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ApplyPositionOffset();
        }
    }

    void ApplyPositionOffset()
    {
        Vector3 indexPosition = childIndexObject.transform.position;
        Vector3 thumbPosition = childThumbObject.transform.position;
        Vector3 midPoint = Vector3.Lerp(indexPosition, thumbPosition, 0.5f);

        Transform childController = transform.Find("Controller");
        //Debug.Log(childController.position);
        childController.localPosition = transform.InverseTransformPoint(midPoint);

        //Debug.Log(childController.position);
    }
}
