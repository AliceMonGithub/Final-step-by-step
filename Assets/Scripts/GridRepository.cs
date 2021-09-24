using System.Collections.Generic;
using UnityEngine;

public class GridRepository : MonoBehaviour
{
    public List<Cell> Cells = new List<Cell>();
    public Cell SelectedCell;

    public List<UnitBase> PlayerUnits;
    public List<UnitBase> EnemyUnits;

    public List<UnitBase> CurrentUnits;

    public Side CurrentTurn;
}
