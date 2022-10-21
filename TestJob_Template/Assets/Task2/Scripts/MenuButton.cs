using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] AddItem addItem;
    [SerializeField] Text buttonText;
    [SerializeField] TMP_Text xMarkText;
    [SerializeField] GameObject button;

    Coroutine PositionReturnCoroutine;

    private float positionReturnWaitTime = 0.0001f;
    private float changeDistance = 1f;
    public int clickCount { get; set; }

    private IEnumerator DragPositionReturn() /*Корутин обрабатывает положение в зависиости 
    от позиции в которой был отпущен курсор в методе OnDrag
    -60 - положение к которыму возращается кнопка, если находится  в диапазоне (-70;-50)
    0 - положение к которому возвращается кнопка, если находится в диапазоне (-50;0)*/
    {
        Vector3 intermediatePosition = button.transform.localPosition;

        if (button.transform.localPosition.x <= -60)
        {
            for (; button.transform.localPosition.x < -60; intermediatePosition.x += changeDistance)
            {
                button.transform.localPosition = intermediatePosition;
                yield return new WaitForSeconds(positionReturnWaitTime);
            }
        }
        else if (button.transform.localPosition.x > -60 && button.transform.localPosition.x <= -50)
        {
            for (; button.transform.localPosition.x > -60; intermediatePosition.x -= changeDistance)
            {
                button.transform.localPosition = intermediatePosition;
                yield return new WaitForSeconds(positionReturnWaitTime);
            }
        }
        else if (button.transform.localPosition.x > -50)
        {
            for (; button.transform.localPosition.x <= 0; intermediatePosition.x += changeDistance)
            {
                button.transform.localPosition = intermediatePosition;
                yield return new WaitForSeconds(positionReturnWaitTime);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.delta.x < 0)      //Свайп кнопки влево
            if (button.transform.localPosition.x > -70)
            {
                button.transform.localPosition += new Vector3(eventData.delta.x, 0, 0);
                if (button.transform.localPosition.x < -70) //Ограничение, чтобы при высоких значениях delta.x кнопка не выехала за допустимые границы
                    button.transform.localPosition =
                        new Vector3(-70, button.transform.localPosition.y, button.transform.localPosition.z);
            }

        if (eventData.delta.x > 0)      //Свайп кнопки вправо
            if (button.transform.localPosition.x < 0)
            {
                button.transform.localPosition += new Vector3(eventData.delta.x, 0, 0);
                if (button.transform.localPosition.x > 0) //Ограничение, чтобы при высоких значения delta.x кнопка не выехала за допустимые границы
                    button.transform.localPosition =
                        new Vector3(0, button.transform.localPosition.y, button.transform.localPosition.z);
            }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PositionReturnCoroutine != null)
            StopCoroutine(PositionReturnCoroutine); //Остановка действуещего корутина, если во время его работы еще раз потянуть за кнопку
        PositionReturnCoroutine = StartCoroutine(DragPositionReturn());
    }

    private void OnMouseDown()
    {
        clickCount++;
        this.SetText();
    }

    public void ResetPosition()
    {
        button.transform.localPosition =
            new Vector3(0, button.transform.localPosition.y, button.transform.localPosition.z);
    }

    public void SetText()
    {
        if (clickCount == 1)
            buttonText.text = $"- Item - {{{clickCount}}} click";
        else
            buttonText.text = $"- Item - {{{clickCount}}} clicks";
    }

    private void DeleteButton()
    {
        if (this.name != "ClickButton") //Невозможно удалить оригинальную кнопку
        {   
            Destroy(this.gameObject);
            addItem.buttonCounter--;
        }
    }

    private void Update()
    {
        float alphaColor = (Mathf.Abs(button.transform.localPosition.x) / 100) * 2f;
        xMarkText.color = 
            new Color(xMarkText.color.r, xMarkText.color.g, xMarkText.color.b, alphaColor - 0.2f); // -0.2f позволяет отметке проявлятся позже, делая анимацию более заметной
    }
}
