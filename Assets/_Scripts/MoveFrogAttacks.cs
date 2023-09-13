using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFrogAttacks : MonoBehaviour
{
    public float speed = 20f;
    public Transform player;
    public float playerDirection;
    public float projectileDirection = -1;
    public GameEnding gameEnding;

    void Start()
    {
        playerDirection = player.position.x - transform.position.x;
        if(playerDirection > 0)
        {
            projectileDirection = 1;
        } else
        {
            projectileDirection = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + projectileDirection, transform.position.y), step);
        transform.Rotate(0, 0, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform == player)
        {
            gameEnding.TouchedPlayer();
        }
    }
}
