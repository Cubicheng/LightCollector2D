using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBall : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start() {
        GameManager.Instance.RegisterBall(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().OnCollect();
            gameObject.SetActive(false);
        }
    }

    public void Respawn() {
        gameObject.SetActive(true);
        animator.SetTrigger(AnimatorParams.CollectableBallRespawn);
    }
}
