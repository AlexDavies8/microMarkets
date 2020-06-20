using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    const int Capacity = 10;
    const float OverflowTime = 10f;

    [SerializeField] private Resource _type = 0;

    public Resource Type => _type;
    public List<Resource> Queue { get; private set; } = new List<Resource>();

    private float _overflowTimer;

    public void Tick(float deltaTime)
    {
        if (Queue.Count > Capacity)
        {
            _overflowTimer -= deltaTime;
        }
        else
        {
            _overflowTimer = OverflowTime;
        }
    }
}
