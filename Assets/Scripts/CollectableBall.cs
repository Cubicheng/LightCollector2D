using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CollectableBall : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Light2D light2d;
    private const float RespawnAnimationDuration = 0.6f;
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
        StartCoroutine(RespawnAnimationRoutine());
    }

    IEnumerator RespawnAnimationRoutine() {
        light2d.intensity = 0;
        transform.localScale = Vector3.zero;
        float time = 0;
        while (time < RespawnAnimationDuration) {
            time += Time.deltaTime;
            light2d.intensity = Mathf.Lerp(0, 1, time / RespawnAnimationDuration);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / RespawnAnimationDuration);
            yield return null;
        }
        light2d.intensity = 1;
        transform.localScale = Vector3.one;
    }
}
