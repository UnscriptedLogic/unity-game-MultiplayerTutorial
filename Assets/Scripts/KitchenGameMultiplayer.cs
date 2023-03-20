using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectListSO kitchenObjectList;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNOReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetworkObject kitchenNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParentNOReference.TryGet(out NetworkObject kitchenObjectParentNO);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNO.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenobjectReference)
    {
        kitchenobjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(kitchenobjectReference);

        kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenobjectReference)
    {
        kitchenobjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitcehObjectOnParent();
    }

    private int GetKitchenObjectIndex(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenObjectList.kitchenObjectSOList.IndexOf(kitchenObjectSO);
    } 

    private KitchenObjectSO GetKitchenObjectFromIndex(int kitchenObjectSOIndex)
    {
        return kitchenObjectList.kitchenObjectSOList[kitchenObjectSOIndex];
    }
}
