using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private Text _text = null;

    private void Update()
    {
        _text.text = _gameManager.Score.ToString();
    }
}
