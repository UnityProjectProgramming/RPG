using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
    [RequireComponent(typeof(UIItemSlot))]
    public class UIItemSlot_SetColorFromQuality : MonoBehaviour
    {
        [Serializable]
        public struct QualityToColor
        {
            public UIItemQuality quality;
            public Color color;
        }

        [SerializeField] private Graphic m_Target;
        [SerializeField] private QualityToColor[] m_QualitToColor;
        
        private UIItemSlot m_Slot;

        protected void Awake()
        {
            this.m_Slot = this.gameObject.GetComponent<UIItemSlot>();
        }

        protected void OnEnable()
        {
            this.m_Slot.onAssign.AddListener(OnSlotAssign);
            this.m_Slot.onUnassign.AddListener(OnSlotUnassign);
        }

        protected void OnDisable()
        {
            this.m_Slot.onAssign.RemoveListener(OnSlotAssign);
            this.m_Slot.onUnassign.RemoveListener(OnSlotUnassign);
        }

        public void OnSlotAssign(UIItemSlot slot)
        {
            if (this.m_Target == null || this.m_QualitToColor.Length == 0)
                return;

            // Find the slot quality
            foreach (QualityToColor qtc in this.m_QualitToColor)
            {
                if (qtc.quality == slot.GetItemInfo().Quality)
                {
                    this.m_Target.canvasRenderer.SetColor(qtc.color);
                    break;
                }
            }
        }

        public void OnSlotUnassign(UIItemSlot slot)
        {
            if (this.m_Target == null)
                return;

            this.m_Target.canvasRenderer.SetColor(Color.white);
        }
    }
}
