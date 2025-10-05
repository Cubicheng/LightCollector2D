using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBall : MonoBehaviour
{
    private GameObject player;

    private void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("enter");
        if (other.CompareTag("Player")) {
            CollectCoin();
        }
    }

    public void CollectCoin() {
        player.GetComponent<Player>().OnCollect();
        Destroy(gameObject);
    }
}
