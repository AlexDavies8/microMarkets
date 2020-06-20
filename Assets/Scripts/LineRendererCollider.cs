using UnityEngine;

public class LineRendererCollider : MonoBehaviour
{
    LineRenderer _lineRenderer;
    EdgeCollider2D _edgeCollider;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        _edgeCollider.points = new Vector2[_lineRenderer.positionCount];
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _edgeCollider.points[i] = _lineRenderer.GetPosition(i);
        }
    }
}
