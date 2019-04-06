using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
    [ExecuteInEditMode]
    public class Demo_SceneButton : MonoBehaviour {

        [SerializeField] private UIScene m_Scene;
        [SerializeField] private Image m_Active;
        [SerializeField] private Text m_Text;
        [SerializeField] private Color m_NormalColor = Color.white;
        [SerializeField] private Color m_ActiveColor = Color.white;

        [SerializeField] private bool m_IsAcitve = false;

        public bool isActive
        {
            get { return this.m_Active; }
            set { this.m_IsAcitve = value; this.UpdateState(); }
        }

        private void Start()
        {
            if (this.m_Scene != null)
            {
                this.m_IsAcitve = this.m_Scene.isActivated;
                this.UpdateState();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.UpdateState();
        }
#endif

        public void OnClick()
        {
            if (!this.m_IsAcitve)
            {
                Demo_SceneButton[] buttons = this.transform.parent.gameObject.GetComponentsInChildren<Demo_SceneButton>();

                // Deactivate all buttons
                foreach (Demo_SceneButton button in buttons)
                {
                    button.isActive = false;
                }

                // Active this one
                this.isActive = true;
            }

            // Activate the scene
            if (this.m_Scene != null && !this.m_Scene.isActivated)
            {
                this.m_Scene.TransitionTo();
            }
        }

        public void UpdateState()
        {
            if (this.m_Active != null) this.m_Active.enabled = this.m_IsAcitve;
            if (this.m_Text != null) this.m_Text.color = (this.m_IsAcitve) ? this.m_ActiveColor : this.m_NormalColor;
        }
    }
}
