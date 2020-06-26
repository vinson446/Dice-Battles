using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Basics")]
    public GameState gameState = GameState.None;
    public GameState priorGameState = GameState.None;
    public static Action<GameState> gameStateChanged = delegate { };
    GameUIController gameUIController;

    [Header("Player1 Choose Die")]
    DetectDiceCount dice;
    public int numSumDie = 0;
    public int numAtkDie = 0;
    public int numDefDie = 0;
    public int diceCount = 0;

    [Header("Player1 Turn")]
    public GameObject levelGameObj;
    public GameObject[] allTilesInGame;
    public GameObject selectedTile;
    public int selectedTileNum;
    public Material initialBorderMaterial;
    public Material selectedBorderMaterial;
    public Material firstSelectedBorderMaterial;
    public Material initialTileMaterial;
    public Material selectedTileMaterial;
    public Material firstSelectedTileMaterial;

    [Header("Player1 Summon 1")]
    int selectedSumMonIndex;
    PlayerPool playerPool;

    [Header("Confirm")]
    int selectedConfirm;

    [Header("Player1 Summon 2")]
    public int selectedDiePanel;
    public int[] selectedDiePanelIndexes;
    public List<int> confirmedDiePanelIndexes;
    bool canMove;
    public int spawnTileIndex;
    int previousDiePanel;

    private void Start()
    {
        gameUIController = FindObjectOfType<GameUIController>();
        dice = FindObjectOfType<DetectDiceCount>();

        // player turn- get all tiles in game
        for (int i = 0; i < 247; i++)
        {
            allTilesInGame[i] = levelGameObj.transform.GetChild(i).gameObject;
        }
        selectedTileNum = 19;
        selectedTile = allTilesInGame[selectedTileNum];

        // player sum 2
        previousDiePanel = 0;
        selectedDiePanelIndexes[0] = 18;
        selectedDiePanelIndexes[1] = 19;
        selectedDiePanelIndexes[2] = 20;
        selectedDiePanelIndexes[3] = 32;
        selectedDiePanelIndexes[4] = 45;
        selectedDiePanelIndexes[5] = 58;

        canMove = true;
        spawnTileIndex = 3;

        ChangeGameState(0);
    }

    public void ChangeGameState(int index)
    {
        priorGameState = gameState;
        gameState = (GameState)index;
        gameStateChanged.Invoke(gameState);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ChangeGameState(1);
            // TODO- DELETE LATER
            gameUIController.camOverhead90.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ChangeGameState(6);
        }

        // 2- player choose die
        if (gameState == GameState.Player1ChooseDie)
        {
            // show continue text
            if (diceCount == 3)
            {
                gameUIController.startToContinueText.SetActive(true);
            }
            else
            {
                gameUIController.startToContinueText.SetActive(false);
            }

            // reset dice count
            if (Input.GetKey(KeyCode.X))
            {
                dice.ResetCount();
                gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text = "0";
                gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text = "0";
                gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text = "0";
            }

            // go to roll dice game state and reset dice count
            if (Input.GetKey(KeyCode.Return) && diceCount == 3)
            {
                numSumDie = System.Convert.ToInt32(gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text);
                numAtkDie = System.Convert.ToInt32(gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text);
                numDefDie = System.Convert.ToInt32(gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text);

                ChangeGameState(3);

                dice.ResetCount();
                gameUIController.summonCountText.GetComponent<TextMeshProUGUI>().text = "0";
                gameUIController.attackCountText.GetComponent<TextMeshProUGUI>().text = "0";
                gameUIController.defenseCountText.GetComponent<TextMeshProUGUI>().text = "0";
            }
        }
        // 4- player turn
        else if (gameState == GameState.Player1Turn && gameUIController.playerMainPhasePanel.activeInHierarchy)
        {
            // initial selection
            for (int i = 0; i < selectedTile.transform.childCount; i++)
            {
                MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                if (i == 0)
                    meshRenderer.material = firstSelectedTileMaterial;
                else
                    meshRenderer.material = firstSelectedBorderMaterial; 
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // reset current tile
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (selectedTile.layer == 9)
                    {
                        if (i == 0)
                            meshRenderer.material = initialTileMaterial;
                        else
                            meshRenderer.material = initialBorderMaterial;
                    }
                    else if (selectedTile.layer == 10)
                    {
                        if (i == 0)
                            meshRenderer.material = selectedTileMaterial;
                        else
                            meshRenderer.material = selectedBorderMaterial;
                    }
                }

                // change selected tile if tile isn't null
                if ((selectedTileNum - 1 >= 0) && (selectedTileNum % 13 != 0))
                    selectedTileNum -= 1;
                else
                    selectedTileNum += 12;
                selectedTile = allTilesInGame[selectedTileNum];

                // highlight new selected tile
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (i == 0)
                        meshRenderer.material = firstSelectedBorderMaterial;
                    else
                        meshRenderer.material = firstSelectedTileMaterial;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (selectedTile.layer == 9)
                    {
                        if (i == 0)
                            meshRenderer.material = initialTileMaterial;
                        else
                            meshRenderer.material = initialBorderMaterial;
                    }
                    else if (selectedTile.layer == 10)
                    {
                        if (i == 0)
                            meshRenderer.material = selectedTileMaterial;
                        else
                            meshRenderer.material = selectedBorderMaterial;
                    }
                }

                if ((selectedTileNum + 1 <= 246) && (System.Convert.ToInt32(selectedTile.name) % 13 != 0))
                    selectedTileNum += 1;
                else
                    selectedTileNum -= 12;
                selectedTile = allTilesInGame[selectedTileNum];

                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (i == 0)
                        meshRenderer.material = firstSelectedTileMaterial;
                    else
                        meshRenderer.material = firstSelectedBorderMaterial;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (selectedTile.layer == 9)
                    {
                        if (i == 0)
                            meshRenderer.material = initialTileMaterial;
                        else
                            meshRenderer.material = initialBorderMaterial;
                    }
                    else if (selectedTile.layer == 10)
                    {
                        if (i == 0)
                            meshRenderer.material = selectedTileMaterial;
                        else
                            meshRenderer.material = selectedBorderMaterial;
                    }
                }

                if ((selectedTileNum - 13 >= 0))
                    selectedTileNum -= 13;
                else
                {
                    int remainder = Mathf.Abs(selectedTileNum - 13);
                    selectedTileNum = 247 - remainder;
                }
                selectedTile = allTilesInGame[selectedTileNum];

                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (i == 0)
                        meshRenderer.material = firstSelectedTileMaterial;
                    else
                        meshRenderer.material = firstSelectedBorderMaterial;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (selectedTile.layer == 9)
                    {
                        if (i == 0)
                            meshRenderer.material = initialTileMaterial;
                        else
                            meshRenderer.material = initialBorderMaterial;
                    }
                    else if (selectedTile.layer == 10)
                    {
                        if (i == 0)
                            meshRenderer.material = selectedTileMaterial;
                        else
                            meshRenderer.material = selectedBorderMaterial;
                    }
                }

                if ((selectedTileNum + 13 <= 246))
                    selectedTileNum += 13;
                else
                {
                    int remainder = Mathf.Abs(selectedTileNum + 13 - 247);
                    selectedTileNum = 0 + remainder;
                }
                selectedTile = allTilesInGame[selectedTileNum];

                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (i == 0)
                        meshRenderer.material = firstSelectedTileMaterial;
                    else
                        meshRenderer.material = firstSelectedBorderMaterial;
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    MeshRenderer meshRenderer = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>();
                    if (selectedTile.layer == 9)
                    {
                        if (i == 0)
                            meshRenderer.material = initialTileMaterial;
                        else
                            meshRenderer.material = initialBorderMaterial;
                    }
                    else if (selectedTile.layer == 10)
                    {
                        if (i == 0)
                            meshRenderer.material = selectedTileMaterial;
                        else
                            meshRenderer.material = selectedBorderMaterial;
                    }
                }
                selectedTileNum = 19;
                selectedTile = allTilesInGame[selectedTileNum];

                ChangeGameState(5);
            }
        }
        
        // 5- player summon
        else if (gameState == GameState.Player1SumMon1 && gameUIController.playerSumMon1Panel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedSumMonIndex - 1 >= 0)
                    selectedSumMonIndex -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedSumMonIndex + 1 <= 7)
                    selectedSumMonIndex += 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedSumMonIndex - 4 >= 0)
                    selectedSumMonIndex -= 4;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedSumMonIndex + 4 <= 7)
                    selectedSumMonIndex += 4;
            }

            gameUIController.ShowSelectedMonOnSum1Panel(selectedSumMonIndex);

            // z during 
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeGameState(23);
            }
        }

        // 23- player confirm when selecting a monster (player1 summon 1)
        else if (gameState == GameState.Confirm && priorGameState == GameState.Player1SumMon1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedConfirm - 1 >= 0)
                    selectedConfirm -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedConfirm + 1 <= 1)
                    selectedConfirm += 1;
            }

            // going to placing monster (player1 summon 2)
            if (Input.GetKeyDown(KeyCode.Z) && selectedConfirm == 0)
            {
                gameUIController.summonWall.GetComponent<Animator>().SetBool("raise", false);
                for (int i = 0; i < gameUIController.spawnedMonsterReplicas.Length; i++)
                    Destroy(gameUIController.spawnedMonsterReplicas[i]);

                selectedSumMonIndex = 0;
                selectedConfirm = 0;
                ChangeGameState(6);
            }
            // going back to selecting a monster (player1 summon 1)
            else if (Input.GetKeyDown(KeyCode.X) || (Input.GetKeyDown(KeyCode.Z) && selectedConfirm == 1))
            {
                selectedConfirm = 0;
                ChangeGameState(5);
            }
        }

        // player sum 2
        else if (gameState == GameState.Player1SumMon2 && gameUIController.player1SumMon2Panel.activeInHierarchy)
        {
            gameUIController.ShowSelectedMonOnSum2Panel(selectedSumMonIndex);

            // move selected die panel on field
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // check if can move
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    if ((selectedDiePanelIndexes[i] - 1 < 0) || (selectedDiePanelIndexes[i] % 13 == 0))
                        canMove = false;
                }
                // move all dile panel indexes
                if (canMove)
                {
                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                    {
                        for (int j = 0; j < selectedTile.transform.childCount; j++)
                        {
                            MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                            if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 9)
                            {
                                if (j == 0)
                                    meshRenderer.material = initialTileMaterial;
                                else
                                    meshRenderer.material = initialBorderMaterial;
                            }
                            else if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 10)
                            {
                                if (j == 0)
                                    meshRenderer.material = selectedTileMaterial;
                                else
                                    meshRenderer.material = selectedBorderMaterial;
                            }
                        }
                    }

                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                        selectedDiePanelIndexes[i] -= 1;
                }
                canMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // check if can move
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    if ((selectedDiePanelIndexes[i] + 1 > 246) || ((selectedDiePanelIndexes[i] + 1) % 13 == 0))
                        canMove = false;
                }
                // move all dile panel indexes
                if (canMove)
                {
                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                    {
                        for (int j = 0; j < selectedTile.transform.childCount; j++)
                        {
                            MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                            if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 9)
                            {
                                if (j == 0)
                                    meshRenderer.material = initialTileMaterial;
                                else
                                    meshRenderer.material = initialBorderMaterial;
                            }
                            else if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 10)
                            {
                                if (j == 0)
                                    meshRenderer.material = selectedTileMaterial;
                                else
                                    meshRenderer.material = selectedBorderMaterial;
                            }
                        }
                    }

                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                        selectedDiePanelIndexes[i] += 1;
                }
                canMove = true; 
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // check if can move
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    if ((selectedDiePanelIndexes[i] - 13 < 0) && (selectedDiePanelIndexes[i] % 13 != 0))
                        canMove = false;
                }
                // move all dile panel indexes
                if (canMove)
                {
                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                    {
                        for (int j = 0; j < selectedTile.transform.childCount; j++)
                        {
                            MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                            if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 9)
                            {
                                if (j == 0)
                                    meshRenderer.material = initialTileMaterial;
                                else
                                    meshRenderer.material = initialBorderMaterial;
                            }
                            else if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 10)
                            {
                                if (j == 0)
                                    meshRenderer.material = selectedTileMaterial;
                                else
                                    meshRenderer.material = selectedBorderMaterial;
                            }
                        }
                    }

                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                        selectedDiePanelIndexes[i] -= 13;
                }
                canMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // check if can move
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    if ((selectedDiePanelIndexes[i] + 13 > 246) && (selectedDiePanelIndexes[i] % 13 != 0))
                        canMove = false;
                }
                // move all dile panel indexes
                if (canMove)
                {
                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                    { 
                        for (int j = 0; j < selectedTile.transform.childCount; j++)
                        {
                            MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                            if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 9)
                            {
                                if (j == 0)
                                    meshRenderer.material = initialTileMaterial;
                                else
                                    meshRenderer.material = initialBorderMaterial;
                            }
                            else if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 10)
                            {
                                if (j == 0)
                                    meshRenderer.material = selectedTileMaterial;
                                else
                                    meshRenderer.material = selectedBorderMaterial;
                            }
                        }
                    }

                    for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                        selectedDiePanelIndexes[i] += 13;
                }
                canMove = true;
            }
            // change die panel
            else if (Input.GetKeyDown(KeyCode.A))
            {
                // clear
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    for (int j = 0; j < selectedTile.transform.childCount; j++)
                    {
                        MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                        if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 9)
                        {
                            if (j == 0)
                                meshRenderer.material = initialTileMaterial;
                            else
                                meshRenderer.material = initialBorderMaterial;
                        }
                        else if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 10)
                        {
                            if (j == 0)
                                meshRenderer.material = selectedTileMaterial;
                            else
                                meshRenderer.material = selectedBorderMaterial;
                        }
                    }
                }

                if (selectedDiePanel - 1 >= 0)
                {
                    previousDiePanel = selectedDiePanel;
                    selectedDiePanel -= 1;

                    if (selectedDiePanel == 0)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                    }
                    else if (selectedDiePanel == 1)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                    }
                    else if (selectedDiePanel == 2)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                    }
                    else if (selectedDiePanel == 3)
                    {
                        selectedDiePanelIndexes[0] += 26;
                        selectedDiePanelIndexes[2] -= 13;
                    }
                    else if (selectedDiePanel == 4)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                    }
                    else if (selectedDiePanel == 5)
                    {
                        selectedDiePanelIndexes[0] += 13;
                        selectedDiePanelIndexes[1] -= 1;
                    }
                    else if (selectedDiePanel == 6)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                    }
                    else if (selectedDiePanel == 7)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                    }
                    else if (selectedDiePanel == 8)
                    {
                        selectedDiePanelIndexes[5] += 14;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    for (int j = 0; j < selectedTile.transform.childCount; j++)
                    {
                        MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                        if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 9)
                        {
                            if (j == 0)
                                meshRenderer.material = initialTileMaterial;
                            else
                                meshRenderer.material = initialBorderMaterial;
                        }
                        else if (allTilesInGame[selectedDiePanelIndexes[i]].layer == 10)
                        {
                            if (j == 0)
                                meshRenderer.material = selectedTileMaterial;
                            else
                                meshRenderer.material = selectedBorderMaterial;
                        }
                    }
                }

                if (selectedDiePanel + 1 <= 8)
                {
                    previousDiePanel = selectedDiePanel;
                    selectedDiePanel += 1;

                    if (selectedDiePanel == 0)
                    {

                    }
                    else if (selectedDiePanel == 1)
                    {
                        selectedDiePanelIndexes[0] += 13;
                    }
                    else if (selectedDiePanel == 2)
                    {
                        selectedDiePanelIndexes[0] += 13;
                    }
                    else if (selectedDiePanel == 3)
                    {
                        selectedDiePanelIndexes[0] += 13;
                    }
                    else if (selectedDiePanel == 4)
                    {
                        selectedDiePanelIndexes[0] -= 26;
                        selectedDiePanelIndexes[2] += 13;
                    }
                    else if (selectedDiePanel == 5)
                    {
                        selectedDiePanelIndexes[0] += 13;
                    }
                    else if (selectedDiePanel == 6)
                    {
                        selectedDiePanelIndexes[0] -= 13;
                        selectedDiePanelIndexes[1] += 1;
                    }
                    else if (selectedDiePanel == 7)
                    {
                        selectedDiePanelIndexes[0] += 13;
                    }
                    else if (selectedDiePanel == 8)
                    {
                        selectedDiePanelIndexes[0] += 13;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeGameState(23);
            }
            else
            {
                // always highlight current die panel by changing material to firstSelect
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    for (int j = 0; j < selectedTile.transform.childCount; j++)
                    {
                        MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                        if (j == 0)
                            meshRenderer.material = firstSelectedTileMaterial;
                        else
                            meshRenderer.material = firstSelectedBorderMaterial;
                    }
                }
            }
        }

        // 23- player confirm when selecting a monster (player1 summon 1)
        else if (gameState == GameState.Confirm && priorGameState == GameState.Player1SumMon2)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedConfirm - 1 >= 0)
                    selectedConfirm -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedConfirm + 1 <= 1)
                    selectedConfirm += 1;
            }

            // going to placing monster (player1 summon 2)
            if (Input.GetKeyDown(KeyCode.Z) && selectedConfirm == 0)
            {
                for (int i = 0; i < selectedDiePanelIndexes.Length; i++)
                {
                    allTilesInGame[selectedDiePanelIndexes[i]].layer = 10;

                    for (int j = 0; j < selectedTile.transform.childCount; j++)
                    {
                        allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).gameObject.layer = 10;
                        MeshRenderer meshRenderer = allTilesInGame[selectedDiePanelIndexes[i]].transform.GetChild(j).GetComponent<MeshRenderer>();
                        if (j == 0)
                            meshRenderer.material = selectedTileMaterial;
                        else
                            meshRenderer.material = selectedBorderMaterial;
                    }
                }

                for (int i = 0; i < 6; i++)
                    confirmedDiePanelIndexes.Add(selectedDiePanelIndexes[i]);

                // reset to intial state
                previousDiePanel = 0;
                selectedDiePanel = 0;
                selectedDiePanelIndexes[0] = 18;
                selectedDiePanelIndexes[1] = 19;
                selectedDiePanelIndexes[2] = 20;
                selectedDiePanelIndexes[3] = 32;
                selectedDiePanelIndexes[4] = 45;
                selectedDiePanelIndexes[5] = 58;

                selectedConfirm = 0;

                ChangeGameState(4);
            }
            // going back to selecting a monster (player1 summon 1)
            else if (Input.GetKeyDown(KeyCode.X) || (Input.GetKeyDown(KeyCode.Z) && selectedConfirm == 1))
            {
                selectedConfirm = 0;

                ChangeGameState(6);
            }
        }
    }
}
