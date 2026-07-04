using UnityEngine;

public class MapConnectionLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer.positionCount = 2;
    }

    public void SetupConnection(Transform startNode, Transform endNode, float offset)
    {
        Vector3 startPos = startNode.position;
        Vector3 endPos = endNode.position;

        Vector3 direction = (endPos - startPos).normalized;

        Vector3 adjustedStart = startPos + (direction * offset);
        Vector3 adjustedEnd = endPos - (direction * offset);

        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, adjustedStart);
        lr.SetPosition(1, adjustedEnd);
    }
}