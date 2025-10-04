using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBall : MonoBehaviour
{
    [SerializeField] private Light2D light2d;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject player;
    [SerializeField] private float lightOuterRadiusFadingSpeed = 2.0f;
    [SerializeField] private float force = 15.0f;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        float facingValue = player.GetComponent<Player>().GetFacingValue();
        rb.velocity = new Vector2(facingValue * force / 1.7f, force / 1.7f);
    }

    private void FixedUpdate() {
        UpdateLight();
    }

    private void UpdateLight() {
        light2d.pointLightOuterRadius -= lightOuterRadiusFadingSpeed * Time.fixedDeltaTime;
        if (light2d.pointLightOuterRadius <= 0) {
            light2d.pointLightOuterRadius = 0;
            Destroy(gameObject);
        }
    }
}
