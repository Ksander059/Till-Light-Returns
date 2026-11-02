using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки Движения")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Ссылки на Объекты")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform weaponPivot;

    private Vector2 moveInput;
    private Vector2 mousePos;
    private Camera mainCamera;

    private void Start()
    {
        // Если забыл привязать Rigidbody2D в инспекторе, скрипт найдёт его сам
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 1. Считываем ввод с клавиш WASD / Стрелочек
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Нормализуем вектор, чтобы по диагонали рыцарь не бегал быстрее
        moveInput = moveInput.normalized;

        // 2. Получаем позицию курсора мыши в игровом мире
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // Движение персонажа через физику (без рывков)
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Поворот оружия в сторону мышки
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        if (weaponPivot == null) return;

        // Вычисляем вектор направления от Pivot оружия к мышке
        Vector2 lookDir = mousePos - (Vector2)weaponPivot.position;

        // Находим угол в градусах
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Применяем поворот по оси Z
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, angle);

        // Отражаем оружие по вертикали, если смотрим влево, чтобы оно не оказывалось перевёрнутым
        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        weaponPivot.localScale = localScale;
    }
}