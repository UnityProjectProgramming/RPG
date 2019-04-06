using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    public class Demo_QuestTrackerCollapse : UIBehaviour, ILayoutSelfController
    {
        public enum FitMode
        {
            Unconstrained,
            MinSize,
            PreferredSize
        }

        [SerializeField][HideInInspector] protected FitMode m_HorizontalFit = FitMode.Unconstrained;
        public FitMode horizontalFit { get { return m_HorizontalFit; } set { this.m_HorizontalFit = value; SetDirty(); } }

        [SerializeField][HideInInspector] protected FitMode m_VerticalFit = FitMode.Unconstrained;
        public FitMode verticalFit { get { return m_VerticalFit; } set { this.m_VerticalFit = value; SetDirty(); } }

        [SerializeField] private Toggle m_Toggle;
        [SerializeField] private UIFlippable m_ArrowFlippable;
        [SerializeField] private bool m_ArrowInvertFlip = false;

        [System.NonSerialized]
        private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        private DrivenRectTransformTracker m_Tracker;

        #region Unity Lifetime calls

        protected override void Awake()
        {
            base.Awake();

            // Make sure the mask graphic is not displayed
            this.gameObject.GetComponent<Mask>().showMaskGraphic = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();

            // Hook the toggle change event
            if (this.m_Toggle != null)
            {
                this.m_Toggle.onValueChanged.AddListener(OnToggleStateChange);
            }

            // Apply the current toggle state
            if (this.m_Toggle != null)
            {
                this.OnToggleStateChange(this.m_Toggle.isOn);
            }
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();

            // Unhook the toggle change event
            if (this.m_Toggle != null)
            {
                this.m_Toggle.onValueChanged.RemoveListener(OnToggleStateChange);
            }

            // Expand the view
            this.OnToggleStateChange(false);
        }

        #endregion

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        public void OnToggleStateChange(bool state)
        {
            if (!IsActive())
                return;

            if (state)
            {
                this.verticalFit = FitMode.PreferredSize;

                if (this.m_ArrowFlippable != null)
                {
                    this.m_ArrowFlippable.vertical = (this.m_ArrowInvertFlip ? false : true);
                }
            }
            else
            {
                this.verticalFit = FitMode.Unconstrained;
                this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);

                if (this.m_ArrowFlippable != null)
                {
                    this.m_ArrowFlippable.vertical = (this.m_ArrowInvertFlip ? true : false);
                }
            }
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            if (fitting == FitMode.Unconstrained)
                return;

            m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));

            // Set size to min or preferred size
            if (fitting == FitMode.MinSize)
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(m_Rect, axis));
            else
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(m_Rect, axis));
        }

        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
#endif
    }
}
