using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string _startScene = "Menu";

    private void Start()
    {
        SceneManager.LoadScene(_startScene);
    }
}
