using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SenceChage : MonoBehaviour
{
    [SerializeField] Button clearMainButton;
    [SerializeField] Button clearRestartButton;
    [SerializeField] Button clearRankButton;
    [SerializeField] Button overmainButton;
    [SerializeField] Button overRestartButton;

    private void Start()
    {
        clearMainButton.onClick.AddListener(mainMenu);
        clearRestartButton.onClick.AddListener(gameRestart);
        overmainButton.onClick.AddListener(mainMenu);
        overRestartButton.onClick.AddListener(gameRestart);
    }

    private void gameRestart() 
    {
        SceneManager.LoadScene(1);
    }

    private void mainMenu() 
    {
        SceneManager.LoadScene(0);
    }
}
