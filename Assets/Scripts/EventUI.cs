using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUI : MonoBehaviour
{
    public TimeController TimeController { get; set; }
    public GameManager GameManager { get; set; }

    bool _initialized;
    float _oldTimescale;

    private void Update()
    {
        if (!_initialized && TimeController)
        {
            _oldTimescale = TimeController.Timescale;
            TimeController.Timescale = 0;
            _initialized = true;
        }
    }

    public void ChooseCarts()
    {
        GameManager.Carts += 2;

        Close();
    }

    public void ChooseWagon()
    {
        GameManager.Wagons++;

        Close();
    }

    void Close()
    {
        TimeController.Timescale = _oldTimescale;
        Destroy(gameObject);
    }
}
