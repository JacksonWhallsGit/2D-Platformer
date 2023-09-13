using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRangedAttack : MonoBehaviour
{
    public float speed = 20f;
    public bool facingRight;
    public PlayerMovement playerMovement;
    public GameObject player;
    float m_direction = 1;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        facingRight = playerMovement.facingRight;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (facingRight)
        {
            m_direction = 1;
        } else
        {
            m_direction = -1;
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + m_direction, transform.position.y), step);
        transform.Rotate(0, 0, 1);
    }

}
