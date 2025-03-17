using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Movimiento
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    // Vida
    public int health = 3;
    public GameObject gameOverMesh; // Para TextMesh
    public TextMesh livesTextMesh; // Referencia directa al TextMesh de vidas

    // Control de estado del juego
    private bool isGameOver = false;

    // Referencias
    private Rigidbody2D rb;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    // Sonido
    public AudioSource jumpSound;
    public AudioSource hurtSound;

    // Posición inicial para respawn
    private Vector3 startPosition;

    // Referencia al GameManager
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Guardar posición inicial
        startPosition = transform.position;

        // Asegurarse de que el mesh de Game Over esté desactivado al inicio
        if (gameOverMesh != null)
        {
            gameOverMesh.SetActive(false);
        }

        // Asegurar que empezamos con 3 vidas
        health = 3;
        isGameOver = false;

        // Actualizar el texto de vidas al inicio
        UpdateLivesText();

        // Desactivar Play On Awake en los sonidos
        if (jumpSound != null)
        {
            jumpSound.playOnAwake = false;
        }
        if (hurtSound != null)
        {
            hurtSound.playOnAwake = false;
        }

        // Encontrar el GameManager
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogWarning("No se encontró el GameManager en la escena");
        }
    }

    void Update()
    {
        // Si el juego ha terminado, no procesar lógica de juego
        if (isGameOver)
        {
            // Solo verificar si se presiona R para reiniciar
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        // Comprobar si el jugador está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Visualización del área de detección
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(groundCheck.position, Vector2.left * groundCheckRadius, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(groundCheck.position, Vector2.right * groundCheckRadius, isGrounded ? Color.green : Color.red);

        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                if (jumpSound != null) jumpSound.Play();
            }
        }

        // Verificar si el jugador ha caído fuera del nivel
        if (transform.position.y < -30)
        {
            TakeDamage();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el juego ha terminado, ignorar colisiones
        if (isGameOver) return;

        // Colisión con enemigos
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("¡COLISIÓN CON ENEMIGO! - Vidas antes: " + health);
            TakeDamage();
        }
    }

    // Para objetos que son triggers (como la meta)
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Si el juego ha terminado, ignorar triggers
        if (isGameOver) return;

        // Llegar a la meta
        if (collider.gameObject.CompareTag("Goal"))
        {
            Debug.Log("¡He llegado a la meta!");
            ResetLevel();
        }

        // También podemos detectar enemigos como triggers si los configuramos así
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("¡TRIGGER CON ENEMIGO! - Vidas antes: " + health);
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        // No tomar daño si el juego ha terminado
        if (isGameOver) return;

        health--;
        Debug.Log("DENTRO DE TakeDamage - Vidas reducidas a: " + health);

        // Asegurarse de que la salud no baje de 0
        if (health < 0)
        {
            health = 0;
        }

        // Actualizar el texto de vidas
        UpdateLivesText();

        if (hurtSound != null) hurtSound.Play();

        if (health <= 0)
        {
            Debug.Log("Sin vidas restantes - Llamando a GameOver()");
            GameOver();
        }
        else
        {
            Debug.Log("Respawneando jugador a posición inicial");
            // Respawn al inicio del nivel
            transform.position = startPosition;
        }
    }

    void UpdateLivesText()
    {
        // Actualizar el texto directamente si tenemos referencia
        if (livesTextMesh != null)
        {
            livesTextMesh.text = "Vidas: " + health;
            Debug.Log("TextMesh actualizado: " + livesTextMesh.text);
        }
        else
        {
            Debug.LogWarning("No hay referencia a livesTextMesh");
        }

        // También intentar actualizar a través del GameManager
        if (gameManager != null)
        {
            gameManager.UpdateLivesUI();
        }
    }

    void GameOver()
    {
        // Marcar el juego como terminado
        isGameOver = true;

        Debug.Log("GAME OVER EJECUTADO");

        // Detener el movimiento del jugador
        rb.velocity = Vector2.zero;

        // Mostrar el panel de Game Over
        if (gameOverMesh != null)
        {
            gameOverMesh.SetActive(true);
            Debug.Log("Panel Game Over activado");
        }
        else
        {
            Debug.LogError("La referencia a gameOverMesh es nula");
        }

        // También intentar mostrar a través del GameManager
        if (gameManager != null)
        {
            gameManager.ShowGameOver();
        }
    }

    void RestartGame()
    {
        Debug.Log("REINICIANDO JUEGO");

        // Ocultar el panel de Game Over
        if (gameOverMesh != null)
        {
            gameOverMesh.SetActive(false);
        }

        // Reiniciar vidas y posición
        health = 3;
        transform.position = startPosition;

        // Reactivar el juego
        isGameOver = false;

        // Actualizar UI
        UpdateLivesText();

        Debug.Log("Juego reiniciado - Vidas: " + health);
    }

    void ResetLevel()
    {
        Debug.Log("NIVEL COMPLETADO - RESETEANDO");

        // En lugar de cargar otra escena, simplemente reinicia la posición
        transform.position = startPosition;

        // Opcional: recuperar vidas
        health = 3;
        UpdateLivesText();

        Debug.Log("Nivel reseteado - Nueva posición: " + transform.position);
    }

    // Método para que el GameManager acceda a la posición inicial
    public Vector3 GetStartPosition()
    {
        return startPosition;
    }

    // Esta función dibuja gizmos en el editor para visualizar mejor el groundCheck
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            // Dibuja una esfera en la posición del groundCheck con el radio especificado
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}