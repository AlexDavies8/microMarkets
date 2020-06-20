using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Route : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer = null;

    public List<Market> Markets { get; private set; } = new List<Market>();
    public bool IsLoop => Markets[0] == Markets[Markets.Count - 1];

    public void Tick(float deltaTime)
    {
        _lineRenderer.positionCount = Markets.Count;
        for (int i = 0; i < Markets.Count; i++)
        {
            _lineRenderer.SetPosition(i,  Markets[i].transform.position);
        }
    }
}
