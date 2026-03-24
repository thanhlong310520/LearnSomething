using GraphAlgorithms;
using UnityEngine;

public class TestGraphAlgorithms : MonoBehaviour
{

    public Node startNode;
    public Node endNode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GraphAlgorithm.BFS(startNode, HandleNode);

        var path = GraphAlgorithm.DijkstraPath(startNode, endNode);
        print("Dijkstra Path:");
        if (path != null)
        {

            foreach (var node in path)
            {
                print(node.name);
            }
        }
        else
        {
            print("No path found");
        }
    }

    void HandleNode(Node node)
    {
        Debug.Log("Visited node: " + node.name);
    }
}
