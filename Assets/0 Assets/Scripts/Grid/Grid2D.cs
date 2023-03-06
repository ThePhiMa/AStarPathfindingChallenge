using Sleep0.Logic.VisualDebug;
using System.Collections.Generic;
using UnityEngine;

namespace Sleep0.Logic.Grid
{
    public class Grid2D<T> where T : MonoBehaviour, ITileable
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private GridNode2D<T>[,] _gridArray;

        private Transform _parent;

        [SerializeField]
        private GameObject _gridNodeUIPrefab;
        private GridNodeUI[,] _debugTextMeshes;
        private float _debugTextOffset;
        private bool _showDebugUI;

        private List<GridNode2D<T>> tempNeighbours;

        public Grid2D(int width, int height, float cellSize, Transform parent, GameObject gridNodeUIPrefab)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._parent = parent;
            this._gridNodeUIPrefab = gridNodeUIPrefab;
            this.tempNeighbours = new List<GridNode2D<T>>();
            this._debugTextOffset = _cellSize * 0.5f;
            this._showDebugUI = true;
        }

        public void GenerateRandomGrid(T[] walkableTiles, T[] blockingTiles, int numberOfBlockingTiles)
        {
            _gridArray = new GridNode2D<T>[_width, _height];
            _debugTextMeshes = new GridNodeUI[_width, _height];

            int rand = 0;

            // Calculate some random blocking tiles.
            List<int> blockingTileIndices = new List<int>();
            for (int i = 0; i < numberOfBlockingTiles; i++)
                blockingTileIndices.Add(Random.Range(0, _width) + Random.Range(0, _height) * _height);

            // Generate the grid.
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _debugTextMeshes[x, y] = GameObject.Instantiate(_gridNodeUIPrefab, _parent).GetComponent<GridNodeUI>(); //VisualTools.CreateWorldText(_parent, "0", GetWorldPosition(x, y), 20);
                    _debugTextMeshes[x, y].transform.position = new Vector3(x, y, 0);

                    // Generate some random blocking tiles (except at the starting 0,0 location) (Not very performant, but since it's only done at startup, it should be fine).
                    if (blockingTileIndices.Contains(x + y * _height) && (x > 0 && y > 0))
                    {
                        rand = Random.Range(0, blockingTiles.Length - 1);
                        _gridArray[x, y] = new GridNode2D<T>(x, y, 1, blockingTiles[rand], _parent);
                    }
                    else // Generate random walkable tiles.
                    {
                        rand = Random.Range(0, walkableTiles.Length - 1);
                        _gridArray[x, y] = new GridNode2D<T>(x, y, 1, walkableTiles[rand], _parent);
                    }

                    SetDefaultColor(_gridArray[x, y]);
                }
            }

            // Calculate all neighbors.
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _gridArray[x, y].Neighbours = CalculatetNeighbours(_gridArray[x, y]);
                }
            }
        }

        public bool IsNodeWalkable(int x, int y)
        {
            if ((x >= 0 && y >= 0)
                 && (x < _width && y < _height))
                return _gridArray[x, y].IsWalkable;

            return false;
        }

        public void ColorPath(Stack<GridNode2D<T>> path, Color color)
        {
            foreach (GridNode2D<T> node in path)
                _debugTextMeshes[node.X, node.Y].SetColor(color);
        }

        public void SetDefaultColor(GridNode2D<T> node)
        {
            _debugTextMeshes[node.X, node.Y].SetColor(node.IsWalkable ? Color.white : Color.red);
        }

        public void ColorNode(GridNode2D<T> node, Color color)
        {
            _debugTextMeshes[node.X, node.Y].SetColor(color);
        }

        public GridNode2D<T> GetNode(Vector3 worldPosition)
        {
            int x, y;
            GetGridXYPosition(worldPosition, out x, out y);
            return GetNode(x, y);
        }

        public GridNode2D<T> GetNode(int x, int y)
        {
            if ((x >= 0 && y >= 0)
                 && (x < _width && y < _height))
                return _gridArray[x, y];

            return null;
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize;
        }

        private Vector3 GetWorldPosition(float x, float y)
        {
            return new Vector3(x, y) * _cellSize;
        }

        private void GetGridXYPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = (int)(Mathf.RoundToInt(worldPosition.x) / _cellSize);
            y = (int)(Mathf.RoundToInt(worldPosition.y) / _cellSize);
        }

        public void DrawDebugGrid()
        {
            for (float x = 0 + _parent.position.x - _debugTextOffset; x < _width + _parent.position.x - _debugTextOffset; x++)
            {
                for (float y = 0 + _parent.position.y - _debugTextOffset; y < _height + _parent.position.y - _debugTextOffset; y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white);
                }
            }
            Debug.DrawLine(GetWorldPosition(_parent.position.x - _debugTextOffset, _height + _parent.position.y - _debugTextOffset), GetWorldPosition(_width + _parent.position.x - _debugTextOffset, _height + _parent.position.y - _debugTextOffset));
            Debug.DrawLine(GetWorldPosition(_width + _parent.position.x - _debugTextOffset, _parent.position.y - _debugTextOffset), GetWorldPosition(_width + _parent.position.x - _debugTextOffset, _height + _parent.position.y - _debugTextOffset));
        }

        public void DebugNodeValues(GridNode2D<T> node)
        {
            _debugTextMeshes[node.X, node.Y].SetValues(node.GCost, node.HCost, node.FCost, node.Weight);
        }

        public void SetNodeValue(int x, int y, T value)
        {
            if ((x >= 0 && y >= 0)
                && (x < _width && y < _height))
            {
                _gridArray[x, y].Value = value;
            }
        }

        public void ResetCosts()
        {
            foreach (GridNode2D<T> node in _gridArray)
            {
                node.ResetCosts();
                DebugNodeValues(node);
                if (node.IsWalkable)
                    _debugTextMeshes[node.X, node.Y].SetColor(Color.white);
            }
        }

        public void SetNodeValue(Vector3 worldPosition, int value)
        {
            int x, y;
            GetGridXYPosition(worldPosition, out x, out y);
            SetNodeValue(x, y, (T)(object)value);
        }

        public GridNode2D<T>[] CalculatetNeighbours(GridNode2D<T> node)
        {
            tempNeighbours.Clear();

            if (node.Y + 1 < _height)
                tempNeighbours.Add(_gridArray[node.X, node.Y + 1]);
            if (node.Y - 1 >= 0)
                tempNeighbours.Add(_gridArray[node.X, node.Y - 1]);
            if (node.X + 1 < _width)
                tempNeighbours.Add(_gridArray[node.X + 1, node.Y]);
            if (node.X - 1 >= 0)
                tempNeighbours.Add(_gridArray[node.X - 1, node.Y]);

            return tempNeighbours.ToArray();
        }

        public void ToggleDebugUI()
        {
            _showDebugUI = !_showDebugUI;

            foreach (GridNodeUI nodeUI in _debugTextMeshes)
                nodeUI.gameObject.SetActive(_showDebugUI);
        }
    }
}