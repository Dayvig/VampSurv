using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Assets/ModifierAttributes")]
public class ModifierAttributes : ScriptableObject
{
    public int priority;
    public List<float> factors = new List<float>();
    public enum Type
    {
        BULLET,
        SWORD
    }

    public Type type;
}
