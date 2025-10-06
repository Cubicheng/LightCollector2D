using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SkillType {
    DoubleJump,
    Throw,
    Pika
}

public class SkillCheckPoint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [Tooltip("选择这个检查点增加的技能类型")]
    public SkillType skillToIncrease;

    private GameObject player;

    void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player.GetComponent<Player>().SetHeal(true);
            switch (skillToIncrease) {
                case SkillType.DoubleJump: {
                        if (player.GetComponent<Player>().GetMaxAirJumpCount() == 0) {
                            player.GetComponent<Player>().SetMaxAirJumpCount(1);
                            text.enabled = true;
                            text.text = "Acquired Double Jump ability!";
                        }
                        break;
                    }
                case SkillType.Throw: {
                        if (!player.GetComponent<Player>().HasThrowSkill()) {
                            player.GetComponent<Player>().SetHasThrowSkill(true);
                            text.enabled = true;
                            text.text = "Learned Light Ball Throw!\nPress K to Throw!";
                        }
                        break;
                    }
                case SkillType.Pika: {
                        if (!player.GetComponent<Player>().HasPikaSkill()) {
                            player.GetComponent<Player>().SetHasPikaSkill(true);
                            text.enabled = true;
                            text.text = "Mastered Flash ability!\nPress J to Flash!";
                        }
                        break;
                    }
            }
        }
    }

}
