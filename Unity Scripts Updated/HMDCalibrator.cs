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
  


public void ApplyPositionOffset()
{
    Vector3 HMDpos = HMDobject.transform.position;
    Quaternion HMDrot = HMDobject.transform.rotation;
    
    transform.position += HMDpos;
    transform.rotation *= HMDrot;


}

public void InverseCameraTransform()
{
    
    //
    Quaternion invertedRotation = Quaternion.Inverse(childTransform.rotation) * transform.rotation;
    transform.rotation *= invertedRotation;

    Vector3 invertedPosition = transform.position - childTransform.position;
    transform.position += invertedPosition;

    
    
    

}



}