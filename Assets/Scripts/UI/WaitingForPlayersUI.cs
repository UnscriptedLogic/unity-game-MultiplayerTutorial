using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayersUI : MonoBehaviour
{

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged += Instance_OnLocalPlayerReadyChanged;
        KitchenGameManager.Instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }

    private void Instance_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void Instance_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.isLocalPlayerReady)
        {
            Show();
        }
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
