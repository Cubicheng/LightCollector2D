using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTrigger : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canvas.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }
}
