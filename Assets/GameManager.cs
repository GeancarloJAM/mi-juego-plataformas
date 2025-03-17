using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Referencia al jugador
    public PlayerController player;

    // UI con TextMesh en vez de Canvas
    public TextMesh livesTextMesh;
    public GameObject gameOverMesh;

    // Audio
    public AudioSource backgroundMusic;

    void Start()
    {
        // Asegurarse de que el Game Over mesh está desactivado al inicio
        if (gameOverMesh != null)
        {
            gameOverMesh.SetActive(false);
        }

        // Iniciar música de fondo
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }

        UpdateLivesUI();

        // Comprobación de referencias
        if (player == null)
        {
            Debug.LogWarning("GameManager: No hay referencia al jugador");
            player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                Debug.Log("GameManager: Jugador encontrado automáticamente");
            }
        }

        if (livesTextMesh == null)
        {
            Debug.LogWarning("GameManager: No hay referencia al texto de vidas");
        }
    }

    void Update()
    {
        // Actualizar la UI
        UpdateLivesUI();

        // Si el jugador presiona ESC, pausar/reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Reiniciar con R cuando se muestra el Game Over
        if (gameOverMesh != null && gameOverMesh.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void UpdateLivesUI()
    {
        if (livesTextMesh != null && player != null)
        {
            livesTextMesh.text = "Vidas: " + player.health;
            Debug.Log("Actualizando UI de vidas: " + player.health);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverMesh != null)
        {
            gameOverMesh.SetActive(true);
            Debug.Log("Mostrando pantalla de Game Over");
        }
    }

    public void RestartGame()
    {
        // Ocultar el panel de Game Over
        if (gameOverMesh != null)
        {
            gameOverMesh.SetActive(false);
        }

        // Reiniciar el nivel actual
        if (player != null)
        {
            player.health = 3;
            player.transform.position = player.GetStartPosition();
            Debug.Log("Juego reiniciado");
        }

        // Reanudar el tiempo si está pausado
        Time.timeScale = 1f;
    }

    void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            // Reanudar
            Time.timeScale = 1f;
            Debug.Log("Juego reanudado");
        }
        else
        {
            // Pausar
            Time.timeScale = 0;
            Debug.Log("Juego pausado");
        }
    }
}