using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float velocidade = 5f;

    private Rigidbody2D rb;
    private float movimentoInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Capture input in Update (called every frame)
    void Update()
    {
        movimentoInput = Input.GetAxis("Horizontal");
    }

    // Apply physics in FixedUpdate
    void FixedUpdate()
    {
        rb.velocity = new Vector2(movimentoInput * velocidade, rb.velocity.y);
    }
}