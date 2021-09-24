using UnityEngine;

public enum State
{
    Standart,
    Selected,
    Variant
}

public class Cell : MonoBehaviour
{
    public Color StandartColor;
    public Color HoverColor;
    public Color HoverMoveColor;
    public Color HoverAttackColor;

    public Color SelectedColor;
    public Color MoveColor;
    public Color AttackColor;

    [SerializeField] private MeshRenderer _meshRenderer;

    public UnitBase Unit;

    [HideInInspector] public Vector2 Position;
    [HideInInspector] public State State;

    public InteractorsBase InteractorsBase;

    public void ChangeColor(Color color) => _meshRenderer.material.color = color;

    private void OnMouseEnter()
    {
        if(State == State.Standart)
        {
            ChangeColor(HoverColor);
        }
        else if(State == State.Variant)
        {
            if(Unit == null)
            {
                ChangeColor(HoverMoveColor);
            }
            else
            {
                ChangeColor(HoverAttackColor);
            }
        }
    }

    private void OnMouseDown()
    {
        if (State == State.Selected)
        {
            InteractorsBase.GridInteractor.DeselectCell(this);
        }
        else if (State == State.Variant)
        {
            InteractorsBase.GridInteractor.GetSelectedCell().Unit.GoToCell(this);
        }
        else if(State == State.Standart)
        {
            if(InteractorsBase.GridInteractor.GetSelectedCell() != null)
            {
                InteractorsBase.GridInteractor.DeselectCell(InteractorsBase.GridInteractor.GetSelectedCell());
            }

            InteractorsBase.GridInteractor.SelectCell(this);
        }
    }

    private void OnMouseExit()
    {
        if(State == State.Standart)
        {
            ChangeColor(StandartColor);
        }

        else if(State == State.Variant)
        {
            if(Unit == null)
            {
                ChangeColor(MoveColor);
            }
            else
            {
                ChangeColor(AttackColor);
            }
        }
    }
}
