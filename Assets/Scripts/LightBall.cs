using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBall : MonoBehaviour
{
    [SerializeField] private Light2D light2d;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Player player;
    [SerializeField] private float lightOuterRadiusFadingSpeed = 2.0f;
    [SerializeField] private float force = 10.0f;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        float lightBallHorizontalSpeed = player.GetLightBallHorizontalSpeed();
        rb.velocity = new Vector2(Mathf.Sign(lightBallHorizontalSpeed) * force / 1.7f+ lightBallHorizontalSpeed, force / 1.7f);
    }
}
