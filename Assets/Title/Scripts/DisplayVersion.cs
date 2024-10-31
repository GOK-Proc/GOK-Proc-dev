using TMPro;
using UnityEngine;

namespace Title
{
    public class DisplayVersion : MonoBehaviour
    {
        private TextMeshProUGUI textMeshProUGUI;    

        void Start()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = $"Ver. {Application.version}";
        }
    }
}
