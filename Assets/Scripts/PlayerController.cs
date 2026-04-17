using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float velocidade = 5f;
    public float forcaPulo = 5f;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private bool estaNoChao;

    private Animator animator;

    // Respawn
    private Vector3 posicaoInicial;

    // Vida
    public int vida = 3;
    public TextMeshProUGUI vidaTexto;
    public GameObject[] vidasUI;

    // Cooldown de dano
    private bool podeTomarDano = true;
    public float tempoInvencivel = 1f;

    // UI
    public GameObject gameOverUI;
    public GameObject winUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        posicaoInicial = transform.position;

        // garante que UI começa escondida
        gameOverUI.SetActive(false);
        winUI.SetActive(false);

        AtualizarUI();
    }

    void Update()
    {
        float movimento = Input.GetAxis("Horizontal");

        // movimento
        rb.velocity = new Vector2(movimento * velocidade, rb.velocity.y);

        // flip
        if (movimento > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movimento < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // chão
        estaNoChao = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // pulo
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
        }

        // animações
        animator.SetFloat("velocidade", Mathf.Abs(movimento));
        animator.SetFloat("velocidadeY", rb.velocity.y);
        animator.SetBool("estaNoChao", estaNoChao);
    }

    // Sistema de dano com cooldown
    void TomarDano()
    {
        if (!podeTomarDano) return;

        podeTomarDano = false;

        vida--;
        AtualizarUI();

        if (vida <= 0)
        {
            vida = 0;
            AtualizarUI();
            GameOver();
            return;
        }

        transform.position = posicaoInicial;

        Invoke("ResetarDano", tempoInvencivel);
    }

    void ResetarDano()
    {
        podeTomarDano = true;
    }

    // Atualiza UI
    void AtualizarUI()
    {
        for (int i = 0; i < vidasUI.Length; i++)
        {
            Image img = vidasUI[i].GetComponent<Image>();

            if (i < vida)
                img.color = Color.white;
            else
                img.color = Color.gray;
        }
    }

    // Game Over
    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Vitória
    void Vitoria()
    {
        winUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Triggers (DeathZone + Win)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            TomarDano();
        }

        if (other.CompareTag("Win"))
        {
            Vitoria();
        }
    }

    // Enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (rb.velocity.y < 0)
            {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.TomarDano();
                }

                // quicar
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
            }
            else
            {
                TomarDano();
            }
        }
    }

    // RESTART
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}