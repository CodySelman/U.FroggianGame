using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    [SerializeField]
    private GameObject winMenu;

    void Start() {
        winMenu.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_PLAYER)) {
            GameController.instance.WinGame();
            winMenu.SetActive(true);
        }
    }
}
