using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private WagonManager _wagonManager = null;
    [SerializeField] private GameObject[] _routePrefabs = null;

    public List<Route> Routes { get; private set; } = new List<Route>();
    public int SelectedRoutePrefab { get; set; } = -1;
    private Route _selectedRoute = null;
    public bool[] PrefabsUsed { get; set; }

    private void Awake()
    {
        PrefabsUsed = new bool[_routePrefabs.Length];
    }

    private void Update()
    {
        if (_selectedRoute)
            _selectedRoute.DragPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        foreach (var route in Routes)
        {
            route.Tick(TimeController.DeltaTime);
        }

        if (Input.GetMouseButtonUp(0) && _selectedRoute)
        {
            _selectedRoute.Dragging = false;
            _selectedRoute.DragPosition = _selectedRoute.Markets[_selectedRoute.Markets.Count - 1].transform.position;
            _selectedRoute.RebuildRoutePositions();
            if (_selectedRoute.Markets.Count < 2)
            {
                PrefabsUsed[SelectedRoutePrefab] = false;
                Routes.Remove(_selectedRoute);
                Destroy(_selectedRoute);
                Destroy(_selectedRoute.gameObject);
            }
            _selectedRoute = null;
        }
    }

    public void CreateRoute(Market startMarket)
    {
        if (SelectedRoutePrefab < 0 || PrefabsUsed[SelectedRoutePrefab] == true) return;

        int cachedPrefabIndex = SelectedRoutePrefab;
        PrefabsUsed[SelectedRoutePrefab] = true;
        var routeGO = Instantiate(_routePrefabs[SelectedRoutePrefab]);
        var routeTransform = routeGO.transform;
        routeTransform.SetParent(transform);
        var route = routeGO.GetComponent<Route>();
        route.Markets.Add(startMarket);
        route.GetComponent<SpriteEventTrigger>().OnMouseDown.AddListener(mousePosition => _wagonManager.CreateWagon(mousePosition, route));
        route.Handle.GetComponent<SpriteEventTrigger>().OnMouseDown.AddListener(mousePosition => { _selectedRoute = route; route.Dragging = true; SelectedRoutePrefab = cachedPrefabIndex; });
        Routes.Add(route);
        _selectedRoute = route;
        _selectedRoute.Dragging = true;
    }

    public void SelectRoute(int index)
    {
        SelectedRoutePrefab = index;
    }

    public void SelectMarket(Market market)
    {
        if (!_selectedRoute) return;

        if (market == _selectedRoute.Markets[_selectedRoute.Markets.Count-1] && _selectedRoute.Markets.Count > 1) _selectedRoute.Markets.RemoveAt(_selectedRoute.Markets.Count-1);
        else if ((!_selectedRoute.Markets.Contains(market) || (_selectedRoute.Markets[0] == market && _selectedRoute.Markets.Count > 2)) && !_selectedRoute.IsLoop) _selectedRoute.Markets.Insert(_selectedRoute.Markets.Count, market);
    }

    public void BeginPlaceWagon()
    {
        foreach (var route in Routes)
        {
            route.EnableCollision();
        }
    }

    public void EndPlaceWagon()
    {
        foreach (var route in Routes)
        {
            route.DisableCollision();
        }
    }
}
