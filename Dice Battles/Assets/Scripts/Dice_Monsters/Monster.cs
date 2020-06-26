using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/Monster", order = 1)]
public class Monster : ScriptableObject
{
    public GameObject monster;
    public int summonCost;
    public int movement;
    public int hp;
    public int attack;
    public int defense;
}
