using UnityEngine;

public class SnakeNode : MonoBehaviour
{

    public SnakeNode previous { get; set; }
    public SnakeNode next { get; set; }
    public Vector2 wantedPosition { get; set; }
    [SerializeField]
    public float speed;

    public SnakeNode(SnakeNode previous)
    {
        this.previous = previous;
    }

    void Start()
    {
        
    }

    void Update()
    {
        move();
    }

    void move()
    {
        if((Vector2)transform.position == wantedPosition)
        {
            if (previous != null)
            {
                wantedPosition = previous.wantedPosition;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, wantedPosition, speed);
    }
}
