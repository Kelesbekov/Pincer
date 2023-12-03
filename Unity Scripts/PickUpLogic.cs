using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    public GameObject MainObject;
    public GameObject Table;
    private Transform cubeTransform0;

    public GameObject cubeObject0;

    public GameObject cubeObject1;
    private Transform cubeTransform1;
    public float distanceTrigger = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        MainObject = GameObject.Find("Main");
        Table = GameObject.Find("Table");
        cubeTransform0 = Table.transform.Find("Cube 0");
        cubeTransform1 = Table.transform.Find("Cube 1");
        cubeObject0 = cubeTransform0.gameObject;
        cubeObject1 = cubeTransform1.gameObject;

    }




    // Update is called once per frame
    void Update()
    {

        isProximate();

        if (MainObject.GetComponent<MainLoop>().grabMode == true &&
            MainObject.GetComponent<MainLoop>().proximityMode == true)
        {
            MainObject.GetComponent<MainLoop>().holdingObjectModeOn();

            //Debug.Log("Grabbing object " + MainObject.GetComponent<MainLoop>().objectIdx);
            
            if (MainObject.GetComponent<MainLoop>().objectIdx == 0) {
                
                if (cubeObject0.gameObject.GetComponent<Rigidbody>() != null) {
                    cubeObject0.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                //Debug.Log("Controller Rotation: " + transform.rotation.eulerAngles);
                //Debug.Log("Cube Rotation: " + cubeTransform0.rotation.eulerAngles);
                cubeTransform0.position = transform.position;
                cubeTransform0.rotation = transform.rotation;  
                 
            }
            else if (MainObject.GetComponent<MainLoop>().objectIdx == 1) {
                
                if (cubeObject1.gameObject.GetComponent<Rigidbody>() != null) {
                    cubeObject1.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                
                //Debug.Log("Controller Rotation: " + transform.rotation.eulerAngles);
                //Debug.Log("Cube Rotation: " + cubeTransform1.rotation.eulerAngles);
                cubeTransform1.position = transform.position;
                cubeTransform1.rotation = transform.rotation;
            }   
        }
        else {
            MainObject.GetComponent<MainLoop>().holdingObjectModeOff();

            if (cubeObject0.gameObject.GetComponent<Rigidbody>() != null) {
                    cubeObject0.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
            if (cubeObject1.gameObject.GetComponent<Rigidbody>() != null) {
                    cubeObject1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
        }
        
    }


    void isProximate() {
        if (Vector3.Distance(transform.position, cubeTransform0.position) < distanceTrigger) {
            
            MainObject.GetComponent<MainLoop>().objectSelector(0);
            MainObject.GetComponent<MainLoop>().proximityModeOn();
        }
        else if (Vector3.Distance(transform.position, cubeTransform1.position) < distanceTrigger) {
            MainObject.GetComponent<MainLoop>().objectSelector(1);
            MainObject.GetComponent<MainLoop>().proximityModeOn();
        }
        else {
            MainObject.GetComponent<MainLoop>().proximityModeOff();
            MainObject.GetComponent<MainLoop>().objectSelector(-1);
        }
    }
    // void OnTriggerEnter (Collider other) {
    //     Debug.Log(other.gameObject.name);
    //     MainObject.GetComponent<MainLoop>().proximityModeOn();
    //     if (other.gameObject.name == "Cube 0") {
    //        MainObject.GetComponent<MainLoop>().objectSelector(0);
    //     }
    //     else if (other.gameObject.name == "Cube 1") {
    //        MainObject.GetComponent<MainLoop>().objectSelector(1);
    //     }
    // }

    // void OnTriggerExit (Collider other) {
    //     MainObject.GetComponent<MainLoop>().proximityModeOff();
    //     MainObject.GetComponent<MainLoop>().objectSelector(-1);
    // }
}
