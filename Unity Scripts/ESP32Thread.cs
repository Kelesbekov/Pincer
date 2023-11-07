using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System;

public class ESP32Thread : MonoBehaviour
{
    [SerializeField] public string portName;
    private Thread thr;
    private Queue outputQueue;
    private Queue inputQueue;
    private SerialPort stream;
    public bool looping = true;
    private int counter = 0;
    public void StopThread()
    {
        lock (this)
        {
            looping = false;
        }
    }

    public bool IsLooping ()
    {
        lock (this)
        {
            return looping;
        }
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
            counter = counter + 1;
            Debug.Log(counter);
            if (counter == 100)
            {
                StopThread();
            }
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
    public void Start()
    // Thread and Queue initialisation
    {
        //Debug.Log("flag");
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());

        thr = new Thread(ThreadLoop);
        thr.Start();
    }

    public void ThreadLoop()
    // Main Loop for the thread
    {
        // Define SerialPort name and BAUD rate, open the stream
        stream = new SerialPort(portName, 9600);
        stream.ReadTimeout = 50;
        stream.Open();

    while (IsLooping())
        // Run the loop
        {
            // Check for items in the output queue - dequeue and send over the serial port
            if (outputQueue.Count != 0)
            {
                string command = (string)outputQueue.Dequeue();
                SendOutgoingMessage(command);
            }

            // 
            string result = ReadIncomingMessage(stream.ReadTimeout);
            if (result != null)
            {
                inputQueue.Enqueue(result);
            }
        }
        stream.Close();
    }
}
