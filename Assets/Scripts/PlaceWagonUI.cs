using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceWagonUI : MonoBehaviour
{
    [SerializeField] private WagonManager _wagonManager = null;
    [SerializeField] private bool _wagon = true;
    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private Text _amountText = null;
    bool _selected;

    private void Update()
    {
        if (_wagon) _amountText.text = _gameManager.Wagons.ToString();
        else _amountText.text = _gameManager.Carts.ToString();
    }

    public void ToggleSelect()
    {
        _selected = !_selected;

        if (_selected)
        {
            if (_wagon) _wagonManager.BeginPlaceWagon();
            else _wagonManager.PlacingCart = true;
            Select();
        }
        else
        {
            if (_wagon) _wagonManager.EndPlaceWagon();
            else _wagonManager.PlacingCart = false;
            Deselect();
        }
    }

    public void Select()
    {
        _selected = true;
        transform.DOLocalMoveX(0.4f, 0.1f).SetEase(Ease.InOutQuad);
    }

    public void Deselect()
    {
        _selected = false;
        transform.DOLocalMoveX(0.05f, 0.1f).SetEase(Ease.InOutQuad);
    }

    public void DeselectExternal()
    {
        _selected = false;
        transform.DOLocalMoveX(0.0f, 0.1f).SetEase(Ease.InOutQuad);
        if (_wagon) _wagonManager.EndPlaceWagon();
        else _wagonManager.PlacingCart = false;
    }

    public void BeginHover()
    {
        if (_selected) return;

        transform.DOLocalMoveX(0.05f, 0.1f);
    }
    public void EndHover()
    {
        if (_selected) return;

        transform.DOLocalMoveX(0f, 0.1f);
    }
}
