using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    //public GameObject mazeGameObject;
    //public GameObject selectionGameObject;
    public Material material;
    private Selection selection;
    private Maze maze;
    public List<GameObject> floorTiles;
    public HashSet<Node> closedSet;
    public List<Node> openSet;

    // Start is called before the first frame update
    void Start()
    {
        selection = GameObject.Find("SelectionManager").GetComponent<Selection>();
        maze = GameObject.Find("MazeGenerator").GetComponent<Maze>();
        //start find path function
        //initialize floor tiles array
        floorTiles = maze.floorTiles;
    }

    // Update is called once per frame
    void Update()
    {
        findPath();
    }

    public void findPath() {
        Node startNode = new Node(findGameObject(selection.spawnPosition));
        Node targetNode = new Node(findGameObject(selection.finishPosition));

        closedSet = new HashSet<Node>();
        openSet = new List<Node>();
        //add first node to be visited
        openSet.Add(startNode);

        //start loop
        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.gameObject == findGameObject(selection.finishPosition)) {
                retracePath(startNode,targetNode);
                return;
            }

            foreach (Node neighbour in findNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                float newMovementCostToNeighbour = currentNode.gCost + calculateDistance(currentNode,neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = calculateDistance(neighbour,targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    public GameObject findGameObject(Vector3 requiredPosition) {
        //find object located at specified position
        foreach (GameObject g in floorTiles)
        {
            if (g.transform.position.Equals(requiredPosition)) {
                Debug.Log("Am gsit gameobject cu startpoint");
                return g;
            }
        }

        return null;
    }

    public List<Node> findNeighbours(Node node) {
        Debug.Log("Am intrat");
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                //current GameObject coordiantes
                /*1 1 1
                  1 O 1
                  1 1 1
                */
                float x = node.gameObject.transform.position.x - i;
                float y = -0.5f;
                float z = node.gameObject.transform.position.z - j;
                Vector3 newNeighbourPosition = new Vector3(x, y, z);

                //find GameObject and add to list
                neighbours.Add(new Node(findGameObject(newNeighbourPosition)));
            }
        }

        return neighbours;
    }

    public float calculateDistance(Node node1, Node node2) {
        float dstX = Mathf.Abs(node1.gameObject.transform.position.x - node2.gameObject.transform.position.x);
        float dstZ = Mathf.Abs(node1.gameObject.transform.position.z - node2.gameObject.transform.position.z);

        if (dstX > dstZ)
        {
            return 14 * dstZ + 10 * (dstX - dstZ);
        }

        return 14 * dstX + 10 * (dstZ - dstX);
    }

    public void retracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        paintPath(path);
    }

    public void paintPath(List<Node> list) {
        foreach (Node n in list) {
            n.gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }
}
