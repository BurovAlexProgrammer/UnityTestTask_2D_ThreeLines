using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core
{
    public class BallView  : MonoBehaviour
    {
        [Inject] private AppSettings _appSettings;

        [SerializeField] private SpriteRenderer _sprite;
        
        private Rigidbody2D _rigidbody;
        private bool _isSleeping;
        private bool _isRealized;

        public bool IsIntoColumn { get; private set; }
        public Color Color { get; private set; }

        public event Action<BallView, Collider2D> TriggerEntered; 
        public event Action<BallView> Stopped; 
        
        public void Init(Vector2 ballPosition)
        {
            transform.position = ballPosition;
            _rigidbody = GetComponent<Rigidbody2D>();
            Color = _appSettings.Colors[Random.Range(0, _appSettings.Colors.Length)];
            _sprite.color = Color;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEntered?.Invoke(this, other);
            IsIntoColumn = true;
        }


        private void Update()
        {
            if (_isRealized) return;
            
            if (_isSleeping != _rigidbody.IsSleeping())
            {
                _isSleeping = _rigidbody.IsSleeping();

                if (_isSleeping)
                {
                    _isRealized = true;
                    _rigidbody.bodyType = RigidbodyType2D.Kinematic;
                    Stopped?.Invoke(this);
                    Debug.Log("Stopped: ", this);
                }
            }
        }
    }
}