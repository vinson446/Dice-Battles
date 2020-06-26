using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None = 0,

    Player1StartTurn, // 1
    Player1ChooseDie, // 2
    Player1RollDie, // 3
    Player1Turn, // 4
    Player1SumMon1, // 5
    Player1SumMon2, // 6
    Player1MoveMon, // 7
    Player1Attack, // 8
    Player1Defend, // 9
    Player1EndTurn, // 10

    Player2StartTurn, // 11
    Player2ChooseDie, // 12
    Player2RollDie, // 13
    Player2Turn, // 14
    Player2SumMon1, // 15
    Player2SumMon2, // 16
    Player2MoveMon, // 17
    Player2Attack, // 18
    Player2Defend, // 19
    Player2EndTurn, // 20

    ShowMonDeck, // 21
    Options, // 22
    Confirm // 23
}
