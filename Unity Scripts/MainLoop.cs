using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject anotherObject;
    public ESP32Thread scriptComponent;
    private string receivedMessage;

    void Start()
    {
        anotherObject = GameObject.Find("GameObject");
        scriptComponent = anotherObject.GetComponent<ESP32Thread>();
        
        if (scriptComponent != null)
        {
            scriptComponent.StartThread();
        }
    }
   

    // Update is called once per frame
    void Update()
    {  
       //scriptComponent.CheckInputQueue();
       receivedMessage = scriptComponent.DequeueIncomingMessage();
       if (receivedMessage != null)
        {
            if (receivedMessage == "10") {
                Debug.Log("Input message dequeued and used: " + receivedMessage);
            }
            
        }
    }
}
