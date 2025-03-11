using UnityEngine;

public class SnakeNode : MonoBehaviour
{
    public Vector2 wantedPosition { get; set; }
    [SerializeField]
    public float speed;

    void Start()
    {

    }

    void Update()
    {
        move();
    }

    void move()
    {
        transform.position = Vector2.MoveTowards(transform.position, wantedPosition, speed);
    }
}
