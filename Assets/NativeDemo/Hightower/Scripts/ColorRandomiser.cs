using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomiser : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _renderer;
    [SerializeField] private Color[] _colors;

    private void Awake()
    {
        Color c = _colors[Random.Range(0, _colors.Length)];
        foreach (var item in _renderer)
        {
            item.color = c;
        }
    }
}
