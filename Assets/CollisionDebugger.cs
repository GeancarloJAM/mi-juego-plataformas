using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    // Solo para depuración
    public bool showDebugMessages = true;
    public Color gizmoColor = Color.red;

    void Start()
    {
        // Verificar componentes al inicio
        CheckComponents();
    }

    void CheckComponents()
    {
        // Verificar si tiene Collider2D
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("DEPURACIÓN: " + gameObject.name + " NO tiene Collider2D!");
        }
        else
        {
            if (collider.isTrigger)
            {
                Debug.Log("DEPURACIÓN: " + gameObject.name + " tiene Collider2D marcado como Trigger.");
            }
            else
            {
                Debug.Log("DEPURACIÓN: " + gameObject.name + " tiene Collider2D normal (no trigger).");
            }
        }

        // Verificar si tiene Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("DEPURACIÓN: " + gameObject.name + " NO tiene Rigidbody2D!");
        }
        else
        {
            Debug.Log("DEPURACIÓN: " + gameObject.name + " tiene Rigidbody2D con bodyType: " + rb.bodyType);
        }

        // Verificar tag
        Debug.Log("DEPURACIÓN: " + gameObject.name + " tiene tag: " + gameObject.tag);

        // Verificar layer
        Debug.Log("DEPURACIÓN: " + gameObject.name + " está en la capa: " + LayerMask.LayerToName(gameObject.layer));
    }

    // Detectar colisiones normales
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (showDebugMessages)
        {
            Debug.Log("DEPURACIÓN: " + gameObject.name + " COLISIONÓ con " + collision.gameObject.name);
        }
    }

    // Detectar triggers
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (showDebugMessages)
        {
            Debug.Log("DEPURACIÓN: " + gameObject.name + " detectó TRIGGER de " + collider.gameObject.name);
        }
    }

    // Visualizar colliders en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            if (collider is BoxCollider2D)
            {
                BoxCollider2D boxCollider = (BoxCollider2D)collider;
                // Dibujar un cubo que represente el collider
                Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
            }
            else if (collider is CircleCollider2D)
            {
                CircleCollider2D circleCollider = (CircleCollider2D)collider;
                // Dibujar un círculo que represente el collider
                Gizmos.DrawWireSphere(circleCollider.bounds.center, circleCollider.radius);
            }
        }
    }
}