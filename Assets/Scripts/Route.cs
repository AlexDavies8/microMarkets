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
    public bool HasCollision { get; set; }
    public List<Market> MarketsBuffer { get; private set; } = new List<Market>();

    bool _prevDragging;
    public List<Vector2> RoutePositions { get; private set; } = new List<Vector2>();
    private List<Vector2> _routePositions = new List<Vector2>();
    public bool _isLoop => MarketsBuffer[0] == MarketsBuffer[MarketsBuffer.Count - 1] && MarketsBuffer.Count > 2;

    public void Tick(float deltaTime)
    {
        if (!Dragging && _prevDragging && MarketsBuffer.Count > 1)
        {
            RoutePositions.Clear();
            RoutePositions.AddRange(_routePositions);

            Markets.Clear();
            Markets.AddRange(MarketsBuffer);
        }

        if (Dragging) RebuildRoutePositions();
        UpdateHandle();

        _prevDragging = Dragging;
    }

    public void RebuildRoutePositions()
    {
        bool showDragPosition = Vector2.Distance(MarketsBuffer[MarketsBuffer.Count - 1].transform.position, DragPosition) > 1f && !_isLoop;

        _routePositions.Clear();

        Vector2 midpoint = MarketsBuffer.Count > 1 ? -(Vector2)MarketsBuffer[1].transform.position : -DragPosition;

        for (int i = 0; i < MarketsBuffer.Count; i++)
        {
            _routePositions.Add(MarketsBuffer[i].transform.position + (Vector3)_positionOffset);
            if (i != MarketsBuffer.Count - 1)
            {
                midpoint = GetMidpoint(MarketsBuffer[i].transform.position + (Vector3)_positionOffset, MarketsBuffer[i + 1].transform.position, midpoint);
                _routePositions.Add(midpoint);
            }
        }

        if (showDragPosition)
        {
            midpoint = GetMidpoint(MarketsBuffer[MarketsBuffer.Count - 1].transform.position, DragPosition, midpoint);
            _routePositions.Add(midpoint);
            _routePositions.Add(DragPosition);
        }

        _lineRenderer.positionCount = _routePositions.Count;
        _lineRenderer.SetPositions(_routePositions.Select(x => (Vector3)x).ToArray());
    }

    public void UpdateHandle()
    {
        bool showDragPosition = Vector2.Distance(MarketsBuffer[MarketsBuffer.Count - 1].transform.position, DragPosition) > 1f && !_isLoop;

        if (_isLoop)
        {
            _handle.gameObject.SetActive(true);
            _handle.transform.position = _routePositions[_routePositions.Count - 1];
            Vector2 v0 = (_routePositions[0] - _routePositions[_routePositions.Count - 2]).normalized;
            Vector2 v1 = (_routePositions[0] - _routePositions[1]).normalized;
            Vector2 diff = ((v0 + v1) * 0.5f).normalized;
            if (diff == Vector2.zero) diff = new Vector2(v0.y, -v0.x);
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            _handle.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        else if (!showDragPosition && MarketsBuffer.Count > 1)
        {
            _handle.gameObject.SetActive(true);
            _handle.transform.position = _routePositions[_routePositions.Count - 1];
            Vector2 diff = (_routePositions[_routePositions.Count - 1] - _routePositions[_routePositions.Count - 2]).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            _handle.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        else
        {
            _handle.gameObject.SetActive(false);
        }
    }

    public void AddMarket(Market market) => MarketsBuffer.Add(market);

    public int GetMarketCount() => MarketsBuffer.Count;

    public void RemoveLastMarket() => MarketsBuffer.RemoveAt(MarketsBuffer.Count - 1);

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
