using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Настройки Стрельбы")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f; // Задержка между выстрелами (секунды)

    private float nextFireTime = 0f;

    private void Update()
    {
        // Стрельба на зажатую Левую Кнопку Мыши (LMB)
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Создаем пулю в точке FirePoint с её поворотом
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}