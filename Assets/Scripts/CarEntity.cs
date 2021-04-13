using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEntity : MonoBehaviour
{
    public GameObject wheelFrontRight;
    public GameObject wheelFrontLeft;
    public GameObject wheelBackRight;
    public GameObject wheelBackLeft;

    // car steering
    float m_FrontWheelAngle = 0;
    const float WHEEL_ANGLE_LIMIT = 20f;
    public float turnAngularVelocity = 40f;

    // accelerate and decelerate
    float m_Velocity;
    public float Velocity { get { return m_Velocity; } }
    public float acceleration;
    public float deceleration;
    public float maxVelocity;
    float m_DeltaMovement;

    // collision
    [SerializeField] SpriteRenderer[] m_Renderers = new SpriteRenderer[5];
    CheckPoint checkpoint;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // speed up
            m_Velocity = Mathf.Min(maxVelocity, m_Velocity + Time.fixedDeltaTime * acceleration);
        }

        if (!Input.anyKey)
        {
            // friction
            m_Velocity = Mathf.MoveTowards(m_Velocity, 0, Time.fixedDeltaTime * acceleration);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            // break
            m_Velocity = Mathf.Max(-maxVelocity, m_Velocity - Time.fixedDeltaTime * deceleration);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // turn left
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle + Time.fixedDeltaTime * turnAngularVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT);
            UpdateWheels();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // turn right
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle - Time.fixedDeltaTime * turnAngularVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT);
            UpdateWheels();
        }
        m_DeltaMovement = m_Velocity * Time.fixedDeltaTime;

        this.transform.Rotate(0f, 0f,
            1 / 1.0f *
            Mathf.Tan(Mathf.Deg2Rad * m_FrontWheelAngle) *
            m_DeltaMovement *
            Mathf.Rad2Deg);
        this.transform.Translate(Vector3.right * m_DeltaMovement);

    }

    void UpdateWheels()
    {
        // update wheels by m_FrontWheelAngle
        Vector3 localEulerAngles = new Vector3(0f, 0f, m_FrontWheelAngle);
        wheelFrontLeft.transform.localEulerAngles = localEulerAngles;
        wheelFrontRight.transform.localEulerAngles = localEulerAngles;

    }

    void ChangeColor(Color color)
    {
        foreach (SpriteRenderer r in m_Renderers)
        {
            r.color = color;
        }
    }

    void ResetColor()
    {
        ChangeColor(Color.white);
    }
    void Stop()
    {
        m_Velocity = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeColor(Color.red);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        ChangeColor(Color.red);
        Stop();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        ResetColor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        checkpoint = other.gameObject.GetComponent<CheckPoint>();
        if (checkpoint != null)
        {
            ChangeColor(Color.green);
            this.Invoke("ResetColor", 1.0f);
            checkpoint = null;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (checkpoint == null)
            ChangeColor(Color.red);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (checkpoint == null)
            ResetColor();
    }
}
