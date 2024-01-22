using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_FakeHP : MonoBehaviour
{
    [SerializeField] Image hPBar;
    [SerializeField] Gradient hPBarGradient;
    [SerializeField] float recoverRate = 0.5f;
    [SerializeField] float healDelay = 2f;


    Coroutine rHP;
    Coroutine iHeal;
    Coroutine cShake;

    public float shakeMagnitude = 0.2f;
    public float shakeDuration = 0.5f;


    public void SubtractHP()
        { 
            if(hPBar.fillAmount > 0)
                {
                    hPBar.fillAmount -= 0.10f;
                }
            hPBar.color = hPBarGradient.Evaluate(hPBar.fillAmount);


            if (cShake != null) StopCoroutine(cShake);
            cShake = StartCoroutine(Shake.Shaker(hPBar.transform.parent.transform, shakeMagnitude, shakeDuration));
            InitHeal();
        }
    public void RemoveAllHP()
        { 
            hPBar.fillAmount = 0;
            hPBar.color = hPBarGradient.Evaluate(0);
            cShake = StartCoroutine(Shake.Shaker(hPBar.transform.parent.transform, shakeMagnitude * 2, shakeDuration * 2));
            InitHeal();
        }

    void InitHeal()
        {
            if(iHeal != null) StopCoroutine(iHeal);
            iHeal = StartCoroutine(StartRecovery());
        }
    IEnumerator StartRecovery()
        {
            yield return new WaitForSeconds(healDelay);

            if(rHP != null) StopCoroutine(rHP);
            rHP = StartCoroutine(RecoverHP());
        }

    IEnumerator RecoverHP()
        {

            while(hPBar.fillAmount < 1)
                {
                    hPBar.fillAmount += Time.deltaTime * recoverRate;

                    if (hPBar.fillAmount > 1) hPBar.fillAmount = 1;

                    hPBar.color = hPBarGradient.Evaluate(hPBar.fillAmount);
                    yield return null;
                }
        }
}
