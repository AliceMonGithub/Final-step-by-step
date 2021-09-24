using System.Collections.Generic;
using UnityEngine;

public class GridInteractor : Interactor
{
    public void SelectCell(Cell cell)
    {
        _gridRepository.SelectedCell = cell;
        cell.State = State.Selected;
        cell.ChangeColor(cell.SelectedColor);

        if (cell.Unit != null) 
        {
            if (_gridRepository.CurrentUnits.Find(unit => unit == cell.Unit) == null && cell.Unit.UnitSide == _gridRepository.CurrentTurn)
            {
                if (cell.Unit != null && cell.Unit.GreenCells.Count != 0)
                {
                    ShowVariants(cell.Unit.GreenCells);
                }
                else if (cell.Unit != null)
                {
                    cell.Unit.Variants = CalculateVariants(cell, cell.Unit.MoveVariants, cell.Unit.AttackVariants);

                    ShowVariants(cell.Unit.Variants);
                }
            }
        }

    }

    public void DeselectCell(Cell cell)
    {
        _gridRepository.SelectedCell.State = State.Standart;
        _gridRepository.SelectedCell.ChangeColor(_gridRepository.SelectedCell.StandartColor);
        _gridRepository.SelectedCell = null;

        if (cell.Unit != null && cell.Unit.GreenCells.Count != 0)
        {
            ClearVariants(cell.Unit.GreenCells);
        }
        else if(cell.Unit != null)
        {
            ClearVariants(cell.Unit.Variants);
            cell.Unit.Variants.Clear();
        }
    }

    public List<Cell> CalculateVariants(Cell unitcell, List<Vector2> movevariants, List<Vector2> attackvariants)
    {
        var variants = new List<Cell>();
        Cell cell;

        foreach (Vector2 offset in movevariants)
        {
            cell = FindCellByPosition(offset, unitcell, true);

            if (cell != null)
            {
                variants.Add(cell);
            }
        }

        foreach (Vector2 offset in attackvariants)
        {
            cell = FindCellByPosition(offset, unitcell, false);

            if (cell != null)
            {
                variants.Add(cell);
            }
        }

        return variants;
    }

    public Cell FindCellByPosition(Vector2 offset, Cell UnitCell, bool MoveVariant)
    {
        return MoveVariant 
            ? _gridRepository.Cells.Find(cell => cell.Position == UnitCell.Position + offset && cell.Unit == null) 
            : _gridRepository.Cells.Find(cell => cell.Position == UnitCell.Position + offset && cell.Unit != null && cell.Unit.UnitSide != UnitCell.Unit.UnitSide);
    }

    public void ShowVariants(List<Cell> variants)
    {
        foreach (Cell variant in variants)
        {

            if(variant.Unit == null && _gridRepository.SelectedCell.Unit != variant.Unit)
            {
                variant.ChangeColor(variant.MoveColor);

                variant.State = State.Variant;
            }
            else if(_gridRepository.SelectedCell.Unit != variant.Unit)
            {
                variant.ChangeColor(variant.AttackColor);

                variant.State = State.Variant;
            }
        }
    }

    public void ClearVariants(List<Cell> variants)
    {
        foreach (Cell variant in variants)
        {
            variant.State = State.Standart;
            variant.ChangeColor(variant.StandartColor);
        }
    }

    public void AddCellToRepository(Cell cell) => _gridRepository.Cells.Add(cell);
    public void ClearCellsInRepository() => _gridRepository.Cells.Clear();

    public Cell GetSelectedCell() => _gridRepository.SelectedCell;
}