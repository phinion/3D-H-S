using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovesManager : MonoBehaviour
{
    [SerializeField] private List<AttackCombo> moves; // All available moves
    [SerializeField] private List<AttackCombo> availableMoves; // Currently available moves during a combo

    private InputManager input;
    private PlayerManager player;
    private int comboCount = 0;
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
        comboCount = 0;
        inCombo = false;
        player.anim.SetInteger("comboCount", comboCount);
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
            .Where(move => move.CurrentComboInput(comboCount)?.ExistsIn(currentInputs) == true)
            .ToList();

        if (availableMoves.Count > 0)
        {
            inCombo = true;
            return true;
        }

        return false;
    }

    public List<InputType> GetInputs()
    {
        List<InputType> currentInputs = new List<InputType>
        {
            new AttackInput { requiredAttackType = AttackType.light }
        };

        if (input.run)
        {
            currentInputs.Add(new NameInputType { requiredValue = "run" });
        }
        
        if (input.dodge)
        {
            currentInputs.Add(new NameInputType { requiredValue = "dodge" });
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
        if (availableMoves.Count > 0 && comboCount < availableMoves[0].combo.Count)
        {
            string anim = availableMoves[0].combo[comboCount].Anim;
            if (!string.IsNullOrEmpty(anim))
            {
                player.anim.Play(anim);
            }
            player.anim.SetInteger("comboCount", comboCount);
            comboCount++;
            inCombo = false;
        }
    }
    
    public bool CurrentAttackMaintainsMomentum()
    {
        if (availableMoves.Count > 0)
        {
            bool currentAttackMaintainsMomentum = availableMoves[0].combo[comboCount - 1].MaintainMomentum;
            Debug.Log($"Current Attack Maintains momentum {currentAttackMaintainsMomentum} Combocount: {comboCount} + Current anim {availableMoves[0].combo[comboCount - 1].Anim}");
            return currentAttackMaintainsMomentum;
        }
        return false;
    }

    public Attack CurrentAttack()
    {
        if (availableMoves.Count > 0)
        {
            return availableMoves[0].combo[comboCount - 1];
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
