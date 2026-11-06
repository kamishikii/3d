using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки скорости")]
    public float walkSpeed = 5f;
    public float flySpeed = 10f;
    
    [Header("Физика")]
    public float gravity = -15f;
    
    [Header("Ссылки")]
    public Transform cameraTransform; // Сюда перетащишь Main Camera

    private CharacterController controller;
    private Vector3 velocity;
    private bool isFlying = false;
    
    // Переменные для ввода с джойстика
    private Vector2 moveInput; 
    private float verticalInput; // Для управления вверх/вниз в полете

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Направление движения относительно камеры (чтобы идти туда, куда смотришь)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        // Убираем влияние наклона камеры по Y, чтобы не улетать в небо при взгляде вниз
        forward.y = 0; 
        right.y = 0; 
        forward.Normalize();
        right.Normalize();

        // Базовое движение по земле (X и Z)
        Vector3 moveDirection = (right * moveInput.x + forward * moveInput.y).normalized;

        if (isFlying)
        {
            // В режиме полета гравитация не работает
            velocity.y = 0; 
            
            // Двигаемся по горизонтали
            controller.Move(moveDirection * flySpeed * Time.deltaTime);
            
            // По вертикали управляем через отдельный ввод (вверх/вниз)
            Vector3 verticalMove = Vector3.up * verticalInput * flySpeed * Time.deltaTime;
            controller.Move(verticalMove);
        }
        else
        {
            // Обычная ходьба с гравитацией
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Прижимаем к земле
            }

            velocity.y += gravity * Time.deltaTime;
            
            controller.Move(moveDirection * walkSpeed * Time.deltaTime);
            controller.Move(velocity * Time.deltaTime);
        }
    }

    // --- Методы для вызова из UI ---
    public void SetMoveInput(Vector2 input) { moveInput = input; }
    public void SetVerticalInput(float input) { verticalInput = input; }
    
    public void ToggleFlight()
    {
        isFlying = !isFlying;
        velocity.y = 0; // Сбрасываем инерцию при переключении
    }
}
