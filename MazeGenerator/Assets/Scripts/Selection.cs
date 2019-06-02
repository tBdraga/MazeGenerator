using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private int clickCounter = 0;
    public Material materialStartPoint;
    public Material materialEndPoint;
    public Vector3 spawnPosition;
    public Vector3 finishPosition;
    public GameObject spawnedBenza;
    //object that contains pathfinding script
    public GameObject pathFinder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawOnClick();
    }

    public void drawOnClick() {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.name == "Floor(Clone)" && clickCounter<2)
                {
                    //Setam material on-Click
                    //Se pot seta cel mult doua puncte
                    //Debug.Log("It's working!");
                    //Setam material pentru StartPoint
                    if (clickCounter == 0) {
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = materialStartPoint;
                        spawnPosition = hitInfo.transform.position;
                    }

                    //Setam material pentru EndPoint
                    if (clickCounter == 1) {
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = materialEndPoint;
                        finishPosition = hitInfo.transform.position;
                        spawnObject();
                        tracePath();
                    }

                    //Click counter creste. Maxim doua clickuri
                    clickCounter++;
                }
                else
                {
                    //Debug.Log("nopz");
                }
            }
            else
            {
               // Debug.Log("No hit");
            }
            //Debug.Log("Mouse is down");
        }
    }

    public void spawnObject() {
        spawnedBenza.transform.localScale = new Vector3(.3f,.3f,.3f);
        spawnPosition.y = -0.15f;
        Instantiate(spawnedBenza, spawnPosition, Quaternion.identity);
    }

    public void tracePath() {
        pathFinder.GetComponent<PathFinding>().enabled = true;
    }
}
