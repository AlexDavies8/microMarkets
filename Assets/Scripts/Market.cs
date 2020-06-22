using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Market : MonoBehaviour
{
    const int Capacity = 10;
    const float OverflowTime = 10f;
    const float ResourceSpawnTime = 6f;
    const float ResourceSpawnRandomness = 2f;

    [SerializeField] private Resource _type = 0;
    [SerializeField] private Sprite[] _resourceSprites = new Sprite[4];

    public MarketController _marketController { get; set; }
    public Resource Type => _type;
    public List<Resource> Queue { get; private set; } = new List<Resource>();

    List<SpriteRenderer> _resourceRenderers = new List<SpriteRenderer>();
    private float _overflowTimer;
    private float _resourceSpawnTimer;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

        _resourceSpawnTimer = ResourceSpawnTime;
    }

    public void Tick(float deltaTime)
    {
        if (Queue.Count > Capacity)
        {
            _overflowTimer -= deltaTime;

            if (_overflowTimer <= 0)
            {
                FindObjectOfType<GameManager>().Lose(); // TODO : FIX LAZYNESS
            }
        }
        else
        {
            _overflowTimer = OverflowTime;
        }

        if (_resourceSpawnTimer <= 0)
        {
            SpawnResource();
            _resourceSpawnTimer = ResourceSpawnTime + Random.Range(-ResourceSpawnRandomness, ResourceSpawnRandomness);
        }

        _resourceSpawnTimer -= deltaTime;
    }

    void SpawnResource()
    {
        var resource = GetResource();
        Queue.Add(resource);
        AddResourceRenderer();

        UpdateResourceRenderers();
    }

    Resource GetResource()
    {
        int resourceIndex = Random.Range(0, _marketController.Resources.Count);
        var resource = _marketController.Resources[resourceIndex];
        if (resource == Type) resource = GetResource();
        return resource;
    }

    void AddResourceRenderer()
    {
        var go = new GameObject("ResourceRenderer", typeof(SpriteRenderer));
        go.transform.SetParent(transform);
        go.transform.localScale = Vector3.one;
        _resourceRenderers.Add(go.GetComponent<SpriteRenderer>());
    }

    void UpdateResourceRenderers()
    {
        for (int i = 0; i < Queue.Count; i++)
        {
            _resourceRenderers[i].transform.localPosition = new Vector2(-6 + (i % 5) * 3, 10 + (i / 5) * 3) / 16f;
            _resourceRenderers[i].sprite = _resourceSprites[(int)Queue[i]];
        }
    }

    public Resource TransferResource()
    {
        var resource = Queue[0];
        Queue.RemoveAt(0);
        Destroy(_resourceRenderers[0].gameObject);
        Destroy(_resourceRenderers[0]);
        _resourceRenderers.RemoveAt(0);
        UpdateResourceRenderers();
        return resource;
    }
}
