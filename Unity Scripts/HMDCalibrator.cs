using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDCalibrator : MonoBehaviour
{
    public GameObject HMDobject;
    public Transform childTransform;

    void Start()
    {
        if (HMDobject == null)
        {
            Debug.LogError("Assign GameObjects in the inspector!");
            return;
    }
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //Debug.Log("hey");
            //InverseCameraTransform();
            ApplyPositionOffset();
            InverseCameraTransform();
        }
        // Check for key press (in this case, the "3" key)
        // if (Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     //Debug.Log("hey");
            
        //     //ApplyPositionOffset();
        // }

          
        // if (Input.GetKeyDown(KeyCode.Alpha5))
        // {
        //     Debug.Log("Applying Camera Transform");
        //     //InverseCameraTransform();
        //     ApplyCameraTransform();
        // }
    }


void ApplyPositionOffset()
{
    Vector3 HMDpos = HMDobject.transform.position;
    Quaternion HMDrot = HMDobject.transform.rotation;
    
    //Debug.Log("HMD distance: " + Vector3.Distance(HMDpos, Vector3.zero));
    //Debug.Log("parent: " + Vector3.Distance(transform.position, Vector3.zero));
    transform.position += HMDpos;
    transform.rotation *= HMDrot;
    //Debug.Log("new child: " + Vector3.Distance(childTransform.position, Vector3.zero));
    //Debug.Log("new parent: " + Vector3.Distance(transform.position, Vector3.zero));
    

}

void InverseCameraTransform()
{
    
    //
    Quaternion invertedRotation = Quaternion.Inverse(childTransform.rotation) * transform.rotation;
    transform.rotation *= invertedRotation;

    Vector3 invertedPosition = transform.position - childTransform.position;
    transform.position += invertedPosition;
    //Debug.Log(Vector3.Distance(invertedPosition, Vector3.zero));
    //Debug.Log(invertedPosition);

    //Debug.Log(Vector3.Distance(childTransform.localPosition, Vector3.zero));
    //Debug.Log(childTransform.localPosition);

    
    
    

}

// void ApplyCameraTransform() {

//     Quaternion rotationDelta = Quaternion.FromToRotation(childTransform.rotation.eulerAngles, HMDobject.transform.rotation.eulerAngles);
//     Debug.Log("old parent: " + transform.rotation.eulerAngles);
//     transform.rotation *= rotationDelta;
//     Debug.Log("new parent: " + transform.rotation.eulerAngles);
//     Debug.Log("difference: " + rotationDelta.eulerAngles);
//     Debug.Log("new child: " + new Vector3(childTransform.rotation.x, childTransform.rotation.y, childTransform.rotation.z));
//     Debug.Log("HMD: " + new Vector3(HMDobject.transform.rotation.x, HMDobject.transform.rotation.y, HMDobject.transform.rotation.z));
// }

}