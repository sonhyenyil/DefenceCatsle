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

    private void Update()
    {
        clearMainButton.onClick.AddListener(scenceChange);
    }
    private void scenceChange()
    {
        SceneManager.LoadScene(0);
    }
}
