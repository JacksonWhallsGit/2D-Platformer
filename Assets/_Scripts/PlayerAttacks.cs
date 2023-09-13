using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public Animator anim;
    public GameObject rangedAttack;
    public BoxCollider2D meleeHitbox;
    public SpriteRenderer meleeHitboxRenderer;
    public GameObject attackBall;

    float rangedAttackBuffer = 2f;
    float meleeBuffer = 0.3f;
    float m_FlyTime = 1f;

    void Update()
    {
        rangedAttackBuffer -= Time.deltaTime;
        meleeBuffer -= Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.J)) && meleeBuffer < 0)
        {
            anim.SetBool("isAttacking", true);
            meleeHitbox.size = new Vector2(1.3f, 1.3f);
            meleeHitbox.offset = new Vector2(0.3f, 0.15f);
            meleeHitboxRenderer.color = new Color(0, 188, 255, 0.5f);
            StartCoroutine(StopAttack());
            meleeBuffer = 0.3f;
        }

        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.K)) && rangedAttackBuffer < 0.1)
        {
            anim.SetBool("isAttacking", true);
            StartCoroutine(StopAttack());
            attackBall = Instantiate(rangedAttack, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            rangedAttackBuffer = 2f;
            m_FlyTime = 1f;
        }

        m_FlyTime -= Time.deltaTime;
        if (m_FlyTime < 0)
        {
            Destroy(attackBall);
        }
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("isAttacking", false);
        meleeHitbox.size = new Vector2(0.03f, 0.03f);
        meleeHitbox.offset = new Vector2(-1f, 0);
        meleeHitboxRenderer.color = new Color(0, 188, 255, 0);
    }
}
