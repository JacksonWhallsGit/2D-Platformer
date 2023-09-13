using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAttack : MonoBehaviour
{
    public SpriteRenderer eagle;
    public Transform player;
    public float speed = 3f;
    public bool m_PlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player)
        {
            m_PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform == player)
        {
            m_PlayerInRange = false;
        }
    }

    void Update()
    {
        if (m_PlayerInRange)
        {
            if((player.position.x - eagle.transform.position.x) < 0)
            {
                eagle.flipX = false;
            } else
            {
                eagle.flipX = true;
            }
            float step = speed * Time.deltaTime;
            eagle.transform.position = Vector2.MoveTowards(eagle.transform.position, player.position, step);
        }
    }
}
