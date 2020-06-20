using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public List<Route> Routes { get; private set; } = new List<Route>();

    public Route testRoute;
    public List<Market> testMarkets;

    private void Awake()
    {
        testRoute.Markets.AddRange(testMarkets);
        Routes.Add(testRoute);
    }

    private void Update()
    {
        foreach (var route in Routes)
        {
            route.Tick(Time.deltaTime);
        }
    }
}
