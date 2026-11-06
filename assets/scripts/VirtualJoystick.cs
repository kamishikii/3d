using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform background; // Фон джойстика
    public RectTransform handle;     // Ползунок (ручка)
    
    public float handleRange = 50f;  // Радиус хода ручки
    
    private Vector2 inputVector;
    public System.Action<Vector2> OnJoystickMove; // Событие для передачи данных

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            inputVector = pos / handleRange;
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Двигаем ручку
            handle.anchoredPosition = inputVector * handleRange;
            
            OnJoystickMove?.Invoke(inputVector);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        OnJoystickMove?.Invoke(inputVector);
    }
}
