using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TablePlacement : MonoBehaviour
{

    private int[] cubeLocations = {-1, -1, -1, -1};
    private bool isTaskFinished = false, isObjectHeld = false;

    private int cubeToMove;

    private int ringToMoveFrom;

    private int ringToMoveTo;

    private Transform[] cubeTransforms;
    public Transform[] ringTransforms;
    // private Transform ringTransform0;
    // private Transform ringTransform1;
    // private Transform ringTransform2;
    // private Transform ringTransform3;


    public Material ringOn, ringOff;

    
    public GameObject Camera;

    public GameObject Main;
    public float distance = 0.5f;
    public float heightAdjustment = -1f;
    public int angle = 30;

    public float angleOffset = 0f;
    public float radius = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        cubeTransforms = new Transform[2];
        cubeTransforms[0] = transform.Find("Cube 0");
        cubeTransforms[1] = transform.Find("Cube 1");

        ringTransforms = new Transform[4];
        ringTransforms[0] = transform.Find("Ring 0");
        ringTransforms[1] = transform.Find("Ring 1");
        ringTransforms[2] = transform.Find("Ring 2");
        ringTransforms[3] = transform.Find("Ring 3");

        ringTransforms[0].gameObject.GetComponent<MeshRenderer>().material = ringOff;
        ringTransforms[1].gameObject.GetComponent<MeshRenderer>().material = ringOff;
        ringTransforms[2].gameObject.GetComponent<MeshRenderer>().material = ringOff;
        ringTransforms[3].gameObject.GetComponent<MeshRenderer>().material = ringOff;


        
        if (Camera == null)
        {
            Debug.LogError("Assign GameObjects in the inspector!");
            return;
        }
    }

    // Recenter the table around the user
    
    // void ResetTable() {
    //     Vector3 forwardProjection = new Vector3(Camera.transform.forward.x, 0f, Camera.transform.forward.z).normalized;
    //     Vector3 tablePos = Camera.transform.position + forwardProjection * distance + new Vector3(0f, heightAdjustment, 0f);
    //     transform.position = tablePos;
    //     transform.LookAt(new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z));

    //     float anglerad = angle * Mathf.Deg2Rad;


    //     cubeTransforms[0].localPosition = new Vector3(0f, 1f, 0.5f) + radius*(new Vector3(Mathf.Sin(anglerad), 0f, -Mathf.Cos(anglerad)));
    //     cubeTransforms[1].localPosition = new Vector3(0f, 1f, 0.5f) + radius*(new Vector3(-Mathf.Sin(anglerad), 0f, -Mathf.Cos(anglerad))); 

    //     cubeTransforms[0].LookAt(new Vector3(Camera.transform.position.x, cubeTransforms[0].position.y, Camera.transform.position.z));
    //     cubeTransforms[1].LookAt(new Vector3(Camera.transform.position.x, cubeTransforms[1].position.y, Camera.transform.position.z));

    //     cubeTransforms[0].Rotate(new Vector3(0f, 1f, 0f), -angleOffset);
    //     cubeTransforms[1].Rotate(new Vector3(0f, 1f, 0f), -angleOffset);
    // }

    public void ResetTableDemo() {
        
        // Reset the table
        Vector3 forwardProjection = new Vector3(Camera.transform.forward.x, 0f, Camera.transform.forward.z).normalized;
        Vector3 tablePos = Camera.transform.position + forwardProjection * distance + new Vector3(0f, heightAdjustment, 0f);
        transform.position = tablePos;
        transform.LookAt(new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z));
    
        // Place cubes onto rings 1 and 2
        cubeTransforms[0].localPosition = ringTransforms[0].localPosition + new Vector3(0f, 1f, 0f);
        cubeTransforms[1].localPosition = ringTransforms[1].localPosition + new Vector3(0f, 1f, 0f);

        cubeTransforms[0].LookAt(new Vector3(Camera.transform.position.x, cubeTransforms[0].position.y, Camera.transform.position.z));
        cubeTransforms[1].LookAt(new Vector3(Camera.transform.position.x, cubeTransforms[1].position.y, Camera.transform.position.z));

        cubeTransforms[0].Rotate(new Vector3(0f, 1f, 0f), -angleOffset);
        cubeTransforms[1].Rotate(new Vector3(0f, 1f, 0f), -angleOffset);

        cubeLocations[0] = 0;
        cubeLocations[1] = 1;
    }


    // Actual demo
    public void Demo() {
    
        StartCoroutine(RepeatTimes(3));
        
    }

    IEnumerator RepeatTimes(int times)
    {
        for (int i = 0; i < times; i++)
        {
            SelectRingFrom();
        
            while (!isObjectHeld) {
                yield return new WaitForSeconds(0.1f);
                isObjectHeld = Main.GetComponent<MainLoop>().holdingObjectMode;
            }
            isObjectHeld = false;

            SelectRingTo();

            while (!isTaskFinished) {
                yield return new WaitForSeconds(0.1f);
                Vector3 cubePos = cubeTransforms[cubeToMove].position;
                Vector3 ringPos = ringTransforms[ringToMoveTo].position;
                cubePos.y = 0f;
                ringPos.y = 0f;
                isTaskFinished = (!Main.GetComponent<MainLoop>().holdingObjectMode && Vector3.Distance(cubePos, ringPos) < 0.05f);
            }
            isTaskFinished = false;
            ResetRings();
            yield return new WaitForSeconds(1f);
            cubeTransforms[cubeToMove].position = new Vector3(ringTransforms[ringToMoveTo].position.x, cubeTransforms[cubeToMove].position.y, ringTransforms[ringToMoveTo].position.z);
            cubeTransforms[cubeToMove].rotation = ringTransforms[ringToMoveTo].rotation;


            float delay = Random.Range(1f, 3f);
            yield return new WaitForSeconds(delay);
        }
    }

    
    void SelectRingFrom() {

        // Pick a cube and identify the ring it is on
        cubeToMove = Random.Range(0, 2);
        ringToMoveFrom = System.Array.IndexOf(cubeLocations, cubeToMove);

        // Other cubes location
        int otherCube = 1 - cubeToMove;
        int otherRing = System.Array.IndexOf(cubeLocations, otherCube);
        
        // Empty rings
        int[] freeRings = Enumerable.Range(0, 4).Except(new int[] {ringToMoveFrom, otherRing}).ToArray();

        // Randomly pick a ring to move to
        ringToMoveTo = freeRings[Random.Range(0, freeRings.Length)];

        // Move the cube
        Debug.Log("Moving cube " + cubeToMove + " from ring " + ringToMoveFrom + " to ring " + ringToMoveTo);
        
        ringTransforms[ringToMoveFrom].gameObject.GetComponent<MeshRenderer>().material = ringOn;

    }

    void SelectRingTo()
    {
        ringTransforms[ringToMoveFrom].gameObject.GetComponent<MeshRenderer>().material = ringOff;
        ringTransforms[ringToMoveTo].gameObject.GetComponent<MeshRenderer>().material = ringOn;


    }

    void ResetRings() {
        ringTransforms[ringToMoveTo].gameObject.GetComponent<MeshRenderer>().material = ringOff;
        cubeLocations[ringToMoveFrom] = -1;
        cubeLocations[ringToMoveTo] = cubeToMove;

        
    }
}
