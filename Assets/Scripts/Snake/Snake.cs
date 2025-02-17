using UnityEngine;

public class Snake : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction currentDir { get; set; }
    public SnakeNode head { get; set; }

    void Start()
    {
        initSnake();   
    }

    void Update()
    {
        move();
    }

    void initSnake()
    {
        AddSection(transform.position);
        AddSection(new Vector2(transform.position.x, transform.position.y - 1));
        AddSection(new Vector2(transform.position.x, transform.position.y - 2));
    }
    void move()
    {
        if((Vector2)head.transform.position == head.wantedPosition)
        {
            updateDirection();
            head.wantedPosition = getNextPosition();
        }
        head.transform.position = Vector2.MoveTowards(head.transform.position, head.wantedPosition, head.speed);
    }

    void updateDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentDir = Direction.up;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            currentDir = Direction.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentDir = Direction.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentDir = Direction.right;
        }
    }

    Vector2 getNextPosition()
    {
        Vector2 next = new Vector2(transform.position.x, transform.position.y);
        switch (currentDir)
        {
            case Direction.up:
                next.y += 1;
                break;
            case Direction.down:
                next.y -= 1;
                break;
            case Direction.left:
                next.x -= 1;
                break;
            case Direction.right:
                next.x += 1;
                break;
        }
        return next;
    }

    void AddSection(Vector2 position)
    {
        if (head == null)
        {
            head = new SnakeNode(null);
            head.transform.position = position;
        }
        else
        {
            SnakeNode last = getLastNode();
            last.next = new SnakeNode(last);
            last.next.transform.position = position;
        }
    }

    SnakeNode getLastNode()
    {
        SnakeNode current = head;
        while(current.next != null)
        {
            current = current.next;
        }
        return current;
    }
}