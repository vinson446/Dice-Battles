using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainMenuController : MonoBehaviour
{
    public MenuState state { get; private set; } = MenuState.None;
    public static event Action<MenuState> stateChanged = delegate { };

    private void Start()
    {
        ChangeState(1);
    }

    // change int index to menu state
    public void ChangeState(int stateIndex)
    {
        state = (MenuState)stateIndex;
        stateChanged.Invoke(state);
    }
}
