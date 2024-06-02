using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomiser : MonoBehaviour
{
    public bool AutoSetColor = false;

    [SerializeField] private SpriteRenderer[] _renderer;
    [SerializeField] private Color[] _colors;

    private void Awake()
    {
        if (AutoSetColor)
        {
            Color c = _colors[Random.Range(0, _colors.Length)];
            SetCharacterColor(c);
        }
    }


    public void SetCharacterColor(Color c)
    {
        foreach (var item in _renderer)
        {
            item.color = c;
        }
    }


}
