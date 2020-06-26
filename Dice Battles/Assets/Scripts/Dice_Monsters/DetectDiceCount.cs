using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectDiceCount : MonoBehaviour
{
    GameController gameController;
    GameUIController gameUIController;

    int summonCount = 0;
    int attackCount = 0;
    int defenseCount = 0;

    int selected = 0;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameUIController = FindObjectOfType<GameUIController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selected -= 1;

            if (selected < 0)
                selected = 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selected += 1;

            if (selected > 2)
                selected = 0;
        }

        // summon
        if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 0 && summonCount == 0 && gameController.diceCount < 3)
        {
            summonCount = 1;
            gameController.diceCount += 1;
            gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text = summonCount.ToString();     
        }
        else if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 0 && summonCount == 1 && gameController.diceCount < 3)
        {
            summonCount = 2;
            gameController.diceCount += 1;
            gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text = summonCount.ToString();
        }
        else if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 0 && summonCount == 2 && gameController.diceCount < 3)
        {
            summonCount = 3;
            gameController.diceCount += 1;
            gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text = summonCount.ToString();
        }

        // attack
        if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 1 && attackCount == 0 && gameController.diceCount < 3)
        {
            attackCount = 1;
            gameController.diceCount += 1;
            gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text = attackCount.ToString();
        }
        else if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 1 && attackCount == 1 && gameController.diceCount < 3)
        {
            attackCount = 2;
            gameController.diceCount += 1;
            gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text = attackCount.ToString();
        }
        else if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 1 && attackCount == 2 && gameController.diceCount < 3)
        {
            attackCount = 3;
            gameController.diceCount += 1;
            gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text = attackCount.ToString();
        }

        // defend
        if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 2 && defenseCount == 0 && gameController.diceCount < 3)
        {
            defenseCount = 1;
            gameController.diceCount += 1;
            gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text = defenseCount.ToString();
        }
        else if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 2 && defenseCount == 1 && gameController.diceCount < 3)
        {
            defenseCount = 2;
            gameController.diceCount += 1;
            gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text = defenseCount.ToString();
        }
        else if (gameController.gameState == (GameState)2 && Input.GetKeyDown(KeyCode.Z) && selected == 2 && defenseCount == 2 && gameController.diceCount < 3)
        {
            defenseCount = 3;
            gameController.diceCount += 1;
            gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text = defenseCount.ToString();
        }
    }

    // testing
    /*
    private void OnMouseDown()
    {
        if (gameObject.tag == "SummonDice" && gameController.diceCount < 3)
        {
            summonCount += 1;
            gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text = summonCount.ToString();
            gameController.diceCount += 1;
        }
        else if (gameObject.tag == "AttackDice" && gameController.diceCount < 3)
        {
            attackCount += 1;
            gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text = attackCount.ToString();
            gameController.diceCount += 1;
        }
        else if (gameObject.tag == "DefenseDice" && gameController.diceCount < 3)
        {
            defenseCount += 1;
            gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text = defenseCount.ToString();
            gameController.diceCount += 1;
        }
    }
    */

    public void ResetCount()
    {
        selected = 0;
        summonCount = 0;
        attackCount = 0;
        defenseCount = 0;
        gameController.diceCount = 0;
    }
}
