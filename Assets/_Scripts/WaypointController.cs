using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public Transform[] waypoints;
    float m_Speed = 6f;
    int m_CurrentIndex;

    private void Start()
    {
        m_CurrentIndex = 0;

    }

    void Update()
    {
        float step = m_Speed * Time.deltaTime;

        float distanceRemaining = Vector2.Distance(transform.position, waypoints[m_CurrentIndex].transform.position);

        if (distanceRemaining < 0.1f)
        {
            m_CurrentIndex++;
        }

        if(m_CurrentIndex + 1 > waypoints.Length)
        {
            m_CurrentIndex = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[m_CurrentIndex].transform.position, step);
    }
}
