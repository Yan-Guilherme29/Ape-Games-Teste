using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float velocidade = 2f;
    private bool movendoDireita = true;

    public Transform groundCheck;
    public float checkDistance = 1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    // 👉 VIDA DO INIMIGO
    public int vida = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // movimento
        if (movendoDireita)
        {
            rb.velocity = new Vector2(velocidade, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-velocidade, rb.velocity.y);
        }

        // detectar borda
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);

        if (!hit)
        {
            Virar();
        }
    }

    void Virar()
    {
        movendoDireita = !movendoDireita;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // 👉 FUNÇÃO DE DANO
    public void TomarDano()
    {
        vida--;

        // efeito visual simples (opcional)
        StartCoroutine(DanoFlash());

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 👉 efeito de hit (piscar)
    System.Collections.IEnumerator DanoFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
        }
    }
}