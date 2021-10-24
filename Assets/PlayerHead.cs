using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovement;

    void OnCollisionEnter2D(Collision2D collision) {
        playerMovement.ProcessHeadCollision(collision);
    }
}
