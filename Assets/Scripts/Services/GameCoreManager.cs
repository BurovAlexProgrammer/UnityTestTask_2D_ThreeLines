using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Services
{
    public class GameCoreManager : MonoBehaviour
    {
        [Inject] private BallServices _ballServices;

        [SerializeField] private SpringJoint2D _springJoint;
        [SerializeField] private List<Collider2D> _columnTriggers;

        private BallView[,] _pinnedBalls = new BallView[Dimension, Dimension];
        private BallView _currentBall;

        private const int Dimension = 3;

        private void Start()
        {
            Time.timeScale = 2f;
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
            
            _currentBall = _ballServices.CreateBall();
            _springJoint.connectedBody = _currentBall.GetComponent<Rigidbody2D>();
        }

        private void RemoveBall(BallView ball)
        {
            var ballCoords = FindBallIndex(ball);
            
            _ballServices.RemoveBall(ball);

            if (ballCoords == null)
            {
                Debug.LogError($"pinnedBall not found in coors: {ballCoords}");
                return;
            }

            _pinnedBalls[ballCoords.Value.x, ballCoords.Value.y] = null;
        } 

        private void BallServices_OnBallStopped(BallView ball)
        {
            Refresh();
        }

        private void BallServices_OnTriggerEntered(BallView ballView, Collider2D columnTrigger)
        {
            if (_currentBall == null) return;

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
                    _currentBall = null;
                    return;
                }
            }

            Debug.LogError($"Column {columnIndex} is full! Ball cannot be placed.");
            RemoveBall(ballView);
            
            Refresh();
        }

        private void Refresh()
        {
            if (_currentBall == null)
            {
                CreateBall();
            }

            FindLines();
        }

        private void FindLines()
        {
            for (int i = 0; i < Dimension; i++)
            {
                FindLine(_pinnedBalls, i, 0, 0, 1, Dimension); // Горизонтальная линия
                FindLine(_pinnedBalls, 0, i, 1, 0, Dimension); // Вертикальная линия
            }

            FindLine(_pinnedBalls, 0, 0, 1, 1, Dimension); // Главная диагональ вниз
            FindLine(_pinnedBalls, Dimension - 1, 0, -1, 1, Dimension); // Побочная диагональ вверх
        }

        private  void FindLine(BallView[,] ballView, int startX, int startY, int dx, int dy, int length)
        {
            var currentLine = new List<Vector2Int>();
            var prevColor = ballView[startX, startY]?.Color;
        
            for (var i = 0; i < length; i++)
            {
                var x = startX + i * dx;
                var y = startY + i * dy;
                var color = ballView[x, y]?.Color;
            
                if (color != null && color == prevColor)
                {
                    currentLine.Add(new Vector2Int(x, y));
                }
                else
                {
                    return;
                }
            }

            if (currentLine.Count == length)
            {
                ReleaseLine(currentLine);
            }
        }

        private async UniTask ReleaseLine(List<Vector2Int> lineCoords)
        {
            Debug.Log($"ReleaseLine {lineCoords.ToString()}");
            
            foreach (var coord in lineCoords)
            {
                var ball = _pinnedBalls[coord.x, coord.y];

                if (ball == null)
                {
                    Debug.LogError($"Ball[{coord.x},{coord.y}] not found");
                    return;
                }
                
                RemoveBall(ball);
            }
        }
        
        private Vector2Int? FindBallIndex(BallView targetBall)
        {
            for (var i = 0; i < _pinnedBalls.GetLength(0); i++)
            {
                for (var j = 0; j < _pinnedBalls.GetLength(1); j++)
                {
                    if (_pinnedBalls[i, j] == targetBall)
                    {
                        return new(i, j);
                    }
                }
            }
            
            return null;
        }
    }
}