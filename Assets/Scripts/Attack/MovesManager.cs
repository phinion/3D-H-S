using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    [SerializeField] private List<AttackCombo> moves; // All available moves
    [SerializeField] private List<AttackCombo> availableMoves; // Currently available moves during a combo

    private PlayerManager player;
    private int comboCount = 0;
    private bool inCombo = false;

    void Start()
    {
        availableMoves = new List<AttackCombo>();
        player = GetComponent<PlayerManager>();
        ResetAvailableMoves();
    }

    public void ResetAvailableMoves()
    {
        availableMoves.Clear();
        availableMoves.AddRange(moves);
    }

    public void ClearAvailableMoves()
    {
        availableMoves.Clear();
        comboCount = 0;
        player.anim.SetInteger("comboCount", comboCount);
    }

    public bool CheckAvailableMoves(AttackType attackType, Vector2 movementDirection, bool dash, bool dodge)
    {
        List<InputType> currentInputs = new List<InputType>
        {
            new AttackInput { requiredAttackType = attackType },
            //new MovementInput { requiredMovement = movementDirection },
            new BoolInputType { requiredValue = dash },
            new BoolInputType { requiredValue = dodge }
        };

        availableMoves = availableMoves
            .Where(move => move.CurrentComboInput(comboCount)?.ExistsIn(currentInputs) == true)
            .ToList();

        if (availableMoves.Count > 0)
        {
            inCombo = true;
            Debug.Log("Available moves filtered, continuing combo.");
            return true;
        }

        //ClearAvailableMoves();
        return false;

    }

    public bool IsMoveAvailable()
    {
        bool testboo = availableMoves.Count > 0 && inCombo;
        Debug.Log($"IsMoveAvailable {testboo}");
        return testboo;
    }

    public void DoNextMove()
    {
        player.anim.SetInteger("comboCount", comboCount);
        comboCount++;

        string anim = availableMoves[0].combo[comboCount].Anim;
        if(anim != "")
        {
            player.anim.Play(anim);
        }

        
        inCombo = false;
    }
}
