using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteeringWheel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float InputDirection { get; private set; }

    public static event Action<float> onSteeringWheelRotate;

    private Image steeringWheel_base;
    private Image steeringWheel_wheel;

    private float bgImageSizeX;
    private float bgImageSizeY;

    private float offsetFactorWithBgSize = 0.5f;

    private void Start()
    {
        steeringWheel_base = GetComponent<Image>();
        steeringWheel_wheel = transform.Find("Steering Wheel").GetComponent<Image>();
        bgImageSizeX = steeringWheel_base.GetComponent<RectTransform>().sizeDelta.x;
        bgImageSizeY = steeringWheel_base.GetComponent<RectTransform>().sizeDelta.y;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 tappedPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(steeringWheel_base.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 touchPoint))
        {
            //Debug.Log(touchPoint);
            tappedPoint.x = touchPoint.x / (bgImageSizeX * offsetFactorWithBgSize);
            tappedPoint.y = touchPoint.y / (bgImageSizeY * offsetFactorWithBgSize);
            Debug.Log(tappedPoint);
            SetSteeringWheelDirection(tappedPoint);
            Debug.Log(InputDirection);

            steeringWheel_wheel.rectTransform.rotation = Quaternion.Euler(0, 0, InputDirection);

            onSteeringWheelRotate?.Invoke(InputDirection);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = 0;
        steeringWheel_wheel.rectTransform.rotation = Quaternion.Euler(Vector3.zero);
        onSteeringWheelRotate?.Invoke(InputDirection);
    }

    private void SetSteeringWheelDirection(Vector2 direction)
    {
        if (direction.x < 0)
        {
            InputDirection = 180 * Mathf.Acos((direction.x * Vector2.up.x + direction.y * Vector2.up.y) / (direction.magnitude * Vector2.up.magnitude)) / Mathf.PI;
        }
        else if (direction.x > 0)
        {
            InputDirection = -180 * Mathf.Acos((direction.x * Vector2.up.x + direction.y * Vector2.up.y) / (direction.magnitude * Vector2.up.magnitude)) / Mathf.PI;
        }
        else
        {
            InputDirection = 0;
        }

    }
}
