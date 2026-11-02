using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Настройки Здоровья")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Настройки Отбрасывания и Неуязвимости")]
    [SerializeField] private float iFrameDuration = 1f;
    [SerializeField] private float knockbackForce = 8f;
    private bool isInvulnerable = false;

    [Header("Ссылки на UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpFillImage;
    [SerializeField] private TextMeshProUGUI hpText; // Поле для текста HP

    [Header("Настройки Градиента / Переливания")]
    [SerializeField] private Color mainColor = new Color(0.8f, 0.1f, 0.1f);
    [SerializeField] private Color pulseColor = new Color(1f, 0.4f, 0.4f);
    [SerializeField] private float pulseSpeed = 3f;

    [Header("Ссылки на Игрока")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (hpFillImage == null && hpSlider != null)
        {
            hpFillImage = hpSlider.fillRect.GetComponent<Image>();
        }

        UpdateUI();
    }

    private void Update()
    {
        if (hpFillImage != null)
        {
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            hpFillImage.color = Color.Lerp(mainColor, pulseColor, t);
        }
    }

    public void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        // Отбрасывание
        Vector2 knockbackDir = ((Vector2)transform.position - attackerPosition).normalized;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(InvulnerabilityRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }

        if (hpText != null)
        {
            hpText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float flashInterval = 0.1f;
        for (float t = 0; t < iFrameDuration; t += flashInterval * 2)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(flashInterval);
        }

        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Рыцарь погиб!");
        gameObject.SetActive(false);
    }
}