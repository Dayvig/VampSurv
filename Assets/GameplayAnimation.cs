using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayAnimation : ScriptableObject
{
    public List<Vector2> positions = new List<Vector2>();
    public List<float> rotations = new List<float>();
    public List<float> durations = new List<float>();
    public List<float> specialActivationTimings = new List<float>();
    public List<float> arcLengths = new List<float>();
    public List<GameObject> hitboxes = new List<GameObject>();
    public List<GameObject> hitboxObjects = new List<GameObject>();

    public float timer = 0.0f;
    public float specialTimer = 0.0f;
    public bool active = false;
    public int stage = 0;
    public int specialStage = 0;

    public void Reset()
    {
        timer = 0.0f;
        specialTimer = 0.0f;
        active = false;
        stage = 0;
        specialStage = 0;
    }
    public abstract void activateSpecialEffects(int timing, float angle, List<GameObject> hitboxes, GameObject owner);
}
