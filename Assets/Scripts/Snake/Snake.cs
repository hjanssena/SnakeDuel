using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class Snake : NetworkBehaviour
{
    public enum Direction { up, down, left, right };
    public NetworkVariable<Direction> currentDir = new NetworkVariable<Direction>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public SnakeNode head;
    [SerializeField]
    public GameObject snakeNode;
    bool dead;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
            initSnake();
    }

    void Update()
    {
        if (!dead)
        {
            move();
            EatFood();
            if (IsOwner)
            {
                updateDirection();
            }
            CheckCollision();
        }
    }

    void initSnake()
    {
        AddSectionRpc(transform.position);
        AddSectionRpc(new Vector2(transform.position.x, transform.position.y - 1));
        AddSectionRpc(new Vector2(transform.position.x, transform.position.y - 2));
        dead = false;
    }

    void move()
    {
        if ((Vector2)head.gameObject.transform.position == head.heading)
        {
            UpdateHeadings();
        }
    }

    void updateDirection()
    {
        Direction direction = getCurrentHeading();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (direction != Direction.down)
                currentDir.Value = Direction.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (direction != Direction.up)
                currentDir.Value = Direction.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (direction != Direction.right)
                currentDir.Value = Direction.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (direction != Direction.left)
                currentDir.Value = Direction.right;
        }
    }

    Vector2 getNextHeading()
    {
        Vector2 next = new Vector2(head.gameObject.transform.position.x, head.gameObject.transform.position.y);
        switch (currentDir.Value)
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

    void CheckCollision()
    {
        RaycastHit2D isWall = new RaycastHit2D();
        RaycastHit2D isSnake = new RaycastHit2D();
        Direction direction = getCurrentHeading();

        switch (direction)
        {
            case Direction.up:
                isWall = head.CheckForWallCollision(Vector2.up);
                if (!isWall)
                    isSnake = head.CheckForNodeCollision(Vector2.up);
                break;
            case Direction.down:
                isWall = head.CheckForWallCollision(Vector2.down);
                if (!isWall)
                    isSnake = head.CheckForNodeCollision(Vector2.down);
                break;
            case Direction.left:
                isWall = head.CheckForWallCollision(Vector2.left);
                if (!isWall)
                    isSnake = head.CheckForNodeCollision(Vector2.left);
                break;
            case Direction.right:
                isWall = head.CheckForWallCollision(Vector2.right);
                if (!isWall)
                    isSnake = head.CheckForNodeCollision(Vector2.right);
                break;
        }
        if (isWall || isSnake)
        {
            StartCoroutine(Die());
        }
    }

    void EatFood()
    {
        RaycastHit2D isFood = new RaycastHit2D();
        Direction direction = getCurrentHeading();
        switch (direction)
        {
            case Direction.up:
                isFood = head.CheckForFoodCollision(Vector2.up);
                break;
            case Direction.down:
                isFood = head.CheckForFoodCollision(Vector2.down);
                break;
            case Direction.left:
                isFood = head.CheckForFoodCollision(Vector2.left);
                break;
            case Direction.right:
                isFood = head.CheckForFoodCollision(Vector2.right);
                break;
        }
        if (isFood)
        {
            isFood.collider.gameObject.SetActive(false);
            AddSectionRpc(GetLastNode().gameObject.transform.position);
        }
    }

    [Rpc(SendTo.Server)]
    void AddSectionRpc(Vector2 position)
    {
        GameObject newNode = Instantiate(snakeNode);
        newNode.transform.position = position;
        SnakeNode newNodeScript = newNode.GetComponent<SnakeNode>();
        NetworkObject newNodeNetworkObject = newNode.GetComponent<NetworkObject>();
        newNodeNetworkObject.Spawn();
        if (head == null)
        {
            head = newNodeScript;
        }
        else
        {
            SnakeNode previous = GetLastNode();
            previous.next = newNodeScript;
            newNodeScript.previous = previous;
        }
    }

    IEnumerator Die()
    {
        SnakeNode current = head;
        head = null;
        dead = true;
        yield return new WaitForSeconds(.2f);
        while (current != null)
        {
            SnakeNode temp = current.next;
            current.KillNode();
            current = temp;
            yield return new WaitForSeconds(.075f);
        }
        StartCoroutine(Respawn());
    }
    SnakeNode GetLastNode()
    {
        SnakeNode current = head;
        while (current.next != null)
        {
            current = current.next;
        }
        return current;
    }

    void UpdateHeadings()
    {
        Vector2 newHeading = head.heading;
        head.heading = getNextHeading();
        SnakeNode current = head.next;
        while (current != null)
        {
            Vector2 temp = current.heading;
            current.heading = newHeading;
            newHeading = temp;
            current = current.next;
        }
    }

    Direction getCurrentHeading()
    {
        Direction direction = Direction.up;

        if (((Vector2)head.transform.position - head.heading).y < 0)
            direction = Direction.up;
        if (((Vector2)head.transform.position - head.heading).y > 0)
            direction = Direction.down;
        if (((Vector2)head.transform.position - head.heading).x > 0)
            direction = Direction.left;
        if (((Vector2)head.transform.position - head.heading).x < 0)
            direction = Direction.right;

        return direction;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(.5f);
        initSnake();
    }
}