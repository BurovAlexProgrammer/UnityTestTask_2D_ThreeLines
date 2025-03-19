﻿using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Services
{
    public class BallServices
    {
        [Inject] private AppSettings _appSettings;
        [Inject] private DiContainer _diContainer;
        
        private List<BallView> _balls = new();
        private BallView _boundBall;
        
        private readonly Vector2 _ballPositionLeft = new(-1.88f, 3.52f);
        private readonly Vector2 _ballPositionRight = new(1.88f, 3.52f);

        public event Action<BallView> BallCreated;
        public event Action<BallView> Destroyed;
        public event Action<BallView, Collider2D> BallTriggerEntered;
        public event Action<BallView> BallStopped;

        public BallView CreateBall()
        {
            var newBall = _diContainer.InstantiatePrefabForComponent<BallView>(_appSettings.BallPrefab);
            var isLeft = Random.Range(0, 2) == 0;
            var ballPosition = isLeft ? _ballPositionLeft : _ballPositionRight;
            newBall.Init(ballPosition);
            newBall.TriggerEntered += OnBallTriggerEntered;
            newBall.Stopped += OnBallStopped;
            _balls.Add(newBall);
            
            return newBall;
        }

        private void OnBallStopped(BallView ball)
        {
            BallStopped?.Invoke(ball);
        }

        private void OnBallTriggerEntered(BallView ballView, Collider2D columnTrigger)
        {
            BallTriggerEntered?.Invoke(ballView, columnTrigger);
        }

        public void RemoveBall(BallView ballView)
        {
            ballView.TriggerEntered -= OnBallTriggerEntered;
            ballView.Stopped -= OnBallStopped;
            _balls.Remove(ballView);
        }
    }
}