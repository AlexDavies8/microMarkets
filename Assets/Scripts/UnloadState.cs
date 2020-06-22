using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadState : IState
{
    Wagon _wagon;

    float _unloadTimer;

    public UnloadState(Wagon wagon)
    {
        _wagon = wagon;
    }

    public void OnEnter()
    {
        _unloadTimer = 1f / _wagon.LoadSpeed;
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (_unloadTimer <= 0)
        {
            int index = _wagon.Resources.IndexOf(_wagon.CurrentMarket.Type);
            _wagon.Resources.RemoveAt(index);
            _wagon.RemoveResourceRenderer();

            _wagon.GameManager.Score++;

            _unloadTimer = 1f / _wagon.LoadSpeed;
        }

        _unloadTimer -= TimeController.DeltaTime;
    }
}
