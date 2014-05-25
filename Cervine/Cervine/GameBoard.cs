using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class GameBoard
    {
        public Field[,] Fields { get; set; }
        public int SizeY { get; private set; }
        public int SizeX { get; private set; }
        public float FrameWidth { get; set; }
        public float FrameHeight { get; set; }
        public float MenuBarHeight { get; set; }
        public List<Sprite> Drawables { get; set; }
        private CervineGame _game;
        public event EventHandler GameOverEvent;
        public SpriteFont font;
        public List<Bomb> Bombs { get; set; }

        public UserControlledSprite Player
        {
            get { return Drawables.FirstOrDefault(x => x is UserControlledSprite) as UserControlledSprite; }
        }

        public GameBoard(Point boardSize, float frameWidth, float frameHeight, float menuBarHeight, CervineGame game, SpriteFont font, Texture2D bombFire)
        {
            _bombFire = bombFire;
            Fields = new Field[boardSize.X, boardSize.Y];
            Drawables = new List<Sprite>();
            for (int i = 0; i < boardSize.X; i++)
            {
                for (int j = 0; j < boardSize.Y; j++)
                {
                    Fields[i,j] = new Field();
                }
            }
            SizeX = boardSize.X;
            SizeY = boardSize.Y;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            MenuBarHeight = menuBarHeight;
            _game = game;
            this.font = font;
            Bombs = new List<Bomb>();
        }

        public void AddObject(Sprite sprite)
        {
            var spritePosition = sprite.GetPosition();
            if (IsPositionValid(spritePosition))
            {
                Drawables.Add(sprite);
                Fields[spritePosition.X, spritePosition.Y].Sprite = sprite;
            }
        }

        public void RemoveObject(Sprite sprite)
        {
            var spritePosition = sprite.GetPosition();
            var spriteOnBoard = Fields[spritePosition.X, spritePosition.Y].Sprite;
            if(spriteOnBoard == null || Drawables.Remove(spriteOnBoard))
                throw new Exception("No such sprite on board");
        }

        public bool IsPositionEmpty(Point newPosition)
        {
            return Fields[newPosition.X, newPosition.Y].Sprite == null;
        }

        public bool IsPositionValid(Point newPosition)
        {
            return IsPositionOnBoard(newPosition) && IsPositionEmpty(newPosition);
        }

        private bool IsPositionOnBoard(Point newPosition)
        {
            return newPosition.X >= 0 && newPosition.X < SizeX && newPosition.Y >= 0 && newPosition.Y < SizeY;
        }

        public void ChangePosition(Point oldPosition, Sprite sprite)
        {
            Fields[oldPosition.X, oldPosition.Y].Sprite = null;
            Fields[sprite.Position.X, sprite.Position.Y].Sprite = sprite;
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Drawables.Count; i++)
            {
                var drawable = Drawables[i];
                var position = drawable.Position;
                spriteBatch.Draw(drawable.TextureImage, new Vector2(FrameWidth * position.X, FrameHeight * position.Y + MenuBarHeight)
                    , Color.White);
            }

            for (int i = Bombs.Count - 1; i >= 0; i--)
            {
                var bomb = Bombs[i];
                spriteBatch.Draw(bomb.TextureImage, new Vector2(FrameWidth * bomb.Position.X, FrameHeight * bomb.Position.Y + MenuBarHeight), Color.White);
                if (bomb.TimeTick < 120)
                {
                    var positions = bomb.GetAffectedPositions();
                    foreach (var position in positions)
                    {
                        spriteBatch.Draw(_bombFire, new Vector2(FrameWidth * position.X, FrameHeight * position.Y + MenuBarHeight),
                            Color.White);
                    }
                }
            }

            //draw life
            for (int i = 0; i < Player.Life; i++)
            {
                spriteBatch.Draw(Player.LifeTextureImage, new Vector2(50 * i + 200, 10), Color.White);
            }
        }

        public Point AdjustToBoardSize(Point position)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X >= this.SizeX)
                position.X = this.SizeX - 1;
            if (position.Y >= this.SizeY)
                position.Y = this.SizeY - 1;
            return position;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = Drawables.Count - 1; i >= 0; i--)
            {
                var drawable = Drawables[i];
                if (drawable.Life <= 0)
                {
                    if (drawable is UserControlledSprite)
                    {
                        _game.GameState = GameState.GameOver;
                        return;
                    }
                    Fields[drawable.Position.X, drawable.Position.Y].Sprite = null;
                    Drawables.RemoveAt(i);
                }
                else
                {
                    drawable.Update(gameTime);
                }
            }

            for (int i = Bombs.Count - 1; i >= 0; i--)
            {
                var bomb = Bombs[i];
                bomb.TimeTick--;
                if (bomb.TimeTick <= 0)
                {
                    DestroyBomb(bomb);
                    Bombs.RemoveAt(i);
                }
            }
        }

        public Sprite GetSprite(Point position)
        {
            return Fields[position.X, position.Y].Sprite;
        }

        private void DestroyBomb(Bomb bomb)
        {
            var positions = bomb.GetAffectedPositions();
            foreach (var position in positions)
            {
                var sprite = Drawables.FirstOrDefault(z => z.Position == position);
                if (sprite != null)
                {
                    sprite.DecreaseLife();
                }
            }
        }
        
        public void OnGameOver()
        {
            if (GameOverEvent != null)
            {
                GameOverEvent(this, new EventArgs());
            }
        }

        public void ResetGame()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    Fields[i, j].Sprite = null;
                }
            }
            foreach (var drawable in Drawables)
            {
                drawable.Reset();
                Fields[drawable.Position.X, drawable.Position.Y].Sprite = drawable;
            }
        }

        private Texture2D bombTexture2D
        {
            get
            {
                if (_bombTexture2D == null)
                    _bombTexture2D = _game.Content.Load<Texture2D>(@"bomb");
                return _bombTexture2D;
            }
        }

        private Texture2D _bombTexture2D;
        private Texture2D _bombFire;

        public void PlantBomb(Point position)
        {
            if (Bombs.FirstOrDefault(x => x.Position.X == position.X && x.Position.Y == position.Y) == null)
            {
                var bomb = new Bomb(bombTexture2D, position, this);
                Fields[position.X, position.Y].Bomb = bomb;
                Bombs.Add(bomb);
            }
        }
    }
}