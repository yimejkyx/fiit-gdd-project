﻿using System;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Color color;
    private Transform bar;
    private Transform barSprite;
    private SpriteRenderer barSpriteSprite;

    void Awake()
    {
        bar = transform.Find("Bar");
        barSprite = bar.Find("BarSprite");
        barSpriteSprite = barSprite.GetComponent<SpriteRenderer>();
        Debug.Log("debug1" + barSprite);
        Debug.Log("debug2" + barSpriteSprite);
    }


    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, bar.localScale.y);
    }

    public void SetColor(Color color)
    {
        barSpriteSprite.color = color;
    }
}
