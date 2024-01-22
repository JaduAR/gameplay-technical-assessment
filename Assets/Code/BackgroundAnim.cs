using UnityEngine;
using DG.Tweening;

public class BackgroundAnim : Singleton<BackgroundAnim> {
    [SerializeField] private Material material;
    [SerializeField] private Vector2 moveSpeed = new Vector2(1f, 1f);

    void FixedUpdate() {
        Vector2 offset = material.mainTextureOffset;
        offset += moveSpeed * Time.fixedDeltaTime;
        material.mainTextureOffset = offset;
    }
}
