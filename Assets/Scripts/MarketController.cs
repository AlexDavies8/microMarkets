using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketController : MonoBehaviour
{
    [SerializeField] private MarketManager _marketManager = null;
    [SerializeField] private float _spawnInterval = 15f;
    [SerializeField] private Vector2 _minPosition = Vector2.zero;
    [SerializeField] private Vector2 _maxPosition = Vector2.zero;
    [SerializeField] private float _minDistance = 2f;
    [SerializeField] private int _startingMarkets = 3;
    [SerializeField] private float _minimumProportion = 0.3f;

    public List<Resource> Resources { get; private set; } = new List<Resource>();

    List<Vector2> _positions = new List<Vector2>();
    float _spawnTimer = 0;

    private void Awake()
    {
        for (int i = 0; i < _startingMarkets; i++)
            SpawnMarket();
    }

    private void Update()
    {
        if (_spawnTimer > _spawnInterval)
        {
            _spawnTimer = 0;
            SpawnMarket();
        }

        _spawnTimer += TimeController.DeltaTime;
    }

    void SpawnMarket()
    {
        Vector2 position = GetPosition();
        _positions.Add(position);

        Resource resource = GetResource();
        Resources.Add(resource);

        _marketManager.CreateMarket(resource, position);
    }

    Resource GetResource()
    {
        for (int i = 0; i < 4; i++)
        {
            var resource = (Resource)i;
            if ((float)Resources.Count(x => x == resource) / Resources.Count < _minimumProportion)
            {
                return resource;
            }
        }

        return (Resource)Random.Range(0, 4);
    }

    Vector2 GetPosition()
    {
        Vector2 position = new Vector2(Random.Range(_minPosition.x, _maxPosition.x), Random.Range(_minPosition.y, _maxPosition.y));
        foreach (var pos in _positions)
        {
            if (Vector2.Distance(pos, position) < _minDistance)
            {
                position = GetPosition();
                break;
            }
        }
        return position;
    }
}
