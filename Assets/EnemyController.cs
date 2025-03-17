using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool isMoving = true;

    // Para enemigos que se mueven de lado a lado
    public float moveDistance = 2f;
    private Vector3 startingPosition;
    private bool movingRight = true;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveEnemy();
        }
    }

    void MoveEnemy()
    {
        if (movingRight)
        {
            // Mover a la derecha
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

            // Verificar si ha alcanzado el límite derecho
            if (transform.position.x >= startingPosition.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            // Mover a la izquierda
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            // Verificar si ha alcanzado el límite izquierdo
            if (transform.position.x <= startingPosition.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }
}