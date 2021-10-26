using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventDate) {
        GameController.instance.PlayAudio(SoundName.SfxButtonMouseOver);
    }
}
