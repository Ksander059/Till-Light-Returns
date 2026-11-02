using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    private float currentHealth;

    private SpriteRenderer sr;
    private Color originalColor; // Запоминаем родной цвет

    private void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            originalColor = sr.color; // Сохраняем исходный (зеленый) цвет
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " получил урон! Осталось ХП: " + currentHealth);

        if (sr != null)
        {
            sr.color = Color.red;
            Invoke(nameof(ResetColor), 0.1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetColor()
    {
        if (sr != null)
        {
            sr.color = originalColor; // Возвращаем именно родной цвет!
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}