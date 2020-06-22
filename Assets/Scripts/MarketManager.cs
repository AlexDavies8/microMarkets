using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private RouteManager _routeManager = null;
    [SerializeField] private MarketController _marketController = null;

    [SerializeField] private GameObject _redMarketPrefab = null;
    [SerializeField] private GameObject _cyanMarketPrefab = null;
    [SerializeField] private GameObject _yellowMarketPrefab = null;
    [SerializeField] private GameObject _purpleMarketPrefab = null;

    List<Market> _markets = new List<Market>();

    private void Update()
    {
        foreach (var market in _markets)
        {
            market.Tick(TimeController.DeltaTime);
        }
    }

    public void CreateMarket(Resource type, Vector2 position)
    {
        if (type == Resource.Red) CreateMarket(_redMarketPrefab, position);
        if (type == Resource.Cyan) CreateMarket(_cyanMarketPrefab, position);
        if (type == Resource.Yellow) CreateMarket(_yellowMarketPrefab, position);
        if (type == Resource.Purple) CreateMarket(_purpleMarketPrefab, position);
    }

    void CreateMarket(GameObject prefab, Vector2 position)
    {
        var marketGO = Instantiate(prefab);
        var marketTransform = marketGO.transform;
        marketTransform.position = position;
        marketTransform.SetParent(transform);
        var market = marketGO.GetComponent<Market>();
        market._marketController = _marketController;
        _markets.Add(market);
        var eventTrigger = marketGO.GetComponent<SpriteEventTrigger>();
        eventTrigger.OnMouseDown.AddListener(mousePosition => _routeManager.CreateRoute(market));
        eventTrigger.OnMouseEnter.AddListener(mousePosition => _routeManager.SelectMarket(market));
    }
}
