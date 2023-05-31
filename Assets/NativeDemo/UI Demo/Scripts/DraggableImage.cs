using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform imageTransform;
    private Vector2 originalPosition;

    private void Awake()
    {
        //imageTransform = GetComponent<RectTransform>();
        originalPosition = imageTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save the current position of the image before dragging begins
        originalPosition = imageTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the image to follow the mouse cursor
        imageTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If the image was not dragged to a new position, return it to its original position
        if (imageTransform.anchoredPosition == originalPosition)
        {
            imageTransform.anchoredPosition = originalPosition;
        }
    }
}
