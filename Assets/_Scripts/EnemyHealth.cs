using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth = 2;
    public GameEnding gameEnding;
    public Animator anim;
    public AudioSource explosionSound;
    AudioSource hitMark;
    bool m_IsDead;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        hitMark = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {

        if(currentHealth < 1 && !m_IsDead)
        {
            m_IsDead = true;
            anim.SetBool("Dead", true);
            explosionSound.Play();
            Destroy(gameObject.GetComponent<TouchHazard>());
            StartCoroutine(Die());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("playerAttack"))
        {
            currentHealth -= 1;
            hitMark.Play();
        }

        if(collision.gameObject.CompareTag("playerMelee"))
        {
            currentHealth -= 1;
            hitMark.Play();
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        if (gameObject.CompareTag("finalEnemy"))
        {
            gameEnding.EnemyKilled();
        }
    }
}
