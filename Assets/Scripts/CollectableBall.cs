using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBall : MonoBehaviour
{
    private void Awake() {
        GameManager.Instance.RegisterBall(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().OnCollect();
            gameObject.SetActive(false);
        }
    }
}
