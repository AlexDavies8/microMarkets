using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private float _eventInterval = 35f;
    [SerializeField] private float _eventIntervalRandomness = 10f;

    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private TimeController _timeController = null;

    [SerializeField] private GameObject _eventPrefab = null;

    float _eventTimer;

    private void Awake()
    {
        ResetEventTimer();
    }

    private void Update()
    {
        _eventTimer -= TimeController.DeltaTime;

        if (_eventTimer <= 0)
        {
            ShowEvent();
            ResetEventTimer();
        }
    }

    void ResetEventTimer()
    {
        _eventTimer = _eventInterval + Random.Range(-_eventIntervalRandomness, _eventIntervalRandomness);
    }

    void ShowEvent()
    {
        var eventGO = Instantiate(_eventPrefab, _eventPrefab.transform.position, Quaternion.identity, transform);
        var eventui = eventGO.GetComponent<EventUI>();
        eventui.GameManager = _gameManager;
        eventui.TimeController = _timeController;
    }
}
