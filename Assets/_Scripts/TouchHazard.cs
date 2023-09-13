using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHazard : MonoBehaviour
{

    public Transform player;
    public GameEnding gameEnding;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player)
        {
            gameEnding.TouchedPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            gameEnding.TouchedPlayer();
        }
    }
}
