using GraphAlgorithms;
using UnityEngine;

public class TestGraphAlgorithms : MonoBehaviour
{

    public Node startNode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GraphAlgorithm.BFS(startNode, HandleNode);
    }

    void HandleNode(Node node)
    {
        Debug.Log("Visited node: " + node.name);
    }
}
