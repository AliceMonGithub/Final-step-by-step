using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Player,
    Enemy
}

public class UnitBase : MonoBehaviour
{
    public List<Vector2> MoveVariants;
    public List<Vector2> AttackVariants;

    public List<Cell> GreenCells;

    public Cell CurrentCell;

    public Side UnitSide;

    public int Health = 10;
    public int Damage = 5;

    [SerializeField] private InteractorsBase _interactorsBase;

    [HideInInspector] public List<Cell> Variants = new List<Cell>();

    public void GoToCell(Cell cell)
    {
        if (cell.Unit == null)
        {
            Move(cell);
        }
        else
        {
            if (DamageUnit(cell.Unit))
            {
                Move(cell);
            }
        }

        _interactorsBase.GridInteractor.DeselectCell(cell);

        if(GreenCells.Count != 0)
        {
            _interactorsBase.GridInteractor.ClearVariants(GreenCells);
        }
        else
        {
            _interactorsBase.GridInteractor.ClearVariants(Variants);
        }

        _interactorsBase.TurnInteractor.DoTurn(this);
        _interactorsBase.TurnInteractor.TryChange();
    }

    private void Move(Cell cell)
    {
        _interactorsBase.GridInteractor.GetSelectedCell().Unit = null;

        transform.position = new Vector3(cell.transform.position.x, transform.position.y, cell.transform.position.z);

        cell.Unit = this;
        CurrentCell = cell;
    }

    private bool DamageUnit(UnitBase enemy)
    {
        enemy.Health -= Damage;

        if (enemy.Health <= 0)
        {
            _interactorsBase.TurnInteractor.DeleteUnit(enemy);

            enemy.Die();

            return true;
        }

        return false;
    }

    private void Die()
    {
        CurrentCell.Unit = null;

        Destroy(gameObject);
    }

    [ContextMenu("Initialize")]
    public void InitializeCell()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider.GetComponent<Cell>())
            {
                transform.position = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);

                hit.collider.GetComponent<Cell>().Unit = this;

                var cell = hit.collider.GetComponent<Cell>();

                _interactorsBase = cell.InteractorsBase;
                CurrentCell = cell;
                _interactorsBase = cell.InteractorsBase;
            }
        }
    }
}