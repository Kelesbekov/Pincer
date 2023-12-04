using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainLoop : MonoBehaviour
{
    public GameObject ESP32Communicator;
    public ESP32Thread scriptComponent;

    public GameObject TableObject;
    public TablePlacement tablePlacementScript;

    public GameObject pinchCalibratorObject;
    public PinchCalibrator pinchCalibratorScript;

    public GameObject controllerCalibratorObject;
    public ControllerCalibrator controllerCalibratorScript;

    public GameObject HMDObject;
    public HMDCalibrator HMDScript;
    
    // Game Logic variables
    public bool farProximityMode = false;
    public bool closeProximityMode = false;
    public bool grippingMode = false;
    public bool holdingObjectMode = false;
    public int objectIdx = -1;

    // Communication variables
    private string receivedMessage;
    
    // Debug variables
    private int counter = 0;

    
    void Start()
    {
        tablePlacementScript = TableObject.GetComponent<TablePlacement>(); 
        //ESP32Thread = GameObject.Find("GameObject");
        scriptComponent = ESP32Communicator.GetComponent<ESP32Thread>();
        
        pinchCalibratorScript = pinchCalibratorObject.GetComponent<PinchCalibrator>();

        controllerCalibratorScript = controllerCalibratorObject.GetComponent<ControllerCalibrator>();

        HMDScript = HMDObject.GetComponent<HMDCalibrator>();

        if (scriptComponent != null)
        {
            scriptComponent.StartThread();
        }

        // 10 Hz
        //InvokeRepeating("SendMessage", 0, 1f);
        
        StartCoroutine(CounterDisplay());


    }
   
  

    // Update is called once per frame
    void FixedUpdate()
    {
        
       receivedMessage = scriptComponent.DequeueIncomingMessage();

       if (receivedMessage != null)
        {
            // Debug.Log(receivedMessage);
            if (receivedMessage == "44") {
                //Debug.Log("Input message dequeued and used: " + receivedMessage);
                grippingModeOn();
            }
            else {
                grippingModeOff();
            }
            SendMessage();
            counter+=1;
        }
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pinchCalibratorScript.ApplyPositionOffset();
        }
        if 
        (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Calibrate the controller arm position
            // TO BE WRITTEN
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            controllerCalibratorScript.ApplyPositionOffset();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HMDScript.ApplyPositionOffset();
            HMDScript.InverseCameraTransform();
        }


        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            tablePlacementScript.ResetTableDemo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            tablePlacementScript.Demo();
        }
    }

    void SendMessage() {
        string message = "" + (farProximityMode ? 1 : 0) + "," + objectIdx;
        //scriptComponent.CheckOutputQueue();
        scriptComponent.EnqueueOutgoingMessage(message);
    }

        
    public void objectSelector(int idx) {
        objectIdx = idx;
    }
    public void farProximityModeOn() {
        farProximityMode = true;
    }
    
    public void farProximityModeOff() {
        farProximityMode = false;
    }

    public void closeProximityModeOn() {
        closeProximityMode = true;
    }

    public void closeProximityModeOff() {
        closeProximityMode = false;
    }
    
    public void grippingModeOn() {
        grippingMode = true;
    } 

    public void grippingModeOff() {
        grippingMode = false;
    }
    
    public void holdingObjectModeOn() {
        holdingObjectMode = true;
        //Debug.Log("holdingObjectOn");
    }

    public void holdingObjectModeOff() {
        holdingObjectMode = false;
        //Debug.Log("holdingObjectOff");
    }


    IEnumerator CounterDisplay()
    {
        while (true)
        {
             Debug.Log("Thread Counter: " + scriptComponent.counter);
             Debug.Log("FixedUpdate Counter: " + counter);
            counter = 0;
            scriptComponent.ResetCounter();
             scriptComponent.CheckOutputQueue();
             scriptComponent.CheckInputQueue(); 
            yield return new WaitForSeconds(1.0f);
        }
        

    }
}
