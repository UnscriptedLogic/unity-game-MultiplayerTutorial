using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMPUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnMultiplayerGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnMultiplayerGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
