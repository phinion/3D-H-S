using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    [SerializeField] List<AttackCombo> Moves; // All available moves
    [SerializeField]  List<AttackCombo> availableMoves;

    PlayerManager player;

    int comboCount = 0;

    AttackCombo nextMove = null;

    bool inCombo = false;

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

    public void ResetAvailableMoves()
    {
        availableMoves.Clear();
        availableMoves.AddRange(Moves);
    }

    public void ClearAvailableMoves()
    {
        availableMoves.Clear();
        comboCount = 0;
        player.anim.SetInteger("comboCount", comboCount);
    }

    public bool CheckAvailableMoves(AttackType _type)
    {
        ComboInput currentComboInput = new ComboInput(_type);

        //List<AttackCombo> listToUse;
        //if(availableMoves.Count == 0)
        //{
        //    listToUse = Moves;
        //}
        //else
        //{
        //    listToUse = availableMoves;
        //}

        //foreach (AttackCombo _c in listToUse)
        //{
        //    if (_c.ContinueCombo(currentComboInput, comboCount) && !availableMoves.Contains(_c))
        //    {
        //        availableMoves.Add(_c);
        //        newMove = true;
        //    }

        //}

        //if (availableMoves.Count > 0)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}


        if (availableMoves.Count != 0)
        {
            List<AttackCombo> remove = new List<AttackCombo>();

            //if (availableMoves.Count == 0)
            //{
            //    ResetAvailableMoves();
            //}


            foreach (AttackCombo _c in availableMoves)
            {
                if (_c.ContinueCombo(currentComboInput, comboCount))
                {

                }
                else
                {
                    remove.Add(_c);
                }
            }

            foreach (AttackCombo _c in remove) availableMoves.Remove(_c);

        }

        if (availableMoves.Count > 0)
        {
            inCombo = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsMoveAvailable()
    {
        if(availableMoves.Count > 0 && inCombo)
        {
            return true;
        }
        return false;
    }

    public void DoNextMove()
    {
        player.anim.SetInteger("comboCount", comboCount);
        comboCount++;
        inCombo = false;
    }
}
