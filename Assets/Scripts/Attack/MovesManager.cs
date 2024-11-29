using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Attack;
using Moves;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovesManager : MonoBehaviour
{
    [SerializeField] private List<AttackCombo> moves; // All available moves
    [SerializeField] private List<AttackCombo> availableMoves; // Currently available moves during a combo

    private InputManager input;
    private PlayerManager player;
    private int comboCount = -1;
    private bool inCombo = false;

    void Start()
    {
        availableMoves = new List<AttackCombo>();
        player = GetComponent<PlayerManager>();
        input = GetComponent<InputManager>();
        input.OnAttack += HandleAttackInput;
        ResetAvailableMoves();
    }
    
    private void OnDestroy()
    {
        input.OnAttack -= HandleAttackInput;
    }

    public void ResetAvailableMoves()
    {
        ClearAvailableMoves();
        availableMoves.AddRange(moves);
    }

    public void ClearAvailableMoves()
    {
        availableMoves.Clear();
        comboCount = -1;
        inCombo = false;
        player.anim.SetInteger("comboCount", comboCount);
    }

    private void FilterAvailableMoves(AttackSO _attack)
    {
        var modifiedList = availableMoves.ToList();
        
        // TODO LINQ this
        for (var i = modifiedList.Count - 1; i >= 0; i--)
        {
            var attackCombo = modifiedList[i];
            if (attackCombo.combo[comboCount] != _attack)
            {
                modifiedList.Remove(attackCombo);
            }
        }

        availableMoves = modifiedList;
    }

    private void HandleAttackInput()
    {
        if (CheckAvailableMoves())
        {
            //DoNextMove(); we let the states decide
        }
        else
        {
            ClearAvailableMoves();
        }
    }

    private bool CheckAvailableMoves()
    {
        var currentInputs = GetInputs();

        availableMoves = availableMoves
            .Where(move => move.CurrentComboInput(comboCount + 1)?.ExistsIn(currentInputs) == true)
            .ToList();

        if (availableMoves.Count > 0)
        {
            inCombo = true;
            return true;
        }

        return false;
    }

    public List<BaseInputType> GetInputs()
    {
        List<BaseInputType> currentInputs = new List<BaseInputType>();

        if (input.MovementInput.y < 0)
        {
            currentInputs.Add(new InputType { RequiredValue = "south" });
        }
        
        if (input.attack)
        {
            currentInputs.Add(new AttackInput { requiredAttackType = AttackType.light });
        }
        
        if (input.heavyAttack)
        {
            currentInputs.Add(new AttackInput { requiredAttackType = AttackType.heavy });
        }

        if (input.run)
        {
            currentInputs.Add(new InputType { RequiredValue = "run" });
        }
        
        if (input.dodge)
        {
            currentInputs.Add(new InputType { RequiredValue = "dodge" });
        }
        
        return currentInputs;
    }

    public bool IsMoveAvailable()
    {
        bool testboo = availableMoves.Count > 0 && inCombo;
        return testboo;
    }

    public void DoNextMove()
    {
        if (availableMoves.Count > 0 && comboCount + 1 < availableMoves[0].combo.Count)
        {
            comboCount++;

            var attack = availableMoves[0].combo[comboCount];
            string anim = attack.Anim;
            if (!string.IsNullOrEmpty(anim))
            {
                player.anim.CrossFade(anim, 0.1f, 0, 0f);
            }
            player.anim.SetInteger("comboCount", comboCount);
            
            FilterAvailableMoves(attack);
            
            inCombo = false;
            
        }
    }
    
    public bool CurrentAttackMaintainsMomentum()
    {
        if (availableMoves.Count > 0)
        {
            bool currentAttackMaintainsMomentum = availableMoves[0].combo[comboCount].MaintainMomentum;
            return currentAttackMaintainsMomentum;
        }
        return false;
    }

    public AttackSO CurrentAttack()
    {
        if (availableMoves.Count > 0)
        {
            return availableMoves[0].combo[comboCount];
        }
        return null;
    }
    
    #region comboReset Timer
    private float comboResetTime = 1.0f; // Customize as needed
    private Coroutine comboResetCoroutine;

    private void StartComboResetTimer()
    {
        if (comboResetCoroutine != null) StopCoroutine(comboResetCoroutine);
        comboResetCoroutine = StartCoroutine(ResetComboAfterDelay(comboResetTime));
    }

    private IEnumerator ResetComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearAvailableMoves();
    }
    #endregion
}
