using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    public class ScoresMenu
    {
        private SpriteFont _font;
        private List<Score> _scoresList;
        private SpriteManager _spriteManager;

        public ScoresMenu(SpriteManager spriteManager, List<Score> scoresList, SpriteFont font)
        {
            this._font = font;
            this._scoresList = scoresList;
            _spriteManager = spriteManager;
        }

        public void GetHighScoresUserName(int score)
        {
            var scores = new Score();
            scores.Time = score;
            this.GetUserName = true;
            this.Score = scores;
            this._scoresList.Add(Score);
        }

        public Score Score { get; set; }
        public bool GetUserName { get; set; }

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

        public int Delay { get; set; }

        public bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

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
