using Sleep0.Logic.Grid;
using Sleep0.Visual;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sleep0.Logic
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private GridTile _gridTile;

        [SerializeField]
        private float _moveSpeed = 1f;

        [SerializeField]
        private float _minDistanceToTarget = 0.05f;

        private Grid2D<GridTile> _grid;
        private Stack<GridNode2D<GridTile>> _waypoints;
        private Stack<GridNode2D<GridTile>> _tmpWaypoints;
        private GridNode2D<GridTile> _currentWaypoint;
        private GridNode2D<GridTile> _futureWaypoint;   // The waypoint AFTER the next waypoint, for path smoothing. Why isn't there a word for this in the english language? In German we just say "über"-next.
        private Vector3 _targetPosition;
        private float _currentDistanceToTarget;

        private bool _isDiagonalPathWalkable = false;

        void Start()
        {
            _targetPosition = transform.position;

            _gridTile.SetRandomTileSprite();
        }

        void Update()
        {
            if (_waypoints == null) return;

            _currentDistanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            if (_currentDistanceToTarget > _minDistanceToTarget)                                // Move towards the target waypoint
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _moveSpeed);
            else if (_currentDistanceToTarget <= _minDistanceToTarget && _waypoints.Count > 0)  // If close enough at target waypoint, look for new one
                SimplePathSmoothing();
        }

        public void SetGrid2D(Grid2D<GridTile> grid2D)
        {
            this._grid = grid2D;
        }

        public void SetPath(Stack<GridNode2D<GridTile>> path)
        {
            if (path.Count == 0) return;

            _waypoints = path;
            _currentWaypoint = path.Pop();
            SimplePathSmoothing();
        }

        private void SimplePathSmoothing()
        {            
            Debug.Log(string.Format("SimplePathSmoothing start at [{0}|{1}]", _currentWaypoint.X, _currentWaypoint.Y));

            // If we have more than one waypoint, try to "cut corners" in the path and check if the player can move diagonally
            if (_waypoints.Count > 1)
            {
                _isDiagonalPathWalkable = false;

                // Make a temporary copy of the waypoint stack, because I need to look at the second element.
                // I know this bad practice and generates garbage, plus this also kinda defeats the purpose of the stack, but errare humanum est and I'm aready over my time estimate, so... 
                _tmpWaypoints = new Stack<GridNode2D<GridTile>>(_waypoints.Reverse());
                _tmpWaypoints.Pop();                    // Now pop once...
                _futureWaypoint = _tmpWaypoints.Pop();  // ... and twice for the _correct_ waypoint.

                // First check if the nodes are diagonal to each other
                if (_futureWaypoint.IsDiagonalTo(_currentWaypoint))
                {
                    // Now check if the nodes in the to cardinal directions "towards" the diagonal node are walkable
                    if ((_futureWaypoint.X == _currentWaypoint.X + 1) || (_futureWaypoint.X == _currentWaypoint.X - 1))
                    {
                        // Check the eastern or western waypoint
                        _isDiagonalPathWalkable = _grid.IsNodeWalkable(_futureWaypoint.X, _currentWaypoint.Y);

                        Debug.Log(string.Format("Checking [{0}|{1}] (eastern or western) waypoint (Temp result, is walkable? {2})", _futureWaypoint.X, _currentWaypoint.Y, _isDiagonalPathWalkable));

                        // Now check the northern, or depending if the next waypoint is "south" the current wapoint, the southern waypoint
                        if (_isDiagonalPathWalkable
                            && ((_futureWaypoint.Y == _currentWaypoint.Y + 1)
                            || (_futureWaypoint.Y == _currentWaypoint.Y - 1)))
                        {
                            _isDiagonalPathWalkable = _grid.IsNodeWalkable(_currentWaypoint.X, _futureWaypoint.Y);
                            Debug.Log(string.Format("Checking [{0}|{1}] (northern or southern) waypoint (Temp result, is walkable? {2})", _currentWaypoint.X, _futureWaypoint.Y, _isDiagonalPathWalkable));
                        }
                    }
                }

                if (_isDiagonalPathWalkable)
                {
                    // Pop once to get the next waypoint in line (basically skipping one).
                    _waypoints.Pop();
                    Debug.Log("Path is diagonally walkable!");
                }                
            }

            _currentWaypoint = _waypoints.Pop();
            Debug.Log(string.Format("New target waypoint [{0}|{1}]!", _currentWaypoint.X, _currentWaypoint.Y));

            _targetPosition = _currentWaypoint.WorldPosition;
            Debug.Log("SimplePathSmoothing end.");
        }
    }
}