using Unity.Netcode;
using UnityEngine;

public class PlayerSnake : NetworkBehaviour
{
    [SerializeField] GameObject snake;
    public NetworkVariable<Snake.Direction> currentDir = new NetworkVariable<Snake.Direction>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
            SpawnSnakeRpc();
    }

    [Rpc(SendTo.Server)]
    void SpawnSnakeRpc()
    {
        GameObject newSnake = Instantiate(snake);
        Snake snakeScript = newSnake.GetComponent<Snake>();
        snakeScript.setPlayer(this);
        NetworkObject networkSnake = newSnake.GetComponent<NetworkObject>();
        networkSnake.Spawn();
    }

    void Update()
    {
        if(IsOwner)
            updateDirection();
    }

    void updateDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentDir.Value = Snake.Direction.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            currentDir.Value = Snake.Direction.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentDir.Value = Snake.Direction.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentDir.Value = Snake.Direction.right;
        }
    }
}
