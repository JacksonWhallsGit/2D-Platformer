using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideObject : MonoBehaviour
{
    public Transform player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform == player)
        {
            player.transform.SetParent(transform);
            player.transform.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        player.transform.SetParent(null);
        player.transform.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
    }
}
