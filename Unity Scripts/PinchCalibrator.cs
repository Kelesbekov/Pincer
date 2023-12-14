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

    // Update is called once per frame
    void Update()
    {
        // Check for key press (in this case, the "1" key)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ApplyPositionOffset();
        }
    }

    void ApplyPositionOffset()
    {
        Vector3 localPos = childIndexObject.transform.parent.transform.InverseTransformPoint(transform.position);
        //Vector3 localPos1 = childIndexObject.transform.InverseTransformPoint(transform.position);
        Debug.Log(localPos);
        //Debug.Log(localPos1);

        childIndexObject.transform.localPosition = localPos;

        Vector3 localPos2 = childThumbObject.transform.parent.transform.InverseTransformPoint(transform.position);
        //Vector3 localPos1 = childIndexObject.transform.InverseTransformPoint(transform.position);
        Debug.Log(localPos2);
        //Debug.Log(localPos1);

        childThumbObject.transform.localPosition = localPos2;


        // var indexTip = indexObject.Find("Index Fingertip");
        // Capture the position of gameObject1
        // Vector3 indexFingerPosition = indexObject.transform.position;
        // Vector3 thumbFingerPosition = thumbObject.transform.position;

        // Vector3 pinchCalibratorPosition =  transform.position;

        // Vector3 thumbOffset = pinchCalibratorPosition - thumbFingerPosition;
        // Vector3 indexOffset = pinchCalibratorPosition - indexFingerPosition;

        // // Transform thumbFingertip = 
        // // Add the constant offset to the captured position
        // Vector3 newPosition = gameObject1Position + constantOffset;

        // // Set the new position for gameObject2
        // gameObject2.position = newPosition;

        // Debug.Log("Position offset applied. New position of gameObject2: " + newPosition);
    }


}
