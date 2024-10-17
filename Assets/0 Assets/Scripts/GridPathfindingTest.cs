using Sleep0.Logic;
using Sleep0.Logic.Grid;
using Sleep0.Logic.Pathfinding;
using Sleep0.Tools;
using Sleep0.Visual;
using System.Collections.Generic;
using UnityEngine;

namespace Sleep0
{
	// Pathfinding test class
    public class GridPathfindingTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gridNodeUIPrefab;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private GridTile[] _walkableGridTiles;

        [SerializeField]
        private GridTile[] _bloclingGridTiles;

        [SerializeField]
        private GridTile _playerGridTile;

        [SerializeField]
        private GridTile _goalGridTile;

        private Grid2D<GridTile> _grid;
        private AStarPathfinding<GridTile> _aStar;
        private GridNode2D<GridTile> _startNode;
        private GridNode2D<GridTile> _targetNode;
        private Stack<GridNode2D<GridTile>> _foundPath;

        void Start()
        {
            _grid = new Grid2D<GridTile>(10, 10, 1f, transform, _gridNodeUIPrefab);
            _grid.GenerateRandomGrid(_walkableGridTiles, _bloclingGridTiles, 20);
            _aStar = new AStarPathfinding<GridTile>(_grid);

            _player.SetGrid2D(_grid);

            // Since the player position is at the [0,0] node, select this as the start node.
            _startNode = _grid.GetNode(0, 0);
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Looking for a star path...");
                _targetNode = _grid.GetNode(VectorTools.GetMouseWorldPosition(Camera.main, transform.position.z));
                _startNode = _grid.GetNode(_player.transform.position);
                if (_targetNode != null && _targetNode.IsWalkable)
                {
                    _foundPath = _aStar.CalculatePath(_startNode, _targetNode);

                    // Color nodes for debug purposes.
                    _grid.ColorPath(_foundPath, Color.cyan);
                    _grid.ColorNode(_startNode, Color.green);
                    _grid.ColorNode(_targetNode, Color.yellow);

                    _foundPath.Push(_startNode);
                    _player.SetPath(_foundPath);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _grid.ToggleDebugUI();
            }
        }
    }
}