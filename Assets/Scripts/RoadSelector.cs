using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RoadSelector : MonoBehaviour
{
    [SerializeField] private RouteManager _routeManager = null;
    [SerializeField] private int _routeIndex = 0;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    bool _selected;

    private void Update()
    {
        _spriteRenderer.material.SetFloat("_EffectAmount", _routeManager.PrefabsUsed[_routeIndex] ? 1 : 0);
    }

    public void ToggleSelect()
    {
        _selected = !_selected;

        if (_selected)
        {
            _routeManager.SelectRoute(_routeIndex);
            Select();
        }
        else
        {
            _routeManager.SelectRoute(-1);
            Deselect();
        }
    }

    public void Select()
    {
        _selected = true;
        transform.DOLocalMoveY(0.4f, 0.1f).SetEase(Ease.InOutQuad);
    }

    public void Deselect()
    {
        _selected = false;
        transform.DOLocalMoveY(0.05f, 0.1f).SetEase(Ease.InOutQuad);
    }

    public void DeselectExternal()
    {
        _selected = false;
        transform.DOLocalMoveY(0.0f, 0.1f).SetEase(Ease.InOutQuad);
    }

    public void BeginHover()
    {
        if (_selected) return;

        transform.DOLocalMoveY(0.05f, 0.1f);
    }
    public void EndHover()
    {
        if (_selected) return;

        transform.DOLocalMoveY(0f, 0.1f);
    }
}
