using UnityEngine;

namespace Sleep0.Logic.Grid
{
    public class GridNode2D<T> where T : MonoBehaviour, ITileable
    {
        public int X { private set; get; }
        public int Y { private set; get; }

        public Vector3 WorldPosition => new Vector3(X, Y, 0);

        public GridNode2D<T> ParentNode;
        public GridNode2D<T>[] Neighbours;

        public bool IsWalkable => Value.IsWalkable();

        public float Weight;        // Cost of node n.
        public float GCost;         // Cost of reaching node n.
        public float HCost;         // Estimated cost from n to goal. This is the heuristic part of the cost function, so it is like a guess.
        public float FCost          // Total estimated cost of path through node n. f(n) = g(n) + h(n)
        {
            get
            {
                if (GCost >= 0 && HCost >= 0)
                    return GCost + HCost;
                else
                    return -1;
            }

            private set { FCost = value; }
        }

        public T Value;

        public GridNode2D(int x, int y, int weight, T value, Transform parent)
        {
            this.X = x;
            this.Y = y;
            this.Weight = weight;
            this.GCost = 0;
            this.HCost = -1;
            this.ParentNode = null;
            this.Neighbours = new GridNode2D<T>[4];

            // This is the part where my use of the generics breaks apart.
            // Since my first use for T was to pass the prefab gameobject which holds the sprite and renderer,
            // I needed to instantiate it as a monobehaviour.
            // BUT this means I have to pass it all the way down which creates an extra dependency on the "parent" classes.
            this.Value = GameObject.Instantiate(value, new Vector3(x, y, 0.01f), Quaternion.identity, parent);
            Value.transform.parent = parent;
            Value.SetRandomTileSprite();
        }

        public float CalculatHeuristicDistanceToNode(GridNode2D<T> toNode)
        {
            // Euclidean distance: Sqr((X1 - X2)^2 + (Y1 - Y2)^2)
            HCost = Mathf.Sqrt(Mathf.Pow(X - toNode.X, 2) + Mathf.Pow(Y - toNode.Y, 2));
            return HCost;
        }

        public bool IsDiagonalTo(GridNode2D<T> node)
        {
            bool isDiagonal = false;

            Debug.Log(string.Format("Checking [{0}|{1}] against [{2}|{3}] for diagonality", X, Y, node.X, node.Y));

            if (((X == node.X + 1) || (X == node.X - 1))
                && ((Y == node.Y + 1) || (Y == node.Y - 1)))
                isDiagonal = true;

            return isDiagonal;
        }

        public void ResetCosts()
        {
            GCost = HCost = 0;
        }
    }
}