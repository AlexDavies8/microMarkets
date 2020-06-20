using DG.Tweening;
using UnityEngine;

public class RoadSelector : MonoBehaviour
{
    bool _selected;

    public void ToggleSelect()
    {
        _selected = !_selected;

        if (_selected) Select();
        else Deselect();
    }

    public void Select()
    {
        transform.DOLocalMoveY(0.4f, 0.1f).SetEase(Ease.InOutQuad);
    }

    public void Deselect()
    {
        transform.DOLocalMoveY(0.05f, 0.1f).SetEase(Ease.InOutQuad);
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
