using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Route : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer = null;
    [SerializeField] private Transform _handle = null;
    [SerializeField] private EdgeCollider2D _edgeCollider = null;
    [SerializeField] private Vector2 _positionOffset = Vector2.zero;
    public Transform Handle => _handle;

    public List<Market> Markets { get; private set; } = new List<Market>();
    public bool IsLoop => Markets[0] == Markets[Markets.Count - 1] && Markets.Count > 2;
    public Vector2 DragPosition { get; set; }
    public bool Dragging { get; set; }

    public List<Vector2> RoutePositions { get; private set; } = new List<Vector2>();

    public void Tick(float deltaTime)
    {
        if (Dragging) RebuildRoutePositions();
        UpdateHandle();
    }

    public void RebuildRoutePositions()
    {
        bool showDragPosition = Vector2.Distance(Markets[Markets.Count - 1].transform.position, DragPosition) > 1f && !IsLoop;

        RoutePositions.Clear();

        Vector2 midpoint = Markets.Count > 1 ? -(Vector2)Markets[1].transform.position : -DragPosition;

        for (int i = 0; i < Markets.Count; i++)
        {
            RoutePositions.Add(Markets[i].transform.position + (Vector3)_positionOffset);
            if (i != Markets.Count - 1)
            {
                midpoint = GetMidpoint(Markets[i].transform.position + (Vector3)_positionOffset, Markets[i + 1].transform.position, midpoint);
                RoutePositions.Add(midpoint);
            }
        }

        if (showDragPosition)
        {
            midpoint = GetMidpoint(Markets[Markets.Count - 1].transform.position, DragPosition, midpoint);
            RoutePositions.Add(midpoint);
            RoutePositions.Add(DragPosition);
        }

        _lineRenderer.positionCount = RoutePositions.Count;
        _lineRenderer.SetPositions(RoutePositions.Select(x => (Vector3)x).ToArray());
    }

    public void UpdateHandle()
    {
        bool showDragPosition = Vector2.Distance(Markets[Markets.Count - 1].transform.position, DragPosition) > 1f && !IsLoop;

        if (IsLoop)
        {
            _handle.gameObject.SetActive(true);
            _handle.transform.position = RoutePositions[RoutePositions.Count - 1];
            Vector2 v0 = (RoutePositions[0] - RoutePositions[RoutePositions.Count - 2]).normalized;
            Vector2 v1 = (RoutePositions[0] - RoutePositions[1]).normalized;
            Vector2 diff = ((v0 + v1) * 0.5f).normalized;
            if (diff == Vector2.zero) diff = new Vector2(v0.y, -v0.x);
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            _handle.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        else if (!showDragPosition && Markets.Count > 1)
        {
            _handle.gameObject.SetActive(true);
            _handle.transform.position = RoutePositions[RoutePositions.Count - 1];
            Vector2 diff = (RoutePositions[RoutePositions.Count - 1] - RoutePositions[RoutePositions.Count - 2]).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            _handle.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        else
        {
            _handle.gameObject.SetActive(false);
        }
    }

    Vector2 GetMidpoint(Vector2 current, Vector2 next, Vector2 prevMidpoint)
    {
        Vector2 shortMidpoint = GetMidpoint(current, next);
        Vector2 longMidpoint = GetMidpoint(next, current);

        float shortAngle = Vector2.Angle(shortMidpoint - current, prevMidpoint - current);
        float longAngle = Vector2.Angle(longMidpoint - current, prevMidpoint - current);

        return shortAngle > longAngle ? shortMidpoint : longMidpoint;
    }

    Vector2 GetMidpoint(Vector2 a, Vector2 b)
    {
        Vector2 diff = b - a;
        Vector2 absDiff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        Vector2 signedDiff = new Vector2(Mathf.Sign(diff.x), Mathf.Sign(diff.y));
        if (absDiff.x > absDiff.y)
            return a + signedDiff * absDiff.y;
        return a + signedDiff * absDiff.x;
    }

    public void EnableCollision()
    {
        _edgeCollider.enabled = true;
    }

    public void DisableCollision()
    {
        _edgeCollider.enabled = false;
    }
}
