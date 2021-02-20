using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text level;

    private void Start()
    {
        score.text = DataController.Instance.Coin.ToString();
        level.text = "Level " + (DataController.Instance.IndexLevel + 1).ToString();
    }

    public void TapToStart()
    {
        DataController.Instance.IndexWave = 0;
        SceneManager.LoadScene("Game");
    }
}
