using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public List<Sprite> arrows;
    public SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetArrowSprite(int spriteNum) {
        animator.enabled = false;
        spriteRenderer.sprite = arrows[spriteNum];
    }

    public void PlayFullyChargedAnimation() {
        animator.enabled = true;
        animator.Play(Constants.ANIM_ARROW_CHARGED);
    }

    public void ShowSpriteRenderer(bool shouldShow = true) {
        spriteRenderer.enabled = shouldShow;
    }
}
