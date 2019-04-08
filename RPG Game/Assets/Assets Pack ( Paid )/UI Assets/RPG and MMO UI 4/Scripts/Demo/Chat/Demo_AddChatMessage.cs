using UnityEngine;

namespace DuloGames.UI
{
    public class Demo_AddChatMessage : MonoBehaviour
    {
        [SerializeField] private Demo_Chat m_Chat;
        [SerializeField] private string m_PlayerName = "Player";
        [SerializeField] private Color m_PlayerColor = Color.white;

        public void OnSendMessage(int tabId, string text)
        {
            if (this.m_Chat != null)
            {
                this.m_Chat.ReceiveChatMessage(tabId, "<color=#" + CommonColorBuffer.ColorToString(this.m_PlayerColor) + "><b>" + this.m_PlayerName + "</b></color> <color=#59524bff>said:</color>" + text);
            }
        }
    }
}
