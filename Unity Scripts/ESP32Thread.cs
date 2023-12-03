using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System;
using JetBrains.Annotations;
using static UnityEditor.Progress;

public class ESP32Thread : MonoBehaviour
{
    [SerializeField] public string portName;
    public int baudRate;
    private Thread thr;
    private Queue outputQueue;
    private Queue inputQueue;
    private SerialPort stream;
    public bool looping = true;
    public int counter = 0;

    public void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());
        Debug.Log("Queues initialised.");
        thr = new Thread(ThreadLoop);
        thr.Start();
        Debug.Log("Thread initialised.");
    }

    public bool HasQueue()
    {
        return outputQueue != null && inputQueue != null;
    }

    // public void StopThread()
    // {
    //     lock (this)
    //     {
    //         looping = false;
    //     }
    // }

    // public bool IsLooping ()
    // {
    //     lock (this)
    //     {
    //         return looping;
    //     }
    // }

    public void ResetCounter()
    {
        //lock (this)
        //{
            counter = 0;
        //}
        
    }
    
    public void SendOutgoingMessage(string message)
    // Send Data to ESP32 over the serial port
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush(); 
    }
    
    public void EnqueueOutgoingMessage(string command)
    // Enqueue the data to be send to ESP32
    {
        if (outputQueue.Count >= 5) {
            outputQueue.Dequeue();
        }
        outputQueue.Enqueue(command);
    }    

    public string ReadIncomingMessage(int timeout = 0)
    // Receive incoming data from ESP32
    {
        
        stream.ReadTimeout = timeout;
        try
        {
            return stream.ReadLine();
        }
        catch (TimeoutException e)
        {
            //counter = counter + 1;
            ////Debug.Log(counter);
            //if (counter == 100)
            //{
            //    StopThread();
            //}
            return null;
        }
    }
    public string DequeueIncomingMessage()
    // Dequeue received data from ESP32
    {
        if (inputQueue.Count == 0)
        {
            return null;
        }
        return (string) inputQueue.Dequeue();
    }
   

    public void CheckOutputQueue()
    {
        string x = "";
        if (outputQueue.Count != 0)
        {
                Debug.Log(outputQueue.Count + " item(s) in the output queue:");
                
                // foreach (string item in outputQueue)
                // {
                //     x = x + item;
                // }
                // //Debug.Log(x);
                // x = "";

            }
        else {
            Debug.Log("No items in the output queue");
        }
    }

    public void CheckInputQueue()
    {
        string x = "";
        if (inputQueue.Count != 0)
        {
                Debug.Log(inputQueue.Count + " item(s) in the input queue:");
                
                // foreach (string item in inputQueue)
                // {
                //     x = x + item;
                // }
                //Debug.Log(x);
                // x = "";

            }
        else {
            Debug.Log("No items in the input queue");
        }
    }


    public void ThreadLoop()
    // Main Loop for the thread
    {
        // Define SerialPort name and BAUD rate, open the stream
        stream = new SerialPort(portName, baudRate);
        stream.ReadTimeout = 0;
        stream.Open();
        inputQueue.Clear();
        outputQueue.Clear();
        
        while (looping)
        // Run the loop
        {   
            
            
            // Check for items in the output queue - dequeue and send over the serial port
            if (outputQueue.Count != 0)
            {
                
                //Debug.Log("sending and dequeuing");
                //CheckOutputQueue();
                string command = (string)outputQueue.Dequeue();
                counter += 1;
                
                //CheckOutputQueue();
                //Debug.Log(command);
                SendOutgoingMessage(command);
            }

            // 
            string result = ReadIncomingMessage(stream.ReadTimeout);
            if (result != null)
            {
                if (inputQueue.Count >= 5) {
                    inputQueue.Dequeue();
                }
                inputQueue.Enqueue(result);
                
            }
        }

        stream.Close();
    }
}
