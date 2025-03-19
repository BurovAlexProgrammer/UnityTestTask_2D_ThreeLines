using System;
using UnityEngine.Serialization;

namespace Core
{
    using UnityEngine;

    [RequireComponent(typeof(LineRenderer))]
    public class RopeRenderer : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _ball;
        [SerializeField] private LineRenderer _lineRenderer;

        private void OnValidate()
        {
            _lineRenderer ??= GetComponent<LineRenderer>();
        }

        void Start()
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = 0.05f;
            _lineRenderer.endWidth = 0.05f;
        }

        void Update()
        {
            _lineRenderer.SetPosition(0, _pivot.position);
            _lineRenderer.SetPosition(1, _ball.position);
        }
    }
}