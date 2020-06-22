using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleControl : MonoBehaviour
{
    [SerializeField] private TimeController _timeController = null;
    [SerializeField] private float _timescale = 1f;

    [SerializeField] private SpriteRenderer _spriteRenderer = null;

    [SerializeField] private Sprite _normalSprite = null;
    [SerializeField] private Sprite _hoverSprite = null;
    [SerializeField] private Sprite _selectSprite = null;

    bool _selected;

    public void Select()
    {
        _selected = true;

        _timeController.Timescale = _timescale;

        _spriteRenderer.sprite = _selectSprite;
    }

    public void Deselect()
    {
        _selected = false;

        _spriteRenderer.sprite = _normalSprite;
    }

    public void BeginHover()
    {
        if (_selected) return;

        _spriteRenderer.sprite = _hoverSprite;
    }

    public void EndHover()
    {
        if (_selected) return;

        _spriteRenderer.sprite = _normalSprite;
    }
}
