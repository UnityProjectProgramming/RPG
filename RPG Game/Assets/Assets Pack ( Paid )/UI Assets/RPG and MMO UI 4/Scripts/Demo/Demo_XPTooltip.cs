using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace DuloGames.UI
{
    public class Demo_XPTooltip : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject m_TooltipObject;
        [SerializeField] private Image m_FillImage;
        [SerializeField] private float m_OffsetY = 0f;

        [SerializeField, Tooltip("How long of a delay to expect before showing the tooltip."), Range(0f, 10f)]
        private float m_Delay = 1f;

        private bool m_IsTooltipShown = false;

        protected override void Awake()
        {
            base.Awake();

            if (this.m_TooltipObject != null)
            {
                RectTransform tooltipRect = (this.m_TooltipObject.transform as RectTransform);
                tooltipRect.anchorMin = new Vector2(0f, 1f);
                tooltipRect.anchorMax = new Vector2(0f, 1f);
                tooltipRect.pivot = new Vector2(0.5f, 0f);
                this.m_TooltipObject.SetActive(false);
            }
        }

        /// <summary>
        /// Raises the tooltip event.
        /// </summary>
        /// <param name="show">If set to <c>true</c> show.</param>
        public virtual void OnTooltip(bool show)
        {
            if (this.m_TooltipObject == null || this.m_FillImage == null)
                return;

            RectTransform tooltipRect = (this.m_TooltipObject.transform as RectTransform);
            RectTransform fillRect = (this.m_FillImage.transform as RectTransform);

            if (show)
            {
                // Change the parent so we can calculate the position correctly
                tooltipRect.SetParent(this.m_FillImage.transform, true);

                // Change the position based on fill
                tooltipRect.anchoredPosition = new Vector2(fillRect.rect.width * this.m_FillImage.fillAmount, this.m_OffsetY);

                // Bring to top
                UIUtility.BringToFront(this.m_TooltipObject);

                // Enable the tooltip
                this.m_TooltipObject.SetActive(true);
            }
            else
            {
                // Disable the tooltip
                this.m_TooltipObject.SetActive(false);
            }
        }

        /// <summary>
        /// Raises the pointer enter event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            // Check if tooltip is enabled
            if (this.enabled && this.IsActive())
            {
                // Start the tooltip delayed show coroutine
                // If delay is set at all
                if (this.m_Delay > 0f)
                {
                    this.StartCoroutine("DelayedShow");
                }
                else
                {
                    this.InternalShowTooltip();
                }
            }
        }

        /// <summary>
        /// Raises the pointer exit event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this.InternalHideTooltip();
        }

        /// <summary>
		/// Internal call for show tooltip.
		/// </summary>
		protected void InternalShowTooltip()
        {
            // Call the on tooltip only if it's currently not shown
            if (!this.m_IsTooltipShown)
            {
                this.m_IsTooltipShown = true;
                this.OnTooltip(true);
            }
        }

        /// <summary>
        /// Internal call for hide tooltip.
        /// </summary>
        protected void InternalHideTooltip()
        {
            // Cancel the delayed show coroutine
            this.StopCoroutine("DelayedShow");

            // Call the on tooltip only if it's currently shown
            if (this.m_IsTooltipShown)
            {
                this.m_IsTooltipShown = false;
                this.OnTooltip(false);
            }
        }

        protected IEnumerator DelayedShow()
        {
            yield return new WaitForSeconds(this.m_Delay);
            this.InternalShowTooltip();
        }
    }
}
