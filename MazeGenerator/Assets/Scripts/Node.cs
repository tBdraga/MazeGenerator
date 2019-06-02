using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public float gCost;
    public float hCost;
    public GameObject gameObject;
    public Node parent;

    // quick function to add gCost and hCost
    public float FCost { get { return gCost + hCost; } }

    public Node(GameObject gameObject) {
        this.gameObject = gameObject;
    }
}
