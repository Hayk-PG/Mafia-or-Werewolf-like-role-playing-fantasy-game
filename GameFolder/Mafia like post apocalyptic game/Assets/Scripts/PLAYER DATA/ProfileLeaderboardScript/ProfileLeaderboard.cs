using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileLeaderboard : MonoBehaviour
{
    [Serializable] public class UI
    {
        [SerializeField] Text positionText;
        [SerializeField] Text nameText;
        [SerializeField] Text scoreText;

        public string Position
        {
            get => positionText.text;
            set => positionText.text = value;
        }
        public string Name
        {
            get => nameText.text;
            set => nameText.text = value;
        }
        public string Score
        {
            get => scoreText.text;
            set => scoreText.text = value;
        }
    }
    public UI _UI;
}
