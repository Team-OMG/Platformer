using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Warning_Animation : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Animation());
    }

    SpriteRenderer Render;

    IEnumerator Animation()
    {
        this.transform.DOMoveY(transform.position.y + 2, 1f);
        yield return new WaitForSeconds(1f);
        Render = GetComponent<SpriteRenderer>();
        Render.DOFade(0, 1);

    }
}
