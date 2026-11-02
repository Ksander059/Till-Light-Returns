using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float attackCooldown = 1f; // Задержка между ударами (1 секунда)

    private float nextAttackTime = 0f;
    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    // Вызывается КАЖДЫЙ КАДР, пока слайм касается игрока
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Проверяем, прошло ли время перезарядки атаки
            if (Time.time >= nextAttackTime)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage, transform.position);
                    nextAttackTime = Time.time + attackCooldown; // Задаем время следующего удара
                }
            }
        }
    }
}