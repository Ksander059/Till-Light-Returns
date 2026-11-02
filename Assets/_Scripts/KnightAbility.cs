using System.Collections;
using UnityEngine;

public class KnightAbility : MonoBehaviour
{
    [Header("Настройки Способности")]
    [SerializeField] private float cooldown = 8f;            // Перезарядка (секунды)
    [SerializeField] private float duration = 4f;            // Длительность действия
    [SerializeField] private float fireRateMultiplier = 2.5f;// Во сколько раз быстрее стреляем

    [Header("Ссылки")]
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private SpriteRenderer playerSprite;

    private bool isCooldown = false;
    private Color originalColor;

    private void Start()
    {
        if (weaponSystem == null)
            weaponSystem = GetComponentInChildren<WeaponSystem>();

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();

        if (playerSprite != null)
            originalColor = playerSprite.color;
    }

    private void Update()
    {
        // Активация способности на ПРОБЕЛ
        if (Input.GetKeyDown(KeyCode.Space) && !isCooldown)
        {
            StartCoroutine(ActivateRage());
        }
    }

    private IEnumerator ActivateRage()
    {
        isCooldown = true;

        // 1. Увеличиваем скорострельность (уменьшаем задержку между выстрелами)
        float originalFireRate = weaponSystem.fireRate;
        weaponSystem.fireRate /= fireRateMultiplier;

        // 2. Включаем визуальный эффект (покраснение рыцаря в знак ярости)
        if (playerSprite != null)
            playerSprite.color = new Color(1f, 0.3f, 0.3f); // Красный оттенок

        // 3. Ждем время действия способности
        yield return new WaitForSeconds(duration);

        // 4. Возвращаем исходную скорострельность и цвет
        weaponSystem.fireRate = originalFireRate;
        if (playerSprite != null)
            playerSprite.color = originalColor;

        // 5. Ждем оставшееся время перезарядки
        yield return new WaitForSeconds(cooldown - duration);

        isCooldown = false;
        Debug.Log("Способность 'Ярость' снова готова!");
    }
}