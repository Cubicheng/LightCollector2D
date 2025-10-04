using UnityEngine;

// ����һ����̬�࣬�����洢����Animator������Hash ID
public static class AnimatorParams {

    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsJump = Animator.StringToHash("IsJump");
    public static readonly int IsAirJump = Animator.StringToHash("IsAirJump");
    public static readonly int IsPika = Animator.StringToHash("IsPika");
    public static readonly int IsLand = Animator.StringToHash("IsLand");
}