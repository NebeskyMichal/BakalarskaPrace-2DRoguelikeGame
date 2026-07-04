using UnityEngine;
using System.Collections.Generic;

public class MapMovementSystem : MonoBehaviour
{
    private List<List<MapNode>> _mapGrid;

    public void Initialize(List<List<MapNode>> grid)
    {
        _mapGrid = grid;
        ResetMap();
    }

    private void ResetMap()
    {
        foreach (var row in _mapGrid)
        {
            foreach (var node in row) node.SetNodeState(NodeState.Locked);
        }

        foreach (var node in _mapGrid[0])
        {
            node.SetNodeState(NodeState.Clickable);
        }
    }

    public void MoveToNode(MapNode clickedNode)
    {
        int currentFloor = clickedNode.FloorLevel;

        foreach (var node in _mapGrid[currentFloor])
        {
            if (node == clickedNode)
            {
                node.SetNodeState(NodeState.Visited);
            }
            else if (node.State == NodeState.Clickable)
            {
                node.SetNodeState(NodeState.Locked);
            }
        }

        foreach (var nextNode in clickedNode.NextNodes)
        {
            nextNode.SetNodeState(NodeState.Clickable);
        }
    }
}