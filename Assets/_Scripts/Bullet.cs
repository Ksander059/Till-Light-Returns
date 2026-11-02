using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 10f; // Урон пули
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Игнорируем игрока и другие пули
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet")) return;

        // Проверяем, есть ли у объекта скрипт здоровья
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Уничтожаем пулю при попадании
        Destroy(gameObject);
    }
}