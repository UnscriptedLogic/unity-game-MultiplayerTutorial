using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter 
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData() 
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player) 
    {
        KitchenObject kitchenObject = player.GetKitchenObject();
        if (kitchenObject) {
            if (kitchenObject.name.Contains("Maxwell")) return;

            KitchenObject.DestroyKitchenObject(kitchenObject);

            InteractLogicServerRpc();
        }
    }
    

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }
}