using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoSingleton<PlayerController>
{
    private Rigidbody2D m_Rigidbody;
    private BoxCollider2D m_BoxCollider;

    private float speed = 5.0f;

    private Vector3 workspace;

    private void Start()
    {
        m_BoxCollider = gameObject.AddComponent<BoxCollider2D>();
        m_Rigidbody = gameObject.AddComponent<Rigidbody2D>();

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useFullKinematicContacts = true;
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");

        if (collision.gameObject.CompareTag("Frog"))
        {
            Debug.Log("hit frog");
            Master.Instance.Battle();
        }
    }

    public void Move(Vector3 dir)
    {
        workspace.Set(dir.x, dir.y, 0);
        transform.position += workspace * Time.deltaTime * speed;
    }
}
