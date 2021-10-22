using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerMovement))]
public class PlayerInput : MonoBehaviour {

    PlayerMovement player;

    void Start () {
        player = GetComponent<PlayerMovement> ();
    }

    void Update () {
        Vector2 directionalInput = new Vector2 (
            Input.GetAxisRaw (Constants.INPUT_AXIS_HORIZONTAL), 
            Input.GetAxisRaw (Constants.INPUT_AXIS_VERTICAL)
        );
        player.SetDirectionalInput (directionalInput);
    }
}