using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    /// <summary>
    /// Scores menu responsible for getting user name and displaying top scores
    /// </summary>
    public class ScoresMenu
    {
        private SpriteFont _font;
        private List<Score> _scoresList;
        private SpriteManager _spriteManager;
        /// <summary>
        /// Ctor for ScoresMenu
        /// </summary>
        /// <param name="spriteManager">SpriteManager used to draw objects on device screen</param>
        /// <param name="scoresList">List of already taken scores</param>
        /// <param name="font">Font used to draw text</param>
        public ScoresMenu(SpriteManager spriteManager, List<Score> scoresList, SpriteFont font)
        {
            this._font = font;
            this._scoresList = scoresList;
            _spriteManager = spriteManager;
        }
        /// <summary>
        /// Gets high scores from last game and sets mode to take user input
        /// </summary>
        /// <param name="score">Score made by player</param>
        public void GetHighScoresUserName(int score)
        {
            var scores = new Score();
            scores.Time = score;
            this.GetUserName = true;
            this.Score = scores;
            this._scoresList.Add(Score);
        }

        /// <summary>
        /// Score made by last player
        /// </summary>
        public Score Score { get; set; }
        /// <summary>
        /// True if ScoresMenu is waiting for user name, false otherwise
        /// </summary>
        public bool GetUserName { get; set; }
        /// <summary>
        /// Handles all user input and updates user name in Scores class.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="clientBounds"></param>
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (Delay == 0)
            {
                var keyboard = Keyboard.GetState().GetPressedKeys();
                if (GetUserName)
                {
                    var key = keyboard.FirstOrDefault();
                    if (key == Keys.Enter)
                    {
                        GetUserName = false;
                    }
                    if (IsKeyAChar(key))
                    {
                        Score.Name += key.ToString();
                    }
                    else if (keyboard.Contains(Keys.Back))
                    {
                        Score.Name = Score.Name.Remove(Score.Name.Length - 1);
                    }
                }
                if (keyboard.FirstOrDefault() == Keys.Escape)
                {
                    _spriteManager.GameState = GameState.MainMenu;
                }
            }
            Delay = (Delay + 1) % 5;
        }
        /// <summary>
        /// Delay used to handle properly user input
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// Checks whether pressed key is a normal character
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if key is between A and Z, false otherwise</returns>
        public bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }
        /// <summary>
        /// Draws all scores on device screen
        /// </summary>
        /// <param name="gameTime">Instance of GameTime class</param>
        /// <param name="spriteBatch">Sprite batch used to draw objects on device screen</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "HIGH SCORES", new Vector2(200, 100), Color.White);

            var scores = _scoresList.OrderByDescending(x => x.Time).Take(5).ToList();
            for (int i = 0; i < scores.Count; i++)
            {
                var score = scores[i];
                if (score == Score && String.IsNullOrEmpty(score.Name))
                {
                    if (Delay < 10)
                    {
                        spriteBatch.DrawString(_font, "_", new Vector2(100, 150 + 50 * i), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(_font, " ", new Vector2(100, 150 + 50 * i), Color.White);
                    }
                }
                else
                {
                    spriteBatch.DrawString(_font, score.Name, new Vector2(100, 150 + 50 * i), Color.White);
                }
                spriteBatch.DrawString(_font, score.Time.ToString("0000"), new Vector2(300, 150 + 50 * i), Color.White);
            }
        }
    }
}
