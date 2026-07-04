using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragging : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 30f;
    [SerializeField] private float minY = 3f;
    [SerializeField] private float maxY = 30f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            float dragAmount = -Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;

            Vector3 newPosition = transform.position + new Vector3(0, dragAmount, 0);

            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;
        }
    }

    private void Start()
    {
        Vector3 startPos = transform.position;
        startPos.y = minY;
        transform.position = startPos;
    }
}