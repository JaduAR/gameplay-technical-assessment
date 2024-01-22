using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : Singleton<EnemyController>  {
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private AvatarBuilder avatarBuilder;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject particleHit;
    [SerializeField] private GameObject explosion;
    [SerializeField] private int health = 100;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float moveSpeed = 5f;

    private int startHealth;
    private bool dead = false;

    void Start() {
        startHealth = health;
    }

    void Hit(int damage, Transform t) {
        health -= damage;
        health = Mathf.Max(health, 0);

        GameGUI.Instance.SetHealthBar((float)health / startHealth);

        NumberSpawner.Instance.ShowNumber(transform, damage);

        if (health == 0) {
            dead = true;
            skinnedMeshRenderer.enabled = false;

            ParticleManager.Instance.SpawnParticle(explosion, transform);
            GameAudio.Instance.PlayMusic(GameAudio.Instance.win, false);
            GameAudio.Instance.PlaySFX(GameAudio.Instance.explosion);
            GameManager.Instance.Complete();
            return;
        }

        foreach (Material mat in skinnedMeshRenderer.materials) {
            if (mat.HasProperty("baseColorFactor"))  {
                mat.SetColor("baseColorFactor", Color.red);
                GameAudio.Instance.PlaySFX(GameAudio.Instance.punch);
                mat.DOColor(Color.white, "baseColorFactor", 1.0f);
            }
        }

        ParticleManager.Instance.SpawnParticle(particleHit, t);
    }

    void FixedUpdate() {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        avatarBuilder.Strafe(0, moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerStay(Collider other) {
        if (dead) {
            return;
        }

        if (other.tag == "LeftHand" && player.LeftPunch()) {
            Hit(10, other.transform);
        } else if (other.tag == "RightHand") {
            if (player.RightPunch()) {
                Hit(10, other.transform);
            } else if (player.PowerPunch()) {
                Hit(100, other.transform);
            }
        } 
    }
}
