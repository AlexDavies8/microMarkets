using System.Linq;
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
        Vector3[] positions = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(positions);
        _edgeCollider.SetPoints(positions.Select(x => (Vector2)x).ToList());
    }
}
