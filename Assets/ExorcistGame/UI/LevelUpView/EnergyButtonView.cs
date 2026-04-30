using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExorcistGame.UI.LevelUpView
{
    public class EnergyButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private SkillData skillData;
        [SerializeField] private Image mainImage;
        [SerializeField] private TextMeshProUGUI nameText;

        public GameObject dragPanel;
        public DragPanelView panelDraggable;
        
        private bool _isDragging = false;
        
        private readonly UnityEvent _onBeginDrag = new UnityEvent();
        private readonly UnityEvent _onDrag = new UnityEvent();
        private readonly UnityEvent _onEndDrag = new UnityEvent();
        
        private void OnValidate()
        {
            if(skillData == null) return;

            mainImage.sprite = skillData.mainSprite;
            nameText.text = skillData.name;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            dragPanel.SetActive(true);
            
            _onBeginDrag?.Invoke();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            panelDraggable.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            panelDraggable.OnDrag(eventData);
            
            _onDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            panelDraggable.OnEndDrag(eventData);
            dragPanel.SetActive(false);
            
            _onEndDrag?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                dragPanel.SetActive(false);
                
                _onEndDrag?.Invoke();
            }
        }

        public void AddBeginDragEvent(UnityAction action)
        {
            _onBeginDrag.AddListener(action);
        }

        public void AddDragEvent(UnityAction action)
        {
            _onDrag.AddListener(action);
        }

        public void AddEndDragEvent(UnityAction action)
        {
            _onEndDrag.AddListener(action);
        }
    }
}
