using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    GameController gameController;

    [Header("Player Start Turn")]
    public GameObject camOverhead55;
    public GameObject camFPV;
    public GameObject playerStartPanel;
    public TextMeshProUGUI playerStartText;
    public Vector3 playerStartBegPlace;
    public float playerStartMidPlace;
    public float playerStartEndPlace;
    public float playerStartMidDuration;
    public float playerStartEndDuration;
    public float playerStartBegTransition;
    public float playerStartMidTransition;
    public float playerStartEndTransition;

    [Header("Player1 Choose Die")]
    public GameObject camChooseDie;
    public GameObject playerChooseDiePanel;
    public GameObject[] dice;
    public GameObject select3DiceText;
    public GameObject summonText;
    public GameObject summonCountText;
    public GameObject attackText;
    public GameObject attackCountText;
    public GameObject defenseText;
    public GameObject defenseCountText;
    public GameObject startToContinueText;
    public GameObject xToResetText;
    public GameObject zToSelectText;
    public float chooseDieMessageDuration;

    [Header("Player1 Roll Die")]
    public GameObject playerRollDiePanel;
    public TextMeshProUGUI summonRollText;
    public TextMeshProUGUI attackRollText;
    public TextMeshProUGUI defenseRollText;
    public int[] dieToThrow;
    public GameObject[] diceToActuallyThrow;
    public Transform[] diceSpawnPoints;
    public GameObject[] dieRollWalls;
    public int currentSumCount = 0;
    public int currentAtkCount = 0;
    public int currentDefCount = 0;
    public GameObject armToAnimate;
    public float throwForce;

    [Header("Player1 Turn")]
    public GameObject playerMainPhasePanel;
    public GameObject camOverhead90;
    // reset this shit on game over
    public int sumCounter1;
    public int atkCounter1;
    public int defCounter1;
    public int sumCounter2;
    public int atkCounter2;
    public int defCounter2;
    public TextMeshProUGUI oppSumCounter;
    public TextMeshProUGUI oppAtkCounter;
    public TextMeshProUGUI oppDefCounter;
    public TextMeshProUGUI mySumCounter;
    public TextMeshProUGUI myAtkCounter;
    public TextMeshProUGUI myDefCounter;

    [Header("Player1 Summon 1")]
    public GameObject playerSumMon1Panel;
    public TextMeshProUGUI selectedMonNameText1;
    public TextMeshProUGUI selectedSumText1;
    public TextMeshProUGUI selectedMovText1;
    public TextMeshProUGUI selectedHPText1;
    public TextMeshProUGUI selectedATKText1;
    public TextMeshProUGUI selectedDEFText1;

    public Transform[] spawnLocations;
    public GameObject summonWall;
    PlayerPool playerPool;
    public GameObject[] spawnedMonsterReplicas;

    [Header("Confirm")]
    public GameObject confirmPanel;

    [Header("Player1 Summon 2")]
    public GameObject player1SumMon2Panel;
    public GameObject player1SumMon2HolderPanel;
    public Image selectedMonImage;
    public TextMeshProUGUI selectedMonNameText2;
    public TextMeshProUGUI selectedSumText2;
    public TextMeshProUGUI selectedMovText2;
    public TextMeshProUGUI selectedHPText2;
    public TextMeshProUGUI selectedATKText2;
    public TextMeshProUGUI selectedDEFText2;

    private void OnEnable()
    {
        GameController.gameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameController.gameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        playerPool = FindObjectOfType<PlayerPool>();
    }

    void OnGameStateChanged(GameState state)
    {
        DisablePanels();

        switch (state)
        {
            // player 1
            // 1
            case GameState.Player1StartTurn:
                camOverhead55.SetActive(true);
                playerStartText.transform.localPosition = playerStartBegPlace;
                playerStartText.text = "Player 1 Turn";
                playerStartPanel.SetActive(true);

                StartCoroutine(PlayerStartTurn());
                break;
   
            // 2
            case GameState.Player1ChooseDie:
                playerChooseDiePanel.SetActive(true);

                StartCoroutine(Select3DiceMessage());
                break;

            // 3
            case GameState.Player1RollDie:
                TurnOffChooseDieStuff();
                for (int i = 0; i < dieRollWalls.Length; i++)
                    dieRollWalls[i].GetComponent<Animator>().SetBool("raise", true);
                playerRollDiePanel.SetActive(true);

                StartCoroutine(RollDice());
                break;

            // 4
            case GameState.Player1Turn:
                if (gameController.priorGameState == (GameState)3)
                {
                    camFPV.SetActive(true);
                    camChooseDie.SetActive(false);
                }

                playerStartText.transform.localPosition = playerStartBegPlace;
                playerStartText.text = "Player 1 Main Phase";
                playerStartPanel.SetActive(true);

                StartCoroutine(PlayerStartTurn());
                break;

            // 5
            case GameState.Player1SumMon1:
                StartCoroutine(PlayerSummonPhase1());
                break;

            // 6
            case GameState.Player1SumMon2:
                player1SumMon2Panel.SetActive(true);
                StartCoroutine(PlayerSumMon2());

                break;

            // 7
            case GameState.Player1MoveMon:
                break;

            // 8
            case GameState.Player1Attack:
                break;

            // 9
            case GameState.Player1Defend:
                break;

            // 10
            case GameState.Player1EndTurn:
                break;

            
            // player 2
            // 11
            case GameState.Player2StartTurn:
                playerStartText.transform.localPosition = playerStartBegPlace;
                playerStartText.text = "Player 2 Turn";
                playerStartPanel.SetActive(true);

                StartCoroutine(PlayerStartTurn());
                break;

            // 12
            case GameState.Player2ChooseDie:
                break;

            // 13
            case GameState.Player2RollDie:
                break;

            // 14
            case GameState.Player2Turn:
                break;

            // 15
            case GameState.Player2SumMon1:
                break;

            // 16
            case GameState.Player2SumMon2:
                break;

            // 17
            case GameState.Player2MoveMon:
                break;

            // 18
            case GameState.Player2Attack:
                break;

            // 19
            case GameState.Player2Defend:
                break;

            // 20
            case GameState.Player2EndTurn:
                break;

            // misc
            // 21
            case GameState.ShowMonDeck:
                break;

            // 22
            case GameState.Options:
                break;

            // 23
            case GameState.Confirm:
                confirmPanel.SetActive(true);
                break;
        }
    }

    IEnumerator PlayerStartTurn()
    {
        yield return new WaitForSeconds(playerStartBegTransition);

        playerStartText.gameObject.transform.DOLocalMoveX(playerStartMidPlace, playerStartMidDuration, false);

        yield return new WaitForSeconds(playerStartMidTransition);

        playerStartText.gameObject.transform.DOLocalMoveX(playerStartEndPlace, playerStartEndDuration, false);

        yield return new WaitForSeconds(playerStartEndTransition);

        camFPV.SetActive(true);
        camOverhead55.SetActive(false);

        if (gameController.gameState == (GameState)1)
        {
            yield return new WaitForSeconds(1);
            gameController.ChangeGameState(2);
        }
        else if (gameController.gameState == (GameState)4)
        {
            DisablePanels();
            StartCoroutine(PlayerMainPhase());
        }
    }

    IEnumerator Select3DiceMessage()
    {
        yield return new WaitForSeconds(1);

        camChooseDie.SetActive(true);
        camFPV.SetActive(false);

        yield return new WaitForSeconds(chooseDieMessageDuration);

        for (int i = 0; i < dice.Length; i++)
            dice[i].SetActive(true);

        select3DiceText.SetActive(true);

        summonText.SetActive(true);
        summonCountText.SetActive(true);
        attackText.SetActive(true);
        attackCountText.SetActive(true);
        defenseText.SetActive(true);
        defenseCountText.SetActive(true);

        xToResetText.SetActive(true);
        zToSelectText.SetActive(true);
    }

    void TurnOffChooseDieStuff()
    {
        for (int i = 0; i < dice.Length; i++)
            dice[i].SetActive(false);

        select3DiceText.SetActive(false);

        summonText.SetActive(false);
        summonCountText.SetActive(false);
        attackText.SetActive(false);
        attackCountText.SetActive(false);
        defenseText.SetActive(false);
        defenseCountText.SetActive(false);

        xToResetText.SetActive(false);
        zToSelectText.SetActive(false);
    }

    IEnumerator RollDice()
    {
        yield return new WaitForSeconds(1);

        // armToAnimate.GetComponent<Animator>().SetBool("throwDice", true);

        // get number of summon, attack, and defense dice to throw
        for (int i = 0; i < gameController.numSumDie; i++)
        {
            dieToThrow[i] = 1;
        }
        for (int i = 0; i < gameController.numAtkDie; i++)
        {
            if (dieToThrow[i + gameController.numSumDie] != 1)
                dieToThrow[i + gameController.numSumDie] = 2;
        }
        for (int i = 0; i < gameController.numDefDie; i++)
        {
            if (dieToThrow[i + gameController.numSumDie + gameController.numAtkDie] != 1 || dieToThrow[i + gameController.numSumDie + gameController.numAtkDie] != 2)
                dieToThrow[i + gameController.numSumDie + gameController.numAtkDie] = 3;
        }

        // summon selected dice and throw it
        for (int i = 0; i < dieToThrow.Length; i++)
        {
            if (dieToThrow[i] == 1)
            {
                GameObject sumDice = Instantiate(diceToActuallyThrow[0], diceSpawnPoints[i].position, Quaternion.Euler(Random.Range(0f, 360f), Random.Range (0f, 360f), Random.Range(0f, 360f)));
                sumDice.GetComponent<Rigidbody>().AddForce(diceSpawnPoints[i].forward * throwForce, ForceMode.VelocityChange);
            }
            else if (dieToThrow[i] == 2)
            {
                GameObject atkDice = Instantiate(diceToActuallyThrow[1], diceSpawnPoints[i].position, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
                atkDice.GetComponent<Rigidbody>().AddForce(diceSpawnPoints[i].forward * throwForce, ForceMode.VelocityChange);
            }
            else if (dieToThrow[i] == 3)
            {
                GameObject defDice = Instantiate(diceToActuallyThrow[2], diceSpawnPoints[i].position, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
                defDice.GetComponent<Rigidbody>().AddForce(diceSpawnPoints[i].forward * throwForce, ForceMode.VelocityChange);
            }
        }

        // wait for dice to land and get results
        yield return new WaitForSeconds(2);

        DiceResultMain[] diceResults = FindObjectsOfType<DiceResultMain>();

        for (int i = 0; i < diceResults.Length; i++)
        {
            currentSumCount += diceResults[i].sumNum;
            currentAtkCount += diceResults[i].atkNum;
            currentDefCount += diceResults[i].defNum;
        }

        // show dice results
        summonRollText.text = currentSumCount.ToString();
        attackRollText.text = currentAtkCount.ToString();
        defenseRollText.text = currentDefCount.ToString();

        // armToAnimate.GetComponent<Animator>().SetBool("throwDice", false);

        yield return new WaitForSeconds(2);

        gameController.ChangeGameState(4);
        CleanUpDiceRollStuff();
    }

    void CleanUpDiceRollStuff()
    {
        DiceResultMain[] diceResults = FindObjectsOfType<DiceResultMain>();

        for (int i = 0; i < diceResults.Length; i++)
        {
            Destroy(diceResults[i].gameObject);
        }

        // add to total counter (player)
        sumCounter2 += currentSumCount;
        atkCounter2 += currentAtkCount;
        defCounter2 += currentDefCount;

        currentSumCount = 0;
        currentAtkCount = 0;
        currentDefCount = 0;
        summonRollText.text = "0";
        attackRollText.text = "0";
        defenseRollText.text = "0";
        gameController.numSumDie = 0;
        gameController.numAtkDie = 0;
        gameController.numDefDie = 0;

        for (int i = 0; i < dieRollWalls.Length; i++)
            dieRollWalls[i].GetComponent<Animator>().SetBool("raise", false);
    }

    IEnumerator PlayerMainPhase()
    {
        camOverhead90.SetActive(true);
        camFPV.SetActive(false);
        
        // update total player counter text
        mySumCounter.text = sumCounter2.ToString();
        myAtkCounter.text = atkCounter2.ToString();
        myDefCounter.text = defCounter2.ToString();

        yield return new WaitForSeconds(2);

        playerMainPhasePanel.SetActive(true);
    }

    IEnumerator PlayerSummonPhase1()
    {
        if (gameController.gameState == (GameState)5 && gameController.priorGameState == (GameState)23)
        {

        }
        else
        {
            camOverhead90.SetActive(false);
            camFPV.SetActive(true);

            yield return new WaitForSeconds(2);

            camChooseDie.SetActive(true);
            camFPV.SetActive(false);

            yield return new WaitForSeconds(1);

            summonWall.GetComponent<Animator>().SetBool("raise", true);

            yield return new WaitForSeconds(1);

            // spawn player's monsters on desk
            for (int i = 0; i < playerPool.monsters.Length; i++)
            {
                GameObject mon = Instantiate(playerPool.monsters[i].monster, spawnLocations[i].position, Quaternion.identity);
                mon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                mon.GetComponent<MonsterMovement>().enabled = false;
                spawnedMonsterReplicas[i] = mon;
            }
        }

        playerSumMon1Panel.SetActive(true);
    }

    public void ShowSelectedMonOnSum1Panel(int selectedMonIndex)
    {
        selectedMonNameText1.text = playerPool.monsters[selectedMonIndex].name;
        selectedSumText1.text = playerPool.monsters[selectedMonIndex].summonCost.ToString();
        selectedMovText1.text = playerPool.monsters[selectedMonIndex].movement.ToString();
        selectedHPText1.text = playerPool.monsters[selectedMonIndex].hp.ToString();
        selectedATKText1.text = playerPool.monsters[selectedMonIndex].attack.ToString();
        selectedDEFText1.text = playerPool.monsters[selectedMonIndex].defense.ToString();
    }

    IEnumerator PlayerSumMon2()
    {
        camOverhead90.SetActive(true);
        camChooseDie.SetActive(false);

        yield return new WaitForSeconds(2);

        player1SumMon2HolderPanel.SetActive(true);
    }

    public void ShowSelectedMonOnSum2Panel(int selectedMonIndex)
    {
        selectedMonImage.color = Color.green;
        selectedMonNameText2.text = playerPool.monsters[selectedMonIndex].name;
        selectedSumText2.text = playerPool.monsters[selectedMonIndex].summonCost.ToString();
        selectedMovText2.text = playerPool.monsters[selectedMonIndex].movement.ToString();
        selectedHPText2.text = playerPool.monsters[selectedMonIndex].hp.ToString();
        selectedATKText2.text = playerPool.monsters[selectedMonIndex].attack.ToString();
        selectedDEFText2.text = playerPool.monsters[selectedMonIndex].defense.ToString();
    }

    void DisablePanels()
    {
        playerStartPanel.SetActive(false);
        playerChooseDiePanel.SetActive(false);
        playerRollDiePanel.SetActive(false);
        playerMainPhasePanel.SetActive(false);
        playerSumMon1Panel.SetActive(false);

        confirmPanel.SetActive(false);

        player1SumMon2Panel.SetActive(false);
    }
}
