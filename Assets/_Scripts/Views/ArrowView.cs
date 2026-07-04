using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject arrowHead;

    [SerializeField] private LineRenderer arrowBody;

    private Vector3 _startPosition;

    private void Update()
    {
        Vector3 mousePos = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 endPosition = new Vector3(mousePos.x, mousePos.y, 0);
        Vector3 direction = -(_startPosition - arrowHead.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowBody.SetPosition(1, endPosition - direction * 0.5f);
        arrowHead.transform.position = endPosition;
        arrowHead.transform.right = direction;
        arrowHead.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetupArrow(Vector3 startPosition)
    {
        this._startPosition = startPosition;
        arrowBody.SetPosition(0, startPosition);
        arrowBody.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());
    }
}