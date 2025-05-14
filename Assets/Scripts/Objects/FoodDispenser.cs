using Unity.Netcode;
using UnityEngine;

public class FoodDispenser : NetworkBehaviour
{
    [SerializeField] GameObject food;
    [SerializeField] float timeForFoodSpawn;
    float lastSpawn;

    void Start()
    {
        if (!IsServer) { return; }
            lastSpawn = Time.time;
    }

    void Update()
    {
        if (!IsServer) { return; }

        if(Time.time > timeForFoodSpawn + lastSpawn)
        {
            SpawnFood();
            lastSpawn = Time.time;
        }
    }

    void SpawnFood()
    {
        Vector2 spawnPosition;
        do
        {
            spawnPosition = new Vector2(Random.Range(-12, 4), Random.Range(-8, 8));
        } while (!checkIfFreePosition(spawnPosition)) ;
        GameObject newFood = Instantiate(food, new Vector2(Random.Range(-12, 4), Random.Range(-8, 8)), Quaternion.identity);
        NetworkObject netFood = newFood.GetComponent<NetworkObject>();
        netFood.Spawn();
    }

    bool checkIfFreePosition(Vector2 position)
    {
        LayerMask mask = LayerMask.GetMask("Snake") | LayerMask.GetMask("Wall");
        RaycastHit2D hitU = Physics2D.Raycast(position, Vector2.up, .48f, mask);
        RaycastHit2D hitD = Physics2D.Raycast(position, Vector2.down, .48f, mask);
        RaycastHit2D hitL = Physics2D.Raycast(position, Vector2.left, .48f, mask);
        RaycastHit2D hitR = Physics2D.Raycast(position, Vector2.right, .48f, mask);

        bool result;
        if (!hitU && !hitD && !hitL && !hitR)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
}