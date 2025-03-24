using Unity.Netcode;
using UnityEngine;

public class Food : NetworkBehaviour
{
    [SerializeField]
    public int value { get; set; }

    public void Despawn()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(this);
    }
}
