using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Ajustes de movimiento")]
    public float speed = 3f;
    private bool movinRight = true;

    [Header("Detectores")]
    public Transform groundCheck;
    public float distanceToGround = 1.5f;
    public float distanceToWall = 1.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Si no se ha asignado groundCheck en el inspector, no ejecuta la lógica para evitar errores
        if (groundCheck == null) return;

        rb.linearVelocity = new Vector2(movinRight ? speed : -speed, rb.linearVelocity.y);
        Vector2 direction = movinRight ? Vector2.right : Vector2.left;

        RaycastHit2D isGroundHead = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);
        RaycastHit2D isWallsAhead = Physics2D.Raycast(groundCheck.position, direction, distanceToWall, groundLayer);

        if (isGroundHead.collider == null || isWallsAhead.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movinRight = !movinRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        // Protege ambas líneas para que OnDrawGizmos solo dibuje si groundCheck existe
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheck.position, Vector2.down * distanceToGround);

            Vector2 direction = movinRight ? Vector2.right : Vector2.left;
            Gizmos.DrawRay(groundCheck.position, direction * distanceToWall);
        }
    }
}