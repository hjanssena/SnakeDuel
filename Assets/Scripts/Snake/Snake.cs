using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction currentDir { get; set; }
    public LinkedList<GameObject> nodes;
    [SerializeField]
    public GameObject snakeNode;

    void Start()
    {
        initSnake();
    }

    void Update()
    {
        updateDirection();
        move();
    }

    void initSnake()
    {
        nodes = new LinkedList<GameObject>();
        AddSection(transform.position);
        AddSection(new Vector2(transform.position.x, transform.position.y - 1));
        AddSection(new Vector2(transform.position.x, transform.position.y - 2));
    }
    void move()
    {
        SnakeNode headScript = nodes.First.Value.GetComponent<SnakeNode>();
        if ((Vector2)nodes.First.Value.transform.position == headScript.wantedPosition)
        {
            UpdateAllNodeHeadings();
        }
        //nodes.First.Value.transform.position = Vector2.MoveTowards(nodes.First.Value.transform.position, headScript.wantedPosition, headScript.speed);
    }

    void updateDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentDir = Direction.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
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

    Vector2 getNextHeading()
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
        GameObject section = Instantiate(snakeNode);
        section.transform.position = position;
        nodes.AddLast(section);
    }

    void UpdateAllNodeHeadings()
    {
        nodes.First.Value.GetComponent<SnakeNode>().wantedPosition = getNextHeading();
        LinkedListNode<GameObject> current = nodes.First.Next;
        while (current != null)
        {
            current.Value.GetComponent<SnakeNode>().wantedPosition = current.Previous.Value.GetComponent<SnakeNode>().transform.position;
            current = current.Next;
        }
    }
}