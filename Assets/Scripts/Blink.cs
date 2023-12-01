using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blink : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        StartCoroutine("BlinkCoroutine");
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            text.text = "";
            yield return new WaitForSeconds(0.5f);
            text.text = "Touch To Start";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
