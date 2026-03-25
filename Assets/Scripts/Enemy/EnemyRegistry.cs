using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    public static EnemyRegistry Instance;
    public static readonly List<Transform> Enemies = new();
    public static readonly List<Transform> Bros = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void RegisterEnemy(Transform t)
    {
        if (!Enemies.Contains(t))
            Enemies.Add(t);
    }

    public static void UnregisterEnemy(Transform t) => Enemies.Remove(t);

    public static void RegisterBro(Transform t)
    {
        if (!Bros.Contains(t))
            Bros.Add(t);
    }

    public static void UnregisterBro(Transform t) => Bros.Remove(t);

    public static Transform GetPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        return p != null ? p.transform : null;
    }
}