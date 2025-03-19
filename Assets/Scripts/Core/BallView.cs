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


        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEntered?.Invoke(this, other);   
        }


        private void Update()
        {
            if (_isSleeping != _rigidbody.IsSleeping())
            {
                _isSleeping = _rigidbody.IsSleeping();

                if (_isSleeping)
                {
                    Stopped?.Invoke(this);
                    Debug.Log("Stopped: ", this);
                }
            }
        }
    }
}