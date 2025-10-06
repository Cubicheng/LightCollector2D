using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player.GetComponent<Player>().SetHeal(true);
            GameManager.Instance.SetRespawnPoint(transform.position);
            GameManager.Instance.ResetCollectableBall();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player.GetComponent<Player>().SetHeal(false);
        }
    }
}
