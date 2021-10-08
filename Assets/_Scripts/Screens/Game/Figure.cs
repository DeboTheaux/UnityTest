using System;
using UnityEngine;

[Serializable]
public class Figure : MonoBehaviour //implementar interfaces...?
{
    [SerializeField] protected string id;

    public string Id => id;
}