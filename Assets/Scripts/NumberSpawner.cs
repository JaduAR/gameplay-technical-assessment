using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

#pragma warning disable  114

public class NumberSpawner : Singleton<NumberSpawner> {
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private float textLifeSpan = 1.0f;

    private Queue<TextMeshProUGUI> numberPool = new Queue<TextMeshProUGUI>();

    private void Awake() {
        base.Awake();
        for (int i = 0; i < 5; i++) {
            SpawnNumber(transform, true);
        }
    }

    private TextMeshProUGUI SpawnNumber(Transform t, bool init = false) {
        TextMeshProUGUI numText = Instantiate(numberText, t.position, t.rotation).GetComponent<TextMeshProUGUI>();
        numText.transform.SetParent(transform, false);

        if (init) {
            Despawn(numText);
        }

        return numText;
    }

    private void Despawn(TextMeshProUGUI numText) {
        numText.enabled = false;
        numberPool.Enqueue(numText);
    }

    public void ShowNumber(Transform t, int num) {
        TextMeshProUGUI numText;

        if (numberPool.Count > 0) {
            numText = numberPool.Dequeue();
            numText.transform.position = t.position;
            numText.transform.rotation = t.rotation;
            numText.enabled = true;
        }
        else {
            numText = SpawnNumber(t);
        }

        numText.text = num.ToString();
        numText.transform.DOMove(numText.transform.position + new Vector3(0f, 1.2f, 0f), textLifeSpan).SetEase(Ease.OutQuad);

        StartCoroutine(TextLifeSpan(numText));
    }

    IEnumerator TextLifeSpan(TextMeshProUGUI numText) {
        float timeSpan = 0.0f;

        while (true) {
            numText.transform.LookAt(numText.transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);

            timeSpan += Time.deltaTime;

            if (timeSpan >= textLifeSpan) {
                numText.enabled = false;
                numberPool.Enqueue(numText);
                break;
            }

            yield return null;
        }
    }
}

#pragma warning restore 114