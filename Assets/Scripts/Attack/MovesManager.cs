using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    [SerializeField] List<AttackCombo> Moves; // All available moves
    List<AttackCombo> availableMoves;

    PlayerManager player;

    int comboCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        availableMoves = new List<AttackCombo>();

        player = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckAvailableMoves(AttackType _type)
    {
        ComboInput currentComboInput = new ComboInput(_type);

        List<AttackCombo> remove = new List<AttackCombo>();

        if (availableMoves.Count == 0)
        {
            availableMoves.AddRange(Moves);
        }


        foreach (AttackCombo _c in availableMoves)
        {
            if (_c.ContinueCombo(currentComboInput))
            {

            }
            else
            {
                remove.Add(_c);
            }
        }

        foreach (AttackCombo _c in remove) availableMoves.Remove(_c);

        if (availableMoves.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DoMove()
    {
        player.attackState.ActivationCheck();
    }
}
