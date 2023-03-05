using UnityEngine;
using TMPro;

namespace Sleep0.Logic.VisualDebug
{
    public class GridNodeUI : MonoBehaviour
    {
        [SerializeField]
        public TextMeshPro _gCost;
        
        [SerializeField]
        public TextMeshPro _hCost;

        [SerializeField]
        public TextMeshPro _fCost;

        [SerializeField]
        public TextMeshPro _weight;

        public void SetColor(Color newColor)
        {
            _gCost.color = _hCost.color = _fCost.color = _weight.color = newColor;
        }

        public void SetValues(int gCost, int hCost, int fCost, int weight)
        {
            SetValues((float)gCost, (float)hCost, (float)fCost, (float)weight);
        }

        public void SetValues(float gCost, float hCost, float fCost, float weight)
        {
            SetValues(string.Format("{0:0}", gCost),
                        string.Format("{0:0}", hCost),
                        string.Format("{0:0}", fCost),
                        string.Format("{0:0}", weight));
        }

        public void SetValues(string gCost, string hCost, string fCost, string weight)
        {
            _gCost.text = gCost;
            _hCost.text = hCost;
            _fCost.text = fCost;
            _weight.text = weight;
        }
    }
}