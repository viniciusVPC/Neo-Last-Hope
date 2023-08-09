using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Range(0, 15)]
    public int emotion;
    [TextArea(3, 1)]
    public string sentence;
}
