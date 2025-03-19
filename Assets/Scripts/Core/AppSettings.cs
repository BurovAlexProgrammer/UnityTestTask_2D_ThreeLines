using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Core/AppSettings", fileName = "AppSettings")]
    public class AppSettings : ScriptableObject
    {
        [field:SerializeField] public Color[] Colors { get; private set; }
        [field:SerializeField] public BallView BallPrefab { get; private set; }

    }
}