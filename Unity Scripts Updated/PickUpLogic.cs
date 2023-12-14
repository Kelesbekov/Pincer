using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    public GameObject MainObject;
    public GameObject Table;
    private Transform cubeTransform0;
    private Transform cubeTransform1;

    // Far proximity distance (to start controller motion)
    public float farProximityDistance = 0.1f;

    // Close proximity distance (to initiate logic for grabbing object)
    public float closeProximityDistance = 0.03f;

    public int defaultArmPWM = 40;
    private int ObjectIdx = -1;



    // Start is called before the first frame update
    void Start()
    {
        MainObject = GameObject.Find("Main");
        Table = GameObject.Find("Table");
        cubeTransform0 = Table.transform.Find("Cube 0");
        cubeTransform1 = Table.transform.Find("Cube 1");

    }


    // Update is called once per frame
    void Update()
    {

        isFarProximate();
        isCloseProximate(ObjectIdx);

        if (MainObject.GetComponent<MainLoop>().grippingMode == true &&
            MainObject.GetComponent<MainLoop>().closeProximityMode == true)
        {
            MainObject.GetComponent<MainLoop>().holdingObjectModeOn();

            //Debug.Log("Grabbing object " + MainObject.GetComponent<MainLoop>().objectIdx);
            
            if (MainObject.GetComponent<MainLoop>().objectIdx == 0) {
                
                if (cubeTransform0.GetComponent<Rigidbody>() != null) {
                    cubeTransform0.GetComponent<Rigidbody>().isKinematic = true;
                }
                //Debug.Log("Controller Rotation: " + transform.rotation.eulerAngles);
                //Debug.Log("Cube Rotation: " + cubeTransform0.rotation.eulerAngles);
                cubeTransform0.position = transform.position;
                cubeTransform0.rotation = transform.rotation;  
                 
            }
            else if (MainObject.GetComponent<MainLoop>().objectIdx == 1) {
                
                if (cubeTransform1.GetComponent<Rigidbody>() != null) {
                    cubeTransform1.GetComponent<Rigidbody>().isKinematic = true;
                }
                
                //Debug.Log("Controller Rotation: " + transform.rotation.eulerAngles);
                //Debug.Log("Cube Rotation: " + cubeTransform1.rotation.eulerAngles);
                cubeTransform1.position = transform.position;
                cubeTransform1.rotation = transform.rotation;
            }   
        }
        else {
            MainObject.GetComponent<MainLoop>().holdingObjectModeOff();

            if (cubeTransform0.GetComponent<Rigidbody>() != null) {
                    cubeTransform0.GetComponent<Rigidbody>().isKinematic = false;
                }
            if (cubeTransform1.GetComponent<Rigidbody>() != null) {
                    cubeTransform1.GetComponent<Rigidbody>().isKinematic = false;
                }
        }
        
    }

   
    // Far proximity to virtual objects
    void isFarProximate() {
        
        float distanceToCube0 = Vector3.Distance(transform.position, cubeTransform0.position);
        float distanceToCube1 = Vector3.Distance(transform.position, cubeTransform1.position);
        
        // if we are not close to either cube, we are not in far proximity mode
        if (distanceToCube0 > farProximityDistance && distanceToCube1 > farProximityDistance) {
            ObjectIdx = -1;
            MainObject.GetComponent<MainLoop>().farProximityModeOff();
            MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
            return;
        }
        // if we are close to one of the cubes, we are in far proximity mode
        else {
            MainObject.GetComponent<MainLoop>().farProximityModeOn();
            // if we haven't picked a cube to focus on, pick one (in order of index)
            if (ObjectIdx == -1) {
                if (distanceToCube0 < farProximityDistance) {
                    ObjectIdx = 0;
                    MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
                }
                else {
                    ObjectIdx = 1;
                    MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
                }
            }
            // if we are focusing on cube 0, only check it's distance
            else if (ObjectIdx == 0) {
                if (distanceToCube0 > farProximityDistance)
                {
                    if (distanceToCube1 < farProximityDistance) {
                        ObjectIdx = 1;
                        MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
                    }
                    else {
                        ObjectIdx = -1;
                        MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
                    }
                }
            }
            // if we are focusing on cube 1, only check it's distance
            else if (ObjectIdx == 1) {
                if (distanceToCube1 > farProximityDistance)
                {
                    if (distanceToCube0 < farProximityDistance) {
                        ObjectIdx = 0;
                        MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
                    }
                    else {
                        ObjectIdx = -1;
                        MainObject.GetComponent<MainLoop>().objectSelector(ObjectIdx);
                    }
                }
            }
        }
    }

    void isCloseProximate(int ObjectIdx) {
        if (ObjectIdx == 0) {
            if (Vector3.Distance(transform.position, cubeTransform0.position) < closeProximityDistance) {
                MainObject.GetComponent<MainLoop>().closeProximityModeOn();
            }
            else {
                MainObject.GetComponent<MainLoop>().closeProximityModeOff();
            }
        }
        else if (ObjectIdx == 1) {
            if (Vector3.Distance(transform.position, cubeTransform1.position) < closeProximityDistance) {
                MainObject.GetComponent<MainLoop>().closeProximityModeOn();
            }
            else {
                MainObject.GetComponent<MainLoop>().closeProximityModeOff();
            }
        }
    }
    
    
    public (int, int) controllerBehaviour(string mode) {
        float distanceToCube;
        if (ObjectIdx == -1) {
            return (-1, -1);
        }
        else if (ObjectIdx == 0) {
            distanceToCube = Vector3.Distance(transform.position, cubeTransform0.position);
        }
        else if (ObjectIdx == 1) {
            distanceToCube = Vector3.Distance(transform.position, cubeTransform1.position);
        }
        else {
            return (-1, -1);
        }

        float stepRatio = 0.5f;
        float stepDistance = Mathf.Lerp(closeProximityDistance, farProximityDistance, stepRatio);
        if (mode == "CC1"){
            int armPWM = defaultArmPWM;
            int gripperPWM = (distanceToCube > stepDistance) ? 1 : 0;
            return (armPWM, gripperPWM);
        }
        else if (mode == "CC2"){
            return (1, 0);
        }
        else if (mode == "CC3"){
            return (0, 0);
        }
        else if (mode == "ETHD"){
            return (1, 1);
        }
        else {
            return (-1, -1);
        }
    }
    
}
