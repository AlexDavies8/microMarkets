using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMarketState : IState
{
    Wagon _wagon;

    public MoveToMarketState(Wagon wagon)
    {
        _wagon = wagon;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        MoveWagon(TimeController.DeltaTime);

        if (_wagon.RouteMarkets.Count > 1) _wagon.MoveCarts();

        _wagon.UpdateResourceRenderers();
    }

    void MoveWagon(float deltaTime)
    {
        var nextPoint = _wagon.RoutePositions[_wagon.NextPointIndex];

        Vector2 diff = (nextPoint - (Vector2)_wagon.transform.position).normalized;
        _wagon.Animator.SetFloat("x", diff.x);
        _wagon.Animator.SetFloat("y", diff.y);

        _wagon.x = diff.x > 0.5f ? 1 : diff.x < -0.5f ? -1 : 0;
        _wagon.y = diff.y > 0.5f ? 1 : diff.y < -0.5f ? -1 : 0;

        _wagon.transform.position = Vector2.MoveTowards(_wagon.transform.position, nextPoint, _wagon.Speed * deltaTime);

        if ((Vector2)_wagon.transform.position == nextPoint)
        {
            if (_wagon.NextPointIndex % 2 == 0) // Market 
                ArriveAtMarket(_wagon.RouteMarkets[_wagon.NextPointIndex / 2]);
            _wagon.NextPointIndex = _wagon.GetNextIndex(_wagon.NextPointIndex, ref _wagon._direction);
        }
    }

    void ArriveAtMarket(Market market)
    {
        if (_wagon.Resources.Contains(market.Type) || _wagon.Resources.Count < 6 * (_wagon.Carts.Count + 1))
        {
            _wagon.CurrentMarket = market;
        }

        _wagon.RemoveCounter++;
    }
}
