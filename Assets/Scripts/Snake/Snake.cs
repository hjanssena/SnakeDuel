using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class Snake : NetworkBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction currentDir;
    public Direction lastDir;
    public SnakeNode head;
    [SerializeField]
    public GameObject snakeNode;
    bool dead;
    PlayerSnake player;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
            initSnake();
    }

    void Update()
    {
        if (!dead && IsServer)
        {
            move();
            EatFood();
            CheckCollision();
        }
    }

    public void setPlayer(PlayerSnake player)
    {
        this.player = player;
    }
    
    void initSnake()
    {
        AddSection(transform.position);
        AddSection(new Vector2(transform.position.x, transform.position.y - 1));
        AddSection(new Vector2(transform.position.x, transform.position.y - 2));
        dead = false;
    }

    void move()
    {
        if ((Vector2)head.gameObject.transform.position == head.heading)
        {
            updateDirection();
            UpdateNodeHeadings();
        }
    }


    void UpdateNodeHeadings()
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

    void updateDirection()
    {
        Direction currentHeading = getCurrentHeading();
        Direction playerDir = player.currentDir.Value;
        if (playerDir == Direction.up)
        {
            if (currentDir != Direction.down)
            {
                lastDir = currentDir;
                currentDir = Direction.up;
            }
        }
        else if (playerDir == Direction.down)
        {
            Direction h = getCurrentHeading();
            if (currentDir != Direction.up)
            {
                lastDir = currentDir;
                currentDir = Direction.down;
            }

        }
        else if (playerDir == Direction.left)
        {
            if (currentDir != Direction.right)
            {
                lastDir = currentDir;
                currentDir = Direction.left;
            }
        }
        else if (playerDir == Direction.right)
        {
            if (currentDir != Direction.left)
            {
                lastDir = currentDir;
                currentDir = Direction.right;
            }
        }
    }

    Vector2 getNextHeading()
    {
        Vector2 next = new Vector2(head.gameObject.transform.position.x, head.gameObject.transform.position.y);
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
            isFood.collider.gameObject.GetComponent<Food>().Despawn();
            AddSection(GetLastNode().gameObject.transform.position);
        }
    }

    void AddSection(Vector2 position)
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