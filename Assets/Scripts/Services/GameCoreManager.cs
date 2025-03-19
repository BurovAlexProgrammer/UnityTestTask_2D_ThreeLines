using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Zenject;

namespace Services
{
    public class GameCoreManager : MonoBehaviour
    {
        [Inject] private BallServices _ballServices;

        [SerializeField] private SpringJoint2D _springJoint;
        [SerializeField] private List<Collider2D> _columnTriggers;

        private BallView[,] _pinnedBalls = new BallView[3,3];
        private BallView _connectedBall;

        private void Start()
        {
            CreateBall();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                CutRope();
            }
        }

        private void CutRope()
        {
            _springJoint.connectedBody = null;
        }

        private void CreateBall()
        {
            _ballServices.BallTriggerEntered += BallServices_OnTriggerEntered;
            _ballServices.BallStopped += BallServices_OnBallStopped;
            
            _connectedBall = _ballServices.CreateBall();
            _springJoint.connectedBody = _connectedBall.GetComponent<Rigidbody2D>();
        }

        private void BallServices_OnBallStopped(BallView ball)
        {
            Refresh();
        }

        private void BallServices_OnTriggerEntered(BallView ballView, Collider2D columnTrigger)
        {
            var columnIndex = _columnTriggers.IndexOf(columnTrigger);
            
            if (columnIndex < 0)
            {
                Debug.LogError("Wrong index");
                return;
            }
            
            for (var rowIndex = 0; rowIndex < 3; rowIndex++)
            {
                if (_pinnedBalls[columnIndex, rowIndex] == null)
                {
                    _pinnedBalls[columnIndex, rowIndex] = ballView;
                    Debug.Log($"Ball placed at [{columnIndex}, {rowIndex}]");
                    return;
                }
            }
            
            Debug.LogError($"Column {columnIndex} is full! Ball cannot be placed.");
            _ballServices.RemoveBall(ballView);
        }

        private void Refresh()
        {
            
        }
    }
}