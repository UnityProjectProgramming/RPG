using UnityEngine;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
    public class Demo_CharacterSelectChar : MonoBehaviour
    {
        private Demo_CharacterInfo m_Info;
        private int m_Index;

        public Demo_CharacterInfo info
        {
            get { return this.m_Info; }
            set { this.m_Info = value; }
        }

        public int index
        {
            get { return this.m_Index; }
            set { this.m_Index = value; }
        }

        private void OnMouseDown()
        {
            if (this.m_Info == null)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Demo_CharacterSelectMgr.instance != null)
            {
                Demo_CharacterSelectMgr.instance.SelectCharacter(this);
            }
        }
    }
}
