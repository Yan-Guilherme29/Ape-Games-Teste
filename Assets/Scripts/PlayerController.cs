using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimento horizontal
        float movimento = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movimento * velocidade, rb.velocity.y);

        animator.SetFloat("velocidade", Mathf.Abs(movimento));

        if (movimento > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movimento < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Detectar se está no chão
        estaNoChao = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Debug (opcional)
        // Debug.Log(estaNoChao);

        // Pulo
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
        }
    }
}