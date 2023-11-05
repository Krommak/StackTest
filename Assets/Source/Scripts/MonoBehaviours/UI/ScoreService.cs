using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Game.MonoBehaviours.UI
{
    public class ScoreService : MonoBehaviour, IScoreService
    {
        [SerializeField]
        private TMP_Text _totalScoreText;
        [SerializeField]
        private TMP_Text _stackText;

        private Dictionary<ScoreType, int> _score;

        private void Start()
        {
            _score = new Dictionary<ScoreType, int>();
            _score.Add(ScoreType.Total, 0);
            _score.Add(ScoreType.Stack, 0);
            UpdateScore();
        }

        public void ClearScore(ScoreType type)
        {
            _score[type] = 0;
            UpdateScore();
        }

        public void DecrementScore(ScoreType type)
        {
            _score[type]--;
            UpdateScore();
        }

        public void IncrementScore(ScoreType type)
        {
            _score[type]++;
            UpdateScore();
        }

        private void UpdateScore()
        {
            _totalScoreText.text = $"Total score: {_score[ScoreType.Total]}";
            _stackText.text = $"Stack: {_score[ScoreType.Stack]}";
        }
    }

    public interface IScoreService
    {
        public abstract void IncrementScore(ScoreType type);

        public abstract void DecrementScore(ScoreType type);

        public abstract void ClearScore(ScoreType type);
    }

    public enum ScoreType
    {
        Total = 0,
        Stack = 1
    }
}