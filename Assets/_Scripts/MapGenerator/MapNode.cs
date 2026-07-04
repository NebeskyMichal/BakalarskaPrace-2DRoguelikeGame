using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour
{
    [SerializeField] private MapIconConfig iconConfig;
    [SerializeField] private SpriteRenderer nodeIcon;
    [SerializeField] private SpriteRenderer nodeBorder;
    [SerializeField] private List<MapNode> nextNodes = new();
    [SerializeField] private List<MapConnectionLine> outgoingLines = new();

    public IReadOnlyList<MapNode> NextNodes => nextNodes;
    public IReadOnlyList<MapConnectionLine> OutgoingLines => outgoingLines;

    public int NodeID { get; private set; }
    public int FloorLevel { get; private set; }
    public NodeState State { get; private set; } = NodeState.Locked;
    public EncounterData EncounterData { get; private set; }

    public void Setup(EncounterData newEncounterData)
    {
        EncounterData = newEncounterData;
        nodeIcon.sprite = iconConfig.GetSprite(EncounterData.EncounterType);
    }

    public void InitializeNode(int nodeID, int floorLevel)
    {
        NodeID = nodeID;
        FloorLevel = floorLevel;
    }

    public void AddConnection(MapNode targetNode, MapConnectionLine line)
    {
        if (nextNodes.Contains(targetNode)) return;
        nextNodes.Add(targetNode);
        outgoingLines.Add(line);
    }

    private void OnMouseEnter()
    {
        if (State != NodeState.Clickable) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (nodeBorder != null)
        {
            nodeBorder.color = Color.white;
        }
    }

    private void OnMouseExit()
    {
        if (State != NodeState.Clickable) return;
        if (nodeBorder != null)
        {
            nodeBorder.color = Color.clear;
        }
    }

    private void OnMouseDown()
    {
        if (State != NodeState.Clickable) return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        MapManagerSystem.Instance.OnNodeClicked(this);
    }

    public void SetNodeState(NodeState newState)
    {
        State = newState;

        switch (State)
        {
            case NodeState.Locked:
            {
                nodeIcon.color = Color.gray;
                if (nodeBorder != null) nodeBorder.color = Color.clear;
                break;
            }
            case NodeState.Clickable:
            {
                nodeIcon.color = Color.white;
                if (nodeBorder != null) nodeBorder.color = Color.clear;
                break;
            }
            default:
            {
                nodeIcon.color = Color.white;
                if (nodeBorder != null) nodeBorder.color = Color.white;
                break;
            }
        }
    }
}