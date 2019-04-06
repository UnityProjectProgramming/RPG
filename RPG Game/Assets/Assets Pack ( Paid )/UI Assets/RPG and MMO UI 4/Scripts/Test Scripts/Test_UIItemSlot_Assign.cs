using UnityEngine;

namespace DuloGames.UI
{
    public class Test_UIItemSlot_Assign : MonoBehaviour
    {
        [SerializeField] private UIItemSlot slot;
        [SerializeField] private int assignItem;

        void Awake()
        {
            if (this.slot == null)
                this.slot = this.GetComponent<UIItemSlot>();
        }

        void Start()
        {
            if (this.slot == null || UIItemDatabase.Instance == null)
            {
                this.Destruct();
                return;
            }

            this.slot.Assign(UIItemDatabase.Instance.GetByID(this.assignItem));
            this.Destruct();
        }

        private void Destruct()
        {
            DestroyImmediate(this);
        }
    }
}
