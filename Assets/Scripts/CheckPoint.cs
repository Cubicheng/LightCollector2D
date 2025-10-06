using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().SetHeal(true);
            GameManager.Instance.SetRespawnPoint(transform.position);
            GameManager.Instance.ResetCollectableBall();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().SetHeal(false);
        }
    }
}
