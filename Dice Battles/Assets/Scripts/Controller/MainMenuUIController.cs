using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject optionsPanel;

    private void OnEnable()
    {
        MainMenuController.stateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        MainMenuController.stateChanged -= OnStateChanged;
    }

    void OnStateChanged(MenuState newState)
    {
        DisablePanels();

        switch (newState)
        {
            case MenuState.Menu:
                menuPanel.SetActive(true);
                break;

            case MenuState.Inventory:
                inventoryPanel.SetActive(true);
                break;

            case MenuState.Shop:
                shopPanel.SetActive(true);
                break;

            case MenuState.Options:
                optionsPanel.SetActive(true);
                break;

            default:
                Debug.Log("Invalid menu state");
                break;
        }
    }

    private void DisablePanels()
    {
        menuPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }
}
