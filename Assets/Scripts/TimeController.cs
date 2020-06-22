using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class TimeController : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float _timescale = 1f;
    public float Timescale
    {
        get => _timescale;
        set => _timescale = value;
    }

    public static float DeltaTime;

    private void Update()
    {
        DeltaTime = Time.deltaTime * Timescale;
    }
}
