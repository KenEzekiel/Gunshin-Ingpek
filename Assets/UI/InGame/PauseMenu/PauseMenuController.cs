using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : InGameUIScreenController
{


    public void Awake()
    {
    }


    public new void OnEnable()
    {
        base.OnEnable();
        Button resumeButton = rootElement.Query<Button>("Resume");
        resumeButton.RegisterCallback((ClickEvent evt) =>
        {
            GameController.stateController.PopState();
        });

        // Button saveButton = rootElement.Query<Button>("Save");
        // // Insert condition on which player can save the game
        // if (false)
        // {
        //     saveButton.style.display = DisplayStyle.None;
        // }
        // saveButton.RegisterCallback((ClickEvent evt) =>
        // {
        //     Debug.Log("Save Button clicked");
        //     GameSaveManager.Instance?.SaveGame();
        // });

        // Button shopButton = rootElement.Query<Button>("Shop");
        // shopButton.RegisterCallback((ClickEvent evt) =>
        // {
        //     GameController.Instance.stateController.PushState(GameState.SHOPPING);
        // });

        Button returnToMainMenuButton = rootElement.Query<Button>("ReturnToMainMenu");

        returnToMainMenuButton.RegisterCallback((ClickEvent evt) =>
        {
            ScenesManager.Instance.LoadMainMenu();
        });
    }
}
