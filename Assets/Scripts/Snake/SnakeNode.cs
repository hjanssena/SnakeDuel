using Unity.Netcode;
using UnityEngine;

public class SnakeNode : NetworkBehaviour
{
    public Vector2 heading { get; set; }
    [SerializeField]
    public float speed;
    public SnakeNode previous { get; set; }
    public SnakeNode next { get; set; }

    void Update()
    {
        move();
    }

    void move()
    {
        transform.position = Vector2.MoveTowards(transform.position, heading, speed * Time.deltaTime);
    }

    public RaycastHit2D CheckForFoodCollision(Vector2 direction)
    {
        LayerMask mask = LayerMask.GetMask("Food");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, .4f, mask);
        return hit;
    }

    public RaycastHit2D CheckForWallCollision(Vector2 direction)
    {
        LayerMask mask = LayerMask.GetMask("Wall");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, .4f, mask);
        return hit;
    }

    public RaycastHit2D CheckForNodeCollision(Vector2 direction)
    {
        LayerMask mask = LayerMask.GetMask("Node");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, .4f, mask);
        return hit;
    }

    public void KillNode()
    {
        Destroy(gameObject);
    }
}
