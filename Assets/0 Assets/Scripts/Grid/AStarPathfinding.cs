using Sleep0.Logic.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Sleep0.Logic.Pathfinding
{
    public class AStarPathfinding<T> where T : MonoBehaviour, ITileable
    {
        private Grid2D<T> _grid2D;
        private List<GridNode2D<T>> _openList;
        private List<GridNode2D<T>> _closedList;
        private Stack<GridNode2D<T>> _foundPath;
        private GridNode2D<T> _current;
        private GridNode2D<T> _tmpNode;

        private System.Diagnostics.Stopwatch _stopwatch;

        public AStarPathfinding(Grid2D<T> grid2D)
        {
            this._grid2D = grid2D;
            this._openList = new List<GridNode2D<T>>();
            this._closedList = new List<GridNode2D<T>>();
            this._foundPath = new Stack<GridNode2D<T>>();

            _stopwatch = new System.Diagnostics.Stopwatch();
        }

        /// <summary>
        /// Simple A* pathfinding from a start node to a target node.        
        /// </summary>
        /// <param name="startNode">Start node for the path finding.</param>
        /// <param name="targetNode">Target node to reach.</param>
        /// <returns>Stack containing all nodes in the found path. If no path was found, returns an empty list.</returns>
        public Stack<GridNode2D<T>> CalculatePath(GridNode2D<T> startNode, GridNode2D<T> targetNode)
        {
            return FindPathToNode(startNode, targetNode);
        }

        // Based on: https://www.geeksforgeeks.org/a-search-algorithm/ and https://brilliant.org/wiki/a-star-search/
        private Stack<GridNode2D<T>> FindPathToNode(GridNode2D<T> fromNode, GridNode2D<T> toNode)
        {
            _grid2D.ResetCosts();
            _openList.Clear();
            _closedList.Clear();
            _foundPath.Clear();

            _stopwatch.Reset();
            _stopwatch.Start();

            if (fromNode == toNode)
            {
                _foundPath.Push(toNode);
                return _foundPath;
            }

            _current = fromNode;
            _openList.Add(_current);

            bool isTargetReached = false;

            int nodeChecked = 0;    // For debug purposes

            while (_openList.Count > 0 && !isTargetReached)
            {
                nodeChecked++;

                _current = _openList[0];

                foreach (GridNode2D<T> node in _openList)
                {
                    if (node.FCost < _current.FCost || (node.FCost == _current.FCost && node.HCost < _current.HCost))
                        _current = node;
                }

                _openList.Remove(_current);
                _closedList.Add(_current);

                foreach (GridNode2D<T> neighbour in _current.Neighbours)
                {
                    if (!neighbour.IsWalkable || _closedList.Contains(neighbour)) continue;

                    if (neighbour == toNode)
                    {
                        _closedList.Add(neighbour);
                        neighbour.ParentNode = _current;
                        // Found the target.
                        isTargetReached = true;
                        break;
                    }

                    if (_openList.Contains(neighbour)) continue;

                    nodeChecked++;

                    neighbour.ParentNode = _current;

                    neighbour.GCost = _current.GCost + neighbour.Weight;
                    neighbour.CalculatHeuristicDistanceToNode(toNode);

                    _grid2D.DebugNodeValues(neighbour);

                    _openList.Add(neighbour);
                }
            }

            // If path was not closed/target node could not be found or reached; return empty path.
            if (!_closedList.Contains(toNode))
            {
                Debug.Log(string.Format("No path found to target, checked {0} nodes.", nodeChecked));
                return _foundPath;
            }

            _tmpNode = _closedList[_closedList.Count - 1];
            do
            {
                _foundPath.Push(_tmpNode);

                if (_tmpNode.ParentNode == null)
                    Debug.LogErrorFormat("Node [{0}|{1}] has no parent!", _tmpNode.X, _tmpNode.Y);

                _tmpNode = _tmpNode.ParentNode;
            } while (_tmpNode != fromNode);

            _stopwatch.Stop();

            Debug.Log(string.Format("A Star path found with length of {0} in {1}ms", _foundPath.Count, _stopwatch.Elapsed.TotalMilliseconds));
            return _foundPath;
        }
    }
}