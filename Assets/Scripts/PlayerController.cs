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

        animator.SetFloat("velocidade", Mathf.Abs(movimento));
        animator.SetFloat("velocidadeY", rb.velocity.y);
        animator.SetBool("estaNoChao", estaNoChao);
    }
}