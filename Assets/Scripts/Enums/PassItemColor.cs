using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassItemColor
{
    Blue,
    Brown,
    Gray,
    Green,
    LightGreen,
    Magenta,
    Purple,
    Orange,
    Pink,
    Red,
    Rose,
    Sky,
    Teal,
    White,
    Yellow
}

// extensions

static public class PassItemColorExtensions
{
    static public Sprite GetLabelSpriteByColor(this PassItemColor color)
    {
        return Resources.Load<Sprite>($"Labels/Label-{color.ToString()}");
    }
    
    static public Sprite GetLabelDecoSpriteByColor(this PassItemColor color)
    {
        return Resources.Load<Sprite>($"Labels/Label-{color.ToString()}-Deco");
    }
}