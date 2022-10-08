using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>().inStack)

            eventData.pointerDrag.GetComponent<Card>().moveBackCards();
        else
            eventData.pointerDrag.GetComponent<Card>().revertPosition();

    }
}
