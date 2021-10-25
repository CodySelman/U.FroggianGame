using System;
using UnityEngine;

public static class Utilities
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static bool IsAnimationFinished(Animator animator, string animationName) {
        AnimatorStateInfo currentAnimState = animator.GetCurrentAnimatorStateInfo(0);
        return currentAnimState.IsName(animationName) 
            && currentAnimState.normalizedTime >= 1;
    }

    private static bool IsAnimationPlayingAndFinished(Animator animator, string animationName) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            && IsAnimationFinished(animator, animationName);
    }
}
