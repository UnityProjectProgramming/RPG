using UnityEngine;

namespace DuloGames.UI
{
    public class Test_SelectAddOption : MonoBehaviour {

        [SerializeField] private UISelectField m_SelectField;
        [SerializeField] private string m_Text;

        [ContextMenu("Add Option")]
        public void AddOption()
        {
            if (this.m_SelectField != null)
            {
                this.m_SelectField.AddOption(this.m_Text);
            }
        }
    }
}
