using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameoverPanel = null;
    [SerializeField] private TimeController _timeController = null;
    [SerializeField] private Text _scoreText = null;

    public int Score { get; set; } = 0;
    public int Wagons { get; set; } = 2;
    public int Carts { get; set; } = 1;

    public void GameOver()
    {
        _gameoverPanel.SetActive(true);
        _scoreText.text = $"Score: {Score}";

        _timeController.Timescale = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
