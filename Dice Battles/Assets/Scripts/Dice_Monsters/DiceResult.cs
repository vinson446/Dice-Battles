using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceResult : MonoBehaviour
{
    Vector3 diceVelocity;
    public int diceNum;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        diceVelocity = GetComponentInParent<Rigidbody>().velocity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerBaseDesk" && diceVelocity.x == 0 && diceVelocity.y == 0 && diceVelocity.z == 0)
        {
            StartCoroutine(WaitForResults());
        }
    }

    IEnumerator WaitForResults()
    {
        DiceResultMain diceMain = GetComponentInParent<DiceResultMain>();

        yield return new WaitForSeconds(0.5f);

        diceNum = System.Convert.ToInt32(gameObject.name);

        if (gameObject.transform.parent.tag == "SummonDice")
        {
            diceMain.sumNum = diceNum;
        }
        else if (gameObject.transform.parent.tag == "AttackDice")
        {
            diceMain.atkNum = diceNum;
        }
        else if (gameObject.transform.parent.tag == "DefenseDice")
        {
            diceMain.defNum = diceNum;
        }
    }
}
