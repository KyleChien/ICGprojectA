using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEntity : MonoBehaviour
{
    SpriteRenderer m_TargetRenderer;

    void Start()
    {
        m_TargetRenderer = this.GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        m_TargetRenderer.color = Color.red;
    }

    void OnCollisionStay2D(Collision2D collision)
    {

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        m_TargetRenderer.color = Color.white;
    }
}
