using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
