using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainLoop : MonoBehaviour
{
    public GameObject ESP32Communicator;
    public ESP32Thread scriptComponent;


    public int hapticMode = -1;
    // Game Logic variables
    public bool proximityMode = false;
    private bool previousProximityMode = false;

    public bool grabMode = false;

    public bool holdingObjectMode = false;

    public int objectIdx = -1;

    private int previousObjectIdx = -1;

    // Communication variables
    private string receivedMessage;
    
    // Debug variables
    private int counter = 0;

    
    void Start()
    {
        //ESP32Thread = GameObject.Find("GameObject");
        scriptComponent = ESP32Communicator.GetComponent<ESP32Thread>();
        
        if (scriptComponent != null)
        {
            scriptComponent.StartThread();
        }

        // 10 Hz
        //InvokeRepeating("SendMessage", 0, 1f);
        
        StartCoroutine(Wait());


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
                grabModeOn();
            }
            else {
                grabModeOff();
            }
            SendMessage();
            counter+=1;
        }
        
        
        

    }

    void SendMessage() {
        string message = "" + (proximityMode ? 1 : 0) + "," + objectIdx;
        //scriptComponent.CheckOutputQueue();
        scriptComponent.EnqueueOutgoingMessage(message);
    }

    private void OnTransitionDetection() {

        if (proximityMode != previousProximityMode || objectIdx != previousObjectIdx) {
            //Debug.Log("Transition Detected");
            previousProximityMode = proximityMode;
            previousObjectIdx = objectIdx;
            SendMessage();
        }
    }
        
    public void objectSelector(int idx) {
        objectIdx = idx;
        //Debug.Log("objectSelector: " + objectIdx);
    }
    public void proximityModeOn() {
        proximityMode = true;
        //Debug.Log("proximityModeOn");
    }
    
    public void proximityModeOff() {
        proximityMode = false;
        //Debug.Log("proximityModeOff");
    }

    public void grabModeOn() {
        grabMode = true;
        //Debug.Log("grabModeOn");
    } 

    public void grabModeOff() {
        grabMode = false;
        //Debug.Log("grabModeOff");
    }
    
    public void holdingObjectModeOn() {
        holdingObjectMode = true;
        //Debug.Log("holdingObjectOn");
    }

    public void holdingObjectModeOff() {
        holdingObjectMode = false;
        //Debug.Log("holdingObjectOff");
    }


    IEnumerator Wait()
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
