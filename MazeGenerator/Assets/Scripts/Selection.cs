using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private int clickCounter = 0;
    public Material materialStartPoint;
    public Material materialEndPoint;
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
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.name == "Floor(Clone)" && clickCounter<2)
                {
                    //Setam material on-Click
                    //Se pot seta cel mult doua puncte
                    Debug.Log("It's working!");
                    //Setam material pentru StartPoint
                    if (clickCounter == 0) {
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = materialStartPoint;
                    }

                    //Setam material pentru EndPoint
                    if (clickCounter == 1) {
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = materialEndPoint;
                    }

                    //Click counter creste. Maxim doua clickuri
                    clickCounter++;
                }
                else
                {
                    Debug.Log("nopz");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
            Debug.Log("Mouse is down");
        }
    }
}
