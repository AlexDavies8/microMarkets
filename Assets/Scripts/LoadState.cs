using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : IState
{
    Wagon _wagon;

    float _loadTimer;

    public LoadState(Wagon wagon)
    {
        _wagon = wagon;
    }

    public void OnEnter()
    {
        _loadTimer = 1f / _wagon.LoadSpeed;
    }

    public void OnExit() 
    {
        _wagon.CurrentMarket = null;
    }

    public void Tick()
    {
        if (_loadTimer <= 0)
        {
            _wagon.Resources.Add(_wagon.CurrentMarket.TransferResource());
            _wagon.AddResourceRenderer();
            _loadTimer = 1f / _wagon.LoadSpeed;
        }

        _loadTimer -= TimeController.DeltaTime;
    }
}
