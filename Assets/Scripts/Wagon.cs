using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private float _cartDistance = 0.4f;
    [SerializeField] private float _firstCartDistance = 0.5f;
    [SerializeField] private Vector2 _cartOffset = Vector2.zero;
    [SerializeField] private float _loadSpeed = 3f;
    [SerializeField] private Sprite[] _resourceSprites = new Sprite[4];

    public Animator Animator => _animator;
    public float Speed => _speed;
    public float LoadSpeed => _loadSpeed;
    public Route Route { get; set; }
    public List<Resource> Resources { get; private set; } = new List<Resource>();
    public List<Cart> Carts { get; private set; } = new List<Cart>();
    public int NextPointIndex { get; set; } = 0;
    public Market CurrentMarket { get; set; }
    public GameManager GameManager { get; set; }

    public float x, y;

    List<SpriteRenderer> _resourceRenderers = new List<SpriteRenderer>();

    [HideInInspector] public int _direction = 1;

    StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        var moveToMarketState = new MoveToMarketState(this);
        var loadState = new LoadState(this);
        var unloadState = new UnloadState(this);

        _stateMachine.AddTransition(moveToMarketState, unloadState, () => CurrentMarket != null);
        _stateMachine.AddTransition(unloadState, loadState, () => !Resources.Contains(CurrentMarket.Type));
        _stateMachine.AddTransition(loadState, moveToMarketState, () => CurrentMarket.Queue.Count == 0 || Resources.Count >= 6 * (Carts.Count + 1));

        _stateMachine.SetState(moveToMarketState);
    }

    public void Tick(float deltaTime)
    {
        _stateMachine.Tick();
    }

    public void MoveCarts()
    {
        for (int i = 0; i < Carts.Count; i++)
        {
            Vector2 cartPosition = TraverseRoute(transform.position, NextPointIndex, -_direction, _firstCartDistance + _cartDistance * i);

            Carts[i].MoveTo(cartPosition + _cartOffset);
        }
    }

    public Vector2 TraverseRoute(Vector2 start, int index, int direction, float distance)
    {
        Vector2 position = start;

        int iterLimit = 0;

        while (distance > 0 && iterLimit < 50)
        {
            index = GetNextIndex(index, ref direction);

            float distanceToNext = Vector2.Distance(position, Route.RoutePositions[index]);
            position = Vector2.MoveTowards(position, Route.RoutePositions[index], distance);
            distance -= distanceToNext;

            iterLimit++;
        }

        return position;
    }

    public int GetNextIndex(int index, ref int direction)
    {
        index += direction;

        if (Route.IsLoop)
        {
            if (index >= Route.RoutePositions.Count) index = 0;
            else if (index < 0) index = Route.RoutePositions.Count - 1;
        }
        else
        {
            if (index >= Route.RoutePositions.Count)
            {
                direction = -1;
                index = Route.RoutePositions.Count - 2;
            }
            else if (index < 0)
            {
                direction = 1;
                index = 1;
            }
        }

        return index;
    }

    public void AddResourceRenderer()
    {
        var go = new GameObject("ResourceRenderer", typeof(SpriteRenderer));
        go.transform.SetParent(transform);
        go.transform.localScale = Vector3.one;
        _resourceRenderers.Add(go.GetComponent<SpriteRenderer>());

        UpdateResourceRenderers();
    }

    public void RemoveResourceRenderer()
    {
        Destroy(_resourceRenderers[0].gameObject);
        Destroy(_resourceRenderers[0]);
        _resourceRenderers.RemoveAt(0);
        UpdateResourceRenderers();
    }

    public void UpdateResourceRenderers()
    {
        for (int i = 0; i < Resources.Count; i++)
        {
            int cartIndex = i / 6 - 1;
            if (cartIndex == -1)
            {
                _resourceRenderers[i].transform.SetParent(transform);
                UpdateResource(_resourceRenderers[i], i);
            }
            else
            {
                _resourceRenderers[i].transform.SetParent(Carts[cartIndex].transform);
                Carts[cartIndex].UpdateResource(_resourceRenderers[i], i % 6);
            }

            _resourceRenderers[i].sprite = _resourceSprites[(int)Resources[i]];
        }
    }

    void UpdateResource(SpriteRenderer resource, int index)
    {
        if (x == 1 && y == 0)
        {
            resource.transform.localPosition = new Vector2(-5 + (index % 3) * 2, -1 + (index / 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        if (x == -1 && y == 0)
        {
            resource.transform.localPosition = new Vector2(1 + (index % 3) * 2, -1 + (index / 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        if (x == 0 && y == 1)
        {
            resource.transform.localPosition = new Vector2(-1 + (index % 2) * 2, -4 + (index / 2) * 2) / 16f;
            resource.sortingOrder = 10 - index / 2;
        }
        if (x == 0 && y == -1)
        {
            resource.transform.localPosition = new Vector2(-1 + (index % 2) * 2, 1 + (index / 2) * 2) / 16f;
            resource.sortingOrder = 10 - index / 2;
        }
        if (x == 1 && y == 1)
        {
            resource.transform.localPosition = new Vector2(-4 + (index % 3) * 2, -4 + (index / 3) * 2 + (index % 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        if (x == -1 && y == -1)
        {
            resource.transform.localPosition = new Vector2((index % 3) * 2, -1 + (index / 3) * 2 + (index % 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        if (x == 1 && y == -1)
        {
            resource.transform.localPosition = new Vector2(-4 + (index % 3) * 2, 3 + (index / 3) * 2 - (index % 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        if (x == -1 && y == 1)
        {
            resource.transform.localPosition = new Vector2((index % 3) * 2, (index / 3) * 2 - (index % 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
    }
}
