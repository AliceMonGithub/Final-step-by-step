using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Cell[] _prefabs;
    [SerializeField] private Cell _centerCell;
    [SerializeField] private float _offset;
    [SerializeField] private Transform _parent;
    [SerializeField] private InteractorsBase InteractorsBase;

    [ContextMenu("Generate grid")]
    private void GenerateGrid()
    {
        InteractorsBase.GridInteractor.ClearCellsInRepository();


        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                if (x == _gridSize.x / 2 && y == _gridSize.y / 2)
                {
                    InstantiateCell(_centerCell, new Vector2(x, y));

                    continue;
                }

                var randomprefab = _prefabs[Random.Range(0, _prefabs.Length)];

                InstantiateCell(randomprefab, new Vector2(x, y));
            }
        }
    }

    private void InstantiateCell(Cell cellprefab, Vector2 prefabposition)
    {
        if (cellprefab != null)
        {
            cellprefab.InteractorsBase = InteractorsBase;
            var cellsize = cellprefab.GetComponent<MeshRenderer>().bounds.size;


            var position = new Vector3(prefabposition.x * (cellsize.x + _offset), 0, prefabposition.y * (cellsize.z + _offset));

            var cell = Instantiate(cellprefab, position, Quaternion.identity, _parent);

            cell.name = $"X: {prefabposition.x} Y: {prefabposition.y}";
            cell.Position = new Vector2(prefabposition.x, prefabposition.y);

            InteractorsBase.GridInteractor.AddCellToRepository(cell);
        }
    }
}