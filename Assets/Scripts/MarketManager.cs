using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private GameObject _redMarketPrefab;
    [SerializeField] private GameObject _cyanMarketPrefab;
    [SerializeField] private GameObject _yellowMarketPrefab;
    [SerializeField] private GameObject _purpleMarketPrefab;

    List<Market> _markets = new List<Market>();

    private void Update()
    {
        foreach (var market in _markets)
        {
            market.Tick(Time.deltaTime);
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
        _markets.Add(market);
    }
}
