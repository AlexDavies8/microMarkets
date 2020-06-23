using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonManager : MonoBehaviour
{
    [SerializeField] private GameObject _wagonPrefab = null;
    [SerializeField] private GameObject _cartPrefab = null;
    [SerializeField] private RouteManager _routeManager = null;
    [SerializeField] private GameManager _gameManager = null;

    public bool PlacingCart { get; set; }

    List<Wagon> _wagons = new List<Wagon>();

    private void Update()
    {
        foreach (var wagon in _wagons)
            wagon.Tick(TimeController.DeltaTime);
    }

    public void BeginPlaceWagon()
    {
        _routeManager.BeginPlaceWagon();
    }

    public void EndPlaceWagon()
    {
        _routeManager.EndPlaceWagon();
    }

    public void CreateWagon(Vector2 mousePosition, Route route)
    {
        if (_gameManager.Wagons <= 0) return;
        _gameManager.Wagons--;
        var wagonGO = Instantiate(_wagonPrefab);
        var wagonTransform = wagonGO.transform;
        wagonTransform.SetParent(route.transform);
        var wagon = wagonGO.GetComponent<Wagon>();
        float minDist = float.MaxValue;
        for (int i = 0; i < route.RoutePositions.Count; i++)
        {
            float dist = Vector2.Distance(mousePosition, route.RoutePositions[i]);
            if (dist < minDist)
            {
                wagonTransform.localPosition = route.RoutePositions[i];
                wagon.NextPointIndex = i;
                minDist = dist;
            }
        }
        wagon.Route = route;
        wagon.GameManager = _gameManager;
        wagonGO.GetComponent<SpriteEventTrigger>().OnMouseDown.AddListener(pos => 
        { 
            if (PlacingCart) CreateCart(wagon); 
            else
            {
                _wagons.Remove(wagon);
                foreach (var cart in wagon.Carts)
                {
                    Destroy(cart.gameObject);
                    Destroy(cart);

                    _gameManager.Carts++;
                }
                Destroy(wagon.gameObject);
                Destroy(wagon);

                _gameManager.Wagons++;
            }
        });
        _wagons.Add(wagon);
    }

    public void CreateCart(Wagon wagon)
    {
        if (_gameManager.Carts <= 0) return;
        _gameManager.Carts--;
        var cartGO = Instantiate(_cartPrefab);
        var cartTransform = cartGO.transform;
        cartTransform.SetParent(wagon.transform.parent);
        var cart = cartGO.GetComponent<Cart>();
        wagon.Carts.Add(cart);
    }
}
