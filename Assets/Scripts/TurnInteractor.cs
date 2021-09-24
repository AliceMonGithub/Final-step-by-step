using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class TurnInteractor : Interactor
{
    [SerializeField] private int UnitsCount;

    private void Awake()
    {
        if(_gridRepository.CurrentTurn == Side.Player)
        {
            UnitsCount = CheckUnits(_gridRepository.PlayerUnits);
        }
        else
        {
            UnitsCount = CheckUnits(_gridRepository.EnemyUnits);
        }
    }

    public void TryChange()
    {
        if(UnitsCount == 0)
        {
            _gridRepository.CurrentUnits.Clear();

            if(_gridRepository.CurrentTurn == Side.Player)
            {
                UnitsCount = CheckUnits(_gridRepository.EnemyUnits);

                _gridRepository.CurrentTurn = Side.Enemy;
            }
            else
            {
                UnitsCount = CheckUnits(_gridRepository.PlayerUnits);

                _gridRepository.CurrentTurn = Side.Player;
            }
        }
    }

    public void DeleteUnit(UnitBase unit)
    {
        _gridRepository.CurrentUnits.Remove(unit);

        if(unit.UnitSide == Side.Player)
        {
            _gridRepository.PlayerUnits.Remove(unit);

            if(_gridRepository.PlayerUnits.Count == 0)
            {
                RestartScene();
            }

        }
        else
        {
            _gridRepository.EnemyUnits.Remove(unit);

            if(_gridRepository.EnemyUnits.Count == 0)
            {
                RestartScene();
            }
        }
    }

    public void DoTurn(UnitBase unit)
    {
        _gridRepository.CurrentUnits.Add(unit);

        UnitsCount--;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int CheckUnits(List<UnitBase> units)
    {
        var count = 0;

        foreach(UnitBase unit in units)
        {
            if(_interactorsBase.GridInteractor.CalculateVariants(unit.CurrentCell, unit.MoveVariants, unit.AttackVariants).Count == 0)
            {
                _gridRepository.CurrentUnits.Add(unit);
            }
            else
            {
                count++;
            }
        }

        return count;
    }
}
