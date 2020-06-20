using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    public Route Route { get; set; }
    public List<Resource> Resources { get; private set; } = new List<Resource>();

    int _nextMarketIndex = 0;

    public void Tick(float deltaTime)
    {
        var nextMarket = Route.Markets[_nextMarketIndex];

        transform.position = Vector2.MoveTowards(transform.position, nextMarket.transform.position, _speed * deltaTime);
    }
}
