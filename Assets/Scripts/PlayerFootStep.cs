using UnityEngine;

public class PlayerFootStep : MonoBehaviour {
    [SerializeField] private float DELTA_TIME = 0.15f;
    private float duration;
    private void Awake() {
        duration = DELTA_TIME;
    }

    private void Update() {
        if (!(Player.Instance.IsWalk() && Player.Instance.IsGround()&&!Player.Instance.IsDead())) {
            duration = DELTA_TIME;
            return;
        }
        duration += Time.deltaTime;
        if (duration > DELTA_TIME) {
            duration = 0;
            SoundManager.Instance.PlayFootstepSound();
        }
    }

}
