using Core;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ScoreService
    {
        [Inject] private AppSettings _appSettings;
        
        private int _currentScores;

        private const int ScoreByLine = 3; 
        
        public int CurrentScores => _currentScores;

        public void AddScoreByLine(Color color)
        {
            _currentScores += _appSettings.GetScoresByColor(color);
        }
    }
}