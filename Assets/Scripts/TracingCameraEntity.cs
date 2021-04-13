using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingCameraEntity : MonoBehaviour
{
    public CarEntity targetObject;
    public float MOVING_THRESHOLD = 3f;

    Camera m_Camera;
    float m_OrthographicSize;
    float maxOrthographicSize;

    void Start()
    {
        m_Camera = this.GetComponent<Camera>();
        m_OrthographicSize = m_Camera.orthographicSize;
        maxOrthographicSize = m_OrthographicSize * 1.3f;
    }

    void LateUpdate()
    {
        Vector2 deltaPos = this.transform.position - targetObject.transform.position;
        //m_Camera.orthographicSize = m_OrthographicSize + targetObject.Velocity * 0.2f;
        m_Camera.orthographicSize += targetObject.Velocity * 0.001f;
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, m_OrthographicSize, maxOrthographicSize);

        if (deltaPos.magnitude > MOVING_THRESHOLD)
        {
            deltaPos.Normalize();
            Vector2 newPosition = new Vector2(targetObject.transform.position.x, targetObject.transform.position.y)
                + deltaPos * MOVING_THRESHOLD;

            this.transform.position = new Vector3(newPosition.x, newPosition.y, this.transform.position.z);
        }
    }
}
