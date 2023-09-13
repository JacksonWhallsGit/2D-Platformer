using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAttack : MonoBehaviour
{
    [SerializeField] Transform player;
    public Animator anim;
    public GameObject fireball;
    public float speed = 20f;
    public SpriteRenderer froggy;
    public GameObject attackBall;
    float m_fireballTimer = 1.7f;
    bool m_playerInRange = false;
    float m_FlyTime = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player)
        {
            anim.SetBool("Spit", true);
            //Run once here for the first fireball, then update will take care of the fireballs occurring every 1.4 seconds.
            StartCoroutine(ShootFire());
            m_playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform == player)
        {
            anim.SetBool("Spit", false);
            m_playerInRange = false;
            m_fireballTimer = 1.7f;
        }
    }

    private IEnumerator ShootFire()
    {
        yield return new WaitForSeconds(0.8f);
        attackBall = Instantiate(fireball, froggy.transform.position + new Vector3(0f, 0f, 0), Quaternion.identity);
        m_FlyTime = 1.5f;
    }

    void Update()
    {
        m_FlyTime -= Time.deltaTime;
        if(m_FlyTime < 0)
        {
            Destroy(attackBall);
        }

        if (m_playerInRange)
        {
            m_fireballTimer -= Time.deltaTime;
            if(m_fireballTimer < 0f)
            {
                StartCoroutine(ShootFire());
                m_fireballTimer = 1.7f;
            }
        }


    }
}
