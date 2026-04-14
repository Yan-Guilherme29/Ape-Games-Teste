using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // 👉 Respawn
    private Vector3 posicaoInicial;

    // 👉 Vida
    public int vida = 3;
    public TextMeshProUGUI vidaTexto;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // salva posição inicial
        posicaoInicial = transform.position;

        // atualiza UI
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

        // animações (sempre no final)
        animator.SetFloat("velocidade", Mathf.Abs(movimento));
        animator.SetFloat("velocidadeY", rb.velocity.y);
        animator.SetBool("estaNoChao", estaNoChao);
    }

    // 👉 Sistema de dano
    void TomarDano()
    {
        vida--;

        AtualizarUI();

        if (vida <= 0)
        {
            vida = 3;
            AtualizarUI();
        }

        transform.position = posicaoInicial;
    }

    // 👉 Atualiza texto da UI
    void AtualizarUI()
    {
        vidaTexto.text = "Vida: " + vida;
    }

    // 👉 DeathZone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            TomarDano();
        }
    }

    // 👉 Enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TomarDano();
        }
    }
}