using UnityEngine;

public class LineAnimator : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = -0.5f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material lineMaterial;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineMaterial = lineRenderer.sharedMaterial;
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        lineMaterial.mainTextureOffset = new Vector2(offset, 0);
    }
}