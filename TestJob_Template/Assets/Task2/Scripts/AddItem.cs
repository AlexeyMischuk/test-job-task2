using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    [SerializeField] MenuButton button;
    [SerializeField] GameObject parentObject;

    public int buttonCounter { get; set; }

    private void AddButton()
    {
        MenuButton newButton = Instantiate(button);
        newButton.transform.SetParent(parentObject.transform);
        newButton.clickCount = 0;
        newButton.SetText();
        newButton.name = $"ClickButton (Clone{buttonCounter})";
        buttonCounter++;   
        ResetAllPositions();
    }

    private void ResetAllPositions()
    {
        GameObject[] buttonList = GameObject.FindGameObjectsWithTag("MenuButton");
        foreach (var menuButton in buttonList)
        {
            menuButton.SendMessage("ResetPosition");
        }
    }

    private void Start()
    {
        buttonCounter = 1;
    }
}
