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
        GameObject newFood = Instantiate(food, new Vector2(Random.Range(-13, 5), Random.Range(-9, 9)), Quaternion.identity);
        NetworkObject netFood = newFood.GetComponent<NetworkObject>();
        netFood.Spawn();
    }

    //x: -13 -> 5
    //y: -9 -> 9
}
