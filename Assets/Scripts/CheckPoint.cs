using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckPoint : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private Light2D light2D;
    [SerializeField] private Light2D light2DofVisual;

    private void Start() {
        SetLightIsNotCheckPoint();
    }

    private static CheckPoint currentCheckPoint;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (currentCheckPoint != this) {
                if (currentCheckPoint != null) {
                    currentCheckPoint.animator.SetBool("IsCheckPoint", false);
                    currentCheckPoint.SetLightIsNotCheckPoint();
                }
                currentCheckPoint = this;
                animator.SetBool("IsCheckPoint", true);
                SetLightIsCheckPoint();
            }
            other.GetComponent<Player>().SetHeal(true);
            GameManager.Instance.SetRespawnPoint(transform.position);
            GameManager.Instance.ResetCollectableBall();
            SoundManager.Instance.OnCheckPoint();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().SetHeal(false);
        }
    }

    private void SetLightIsCheckPoint() {
        if (gameObject.GetComponent<SkillCheckPoint>() == null) {
            //绿色，打开
            SetLight2d(light2D, new Color32(105, 247, 110, 255), 0.82f, 1.96f, 5.53f, 0.37f);
            SetLight2d(light2DofVisual, new Color32(207, 217, 181, 255), 0.89f, 2.38f, 9.61f, 0.35f);
        } else {
            //紫色，打开
            SetLight2d(light2D, new Color32(244, 186, 173, 255), 1.25f, 0.47f, 6.28f, 0.306f);
            SetLight2d(light2DofVisual, new Color32(236, 106, 203, 255), 1.18f, 0, 9.42f, 0.204f);
        }
    }

    private void SetLightIsNotCheckPoint() {
        if (gameObject.GetComponent<SkillCheckPoint>() == null) {
            //绿色，关闭
            SetLight2d(light2D, new Color32(39, 149, 34, 255), 0.85f, 1.65f, 5.41f, 0.33f);
            SetLight2d(light2DofVisual, new Color32(229, 236, 161, 255), 0.58f, 0.93f, 9.01f, 0.43f);
        } else {
            //紫色，关闭
            SetLight2d(light2D, new Color32(135, 53, 200, 255), 0.6f, 0.4f, 7.85f, 0.021f);
            SetLight2d(light2DofVisual, new Color32(189, 112, 112, 255), 0.56f, 1.33f, 5.65f, 0.26f);
        }
    }

    private void SetLight2d(Light2D light2d,Color32 color32,float intensity,float innerRadius,float outerRadius,float fallofStrength) {
        light2d.color = color32;
        light2d.intensity = intensity;
        light2d.pointLightInnerRadius = innerRadius;
        light2d.pointLightOuterRadius = outerRadius;
        light2d.falloffIntensity = fallofStrength;
    }
}