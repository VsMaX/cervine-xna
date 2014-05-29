using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Cervine.Content;
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
        public int Level { get; set; }
        public double TimeSinceLastRespawn { get; set; }
        public double TimeToRespawn { get; set; }
        public Tnt Tnt { get; set; }
        private double _gameTime;

        public UserControlledSprite Player
        {
            get { return Drawables.FirstOrDefault(x => x is UserControlledSprite) as UserControlledSprite; }
        }

        public GameBoard(Point boardSize, float frameWidth, float frameHeight, float menuBarHeight, SpriteManager spriteManager, SpriteFont font, 
            Texture2D bombFire, Texture2D normalEnemyTexture, Texture2D hunterEnemyTexture, Texture2D shooterEnemyTexture,
            Texture2D tankEnemyTexture, Texture2D foodTexture, Texture2D medpackTexture, Texture2D tntDetonatorTexture,
            Texture2D chargingTexture, Texture2D tntTexture, Texture2D bombTexture2D, Texture2D debrisTexture1, Texture2D debrisTexture2, Texture2D debrisTexture3,
            Texture2D debrisTexture4, Texture2D destroyableWallTexture)
        {
            _gameTime = 0;
            _bombFire = bombFire;
            _normalEnemyTexture = normalEnemyTexture;
            _hunterEnemyTexture = hunterEnemyTexture;
            _shooterEnemyTexture = shooterEnemyTexture;
            _tankEnemyTexture = tankEnemyTexture;
            _foodTexture = foodTexture;
            _medpackTexture = medpackTexture;
            _tntDetonatorTexture = tntDetonatorTexture;
            _chargingTexture = chargingTexture;
            _tntTexture = tntTexture;
            _bombTexture2D = bombTexture2D;
            _debrisTexture1 = debrisTexture1;
            _debrisTexture2 = debrisTexture2;
            _debrisTexture3 = debrisTexture3;
            _debrisTexture4 = debrisTexture4;
            _destroyableWallTexture = destroyableWallTexture;
            Fields = new Field[boardSize.X, boardSize.Y];
            Drawables = new List<Sprite>();
            for (int i = 0; i < boardSize.X; i++)
            {
                for (int j = 0; j < boardSize.Y; j++)
                {
                    Fields[i,j] = new Field(i,j);
                }
            }
            SizeX = boardSize.X;
            SizeY = boardSize.Y;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            MenuBarHeight = menuBarHeight;
            _spriteManager = spriteManager;
            this.font = font;
            Bombs = new List<Bomb>();
            Level = 4;
            TimeToRespawn = 200000;
            TimeToPowerUp = 5000;
            TimeToDestroyableWall = 7000;

            AddObject(new NormalEnemySprite(_normalEnemyTexture,
                new Point(19, 14), this));

            AddObject(new HunterEnemySprite(_hunterEnemyTexture,
                new Point(5, 0), this));
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
            Fields[spritePosition.X, spritePosition.Y].Sprite = null;
            if(spriteOnBoard == null || !Drawables.Remove(spriteOnBoard))
                throw new Exception("No such sprite on board");
        }

        public bool IsPositionEmpty(Point newPosition)
        {
            return Fields[newPosition.X, newPosition.Y].Sprite == null &&
                Fields[newPosition.X, newPosition.Y].Bomb == null;
        }

        public bool IsPositionValid(Point newPosition)
        {
            return IsPositionOnBoard(newPosition) && IsPositionEmpty(newPosition);
        }

        private bool IsPositionOnBoard(Point newPosition)
        {
            return newPosition.X >= 0 && newPosition.X < SizeX && newPosition.Y >= 0 && newPosition.Y < SizeY;
        }

        public bool IsPositionValidExceptPlayer(Point position)
        {
            if (!IsPositionOnBoard(position))
                return false;
            return IsPositionEmpty(position) || Fields[position.X, position.Y].Sprite == Player;
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
                if (drawable is UserControlledSprite)
                {
                    var player = drawable as UserControlledSprite;
                    if (player.PowerUp is ChargingPowerUp)
                    {
                        spriteBatch.Draw(player.YellowTextureImage, new Vector2(FrameWidth * position.X, FrameHeight * position.Y + MenuBarHeight)
                            , Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(player.TextureImage, new Vector2(FrameWidth * position.X, FrameHeight * position.Y + MenuBarHeight)
                            , Color.White);
                    }
                }
                else
                {
                    spriteBatch.Draw(drawable.TextureImage, new Vector2(FrameWidth * position.X, FrameHeight * position.Y + MenuBarHeight)
                        ,Color.White);
                }
            }

            for (int i = Bombs.Count - 1; i >= 0; i--)
            {
                var bomb = Bombs[i];
                spriteBatch.Draw(bomb.TextureImage, new Vector2(FrameWidth * bomb.Position.X, FrameHeight * bomb.Position.Y + MenuBarHeight), Color.White);
                if (bomb.TimeTick < 40)
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
                spriteBatch.Draw(Player.LifeTextureImage, new Vector2(50 * i + 600, 10), Color.White);
            }
            //draw hunger bar
            for (int i = 0; i < Player.Hunger; i++)
            {
                spriteBatch.Draw(Player.HungerTextureImage, new Vector2(i + 600, 45), Color.White);
            }
            //powerup
            if (Player.HasPowerUp)
            {
                spriteBatch.Draw(Player.PowerUp.TextureImage, new Vector2(340, 5), Color.White);
            }

            spriteBatch.DrawString(font, ((int)(_gameTime/1000)).ToString("0000"), new Vector2(130, 10), Color.White);
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
                drawable.Update(gameTime);
                if (drawable.Life <= 0)
                {
                    Drawables.RemoveAt(i);
                    if (Fields[drawable.Position.X, drawable.Position.Y].Sprite == drawable)
                        Fields[drawable.Position.X, drawable.Position.Y].Sprite = null;
                    if (drawable is DestroyableWallSprite)
                    {
                        var debris = new DebrisPowerUp(_debrisTexture1, _debrisTexture2, _debrisTexture3, _debrisTexture4, drawable.Position, this);
                        AddObject(debris);
                    }
                }
            }

            for (int i = Bombs.Count - 1; i >= 0; i--)
            {
                var bomb = Bombs[i];
                bomb.DecreaseTime();
                if (bomb.IsReadyToDetonate)
                {
                    bomb.Detonate();
                }
            }

            if (TimeSinceLastPowerUp >= TimeToPowerUp * Level)
            {
                TimeSinceLastPowerUp = 0;
                var powerUp = GeneratePowerUp();
                AddObject(powerUp);
            }
            else
            {
                TimeSinceLastPowerUp += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (TimeSinceLastRespawn >= TimeToRespawn/Level)
            {
                var monster = GenerateEnemy(Level);
                AddObject(monster);
                TimeSinceLastRespawn = 0;
            }
            else
            {
                TimeSinceLastRespawn += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (TimeSinceLastDestroyableWall >= TimeToDestroyableWall)
            {
                var destroyableWall = GenerateDestroyableWall();
                AddObject(destroyableWall);
                TimeSinceLastDestroyableWall = 0;
            }
            else
            {
                TimeSinceLastDestroyableWall += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            _gameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private Sprite GenerateDestroyableWall()
        {
            var position = GenerateNewPosition();
            while (!IsPositionValid(position) && !IsPositionEmpty(position) && Math.Abs(position.X - Player.Position.X) < 3 && Math.Abs(position.Y - Player.Position.Y) < 3)
                position = GenerateNewPosition();

            var wall = new DestroyableWallSprite(_destroyableWallTexture, position, this);
            return wall;
        }

        public double TimeToDestroyableWall { get; set; }

        public double TimeSinceLastDestroyableWall { get; set; }

        private PowerUp GeneratePowerUp()
        {
            Random r = new Random();
            int powerupid = r.Next(4);
            PowerUp powerUp = null;
            var position = GenerateNewPosition();
            while (!IsPositionValid(position) && !IsPositionEmpty(position))
                position = GenerateNewPosition();
            switch (powerupid)
            {
                case 0: //Food
                    powerUp = new FoodPowerUp(_foodTexture, position, this);
                    break;
                case 1: //Medpack
                    powerUp = new MedpackPowerUp(_medpackTexture, position, this);
                    break;
                case 2: //Tnt + detonator
                    powerUp = new TntDetonatorPowerUp(_tntDetonatorTexture, position, this, _tntTexture);
                    break;
                case 3: //Charging up
                    powerUp = new ChargingPowerUp(_chargingTexture, position, this);
                    break;
            }
            return powerUp;
        }

        public double TimeSinceLastPowerUp { get; set; }
        public double TimeToPowerUp { get; set; }

        private EnemySprite GenerateEnemy(int level)
        {
            var position = GenerateNewPosition();
            while (!IsPositionValid(position) && !IsPositionEmpty(position) && Math.Abs(position.X - Player.Position.X) < 3 && Math.Abs(position.Y - Player.Position.Y) < 3)
                position = GenerateNewPosition();

            Random r = new Random();
            int randomMonster = (r.Next()%(level + 1)) % 4;
            EnemySprite monster = null;
            switch (randomMonster)
            {
                case 0:
                    monster = new NormalEnemySprite(_normalEnemyTexture, position, this);
                    break;
                case 1:
                    monster = new HunterEnemySprite(_hunterEnemyTexture, position, this);
                    break;
                case 2:
                    monster = new ShooterMonsterEnemySprite(_shooterEnemyTexture, position, this);
                    break;
                case 3:
                    monster = new TankMonsterEnemySprite(_tankEnemyTexture, position, this);
                    break;
            }
            if(monster == null)
                throw new ArgumentException("Could not generate monster");
            return monster;
        }

        private Point GenerateNewPosition()
        {
            Random r = new Random();
            int x = r.Next(SizeX);
            int y = r.Next(SizeY);
            return new Point(x,y);
        }

        public Sprite GetSprite(Point position)
        {
            return Fields[position.X, position.Y].Sprite;
        }
        
        public void GameOver()
        {
            _spriteManager.GameOver((int)(_gameTime/1000));
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

        private Texture2D _bombTexture2D;
        private Texture2D _bombFire;
        private Texture2D _normalEnemyTexture;
        private Texture2D _hunterEnemyTexture;
        private Texture2D _shooterEnemyTexture;
        private Texture2D _tankEnemyTexture;
        private Texture2D _foodTexture;
        private Texture2D _medpackTexture;
        private Texture2D _tntDetonatorTexture;
        private Texture2D _chargingTexture;
        private Texture2D _tntTexture;
        private SpriteManager _spriteManager;
        private Texture2D _debrisTexture1;
        private Texture2D _debrisTexture2;
        private Texture2D _debrisTexture3;
        private Texture2D _debrisTexture4;
        private Texture2D _destroyableWallTexture;

        public void PlantBomb(Point position)
        {
            
        }

        public List<Field> GetAdjacentPositionsForAStar(Field position)
        {
            var list = new List<Field>();
            var pos1 = new Point(position.X + 1, position.Y);
            var pos2 = new Point(position.X - 1, position.Y);
            var pos3 = new Point(position.X, position.Y + 1);
            var pos4 = new Point(position.X, position.Y - 1);
            if(IsPositionValidExceptPlayer(pos1))
                list.Add(Fields[pos1.X, pos1.Y]);
            if (IsPositionValidExceptPlayer(pos2))
                list.Add(Fields[pos2.X, pos2.Y]);
            if (IsPositionValidExceptPlayer(pos3))
                list.Add(Fields[pos3.X, pos3.Y]);
            if (IsPositionValidExceptPlayer(pos4))
                list.Add(Fields[pos4.X, pos4.Y]);
            return list;
        }

        public void Plant(Bomb bomb)
        {
            if (Bombs.FirstOrDefault(x => x.Position.X == bomb.Position.X && x.Position.Y == bomb.Position.Y) == null)
            {
                Fields[bomb.Position.X, bomb.Position.Y].Bomb = bomb;
                Bombs.Add(bomb);
            }
        }
    }

    public class ChargingPowerUp : PowerUp
    {
        public ChargingPowerUp(Texture2D texture2D, Point position, GameBoard board) : base(texture2D, position, board)
        {
            this.Life = 15000;
        }
    }

    public class TntDetonatorPowerUp : PowerUp
    {
        private Texture2D _tntTexture;

        public TntDetonatorPowerUp(Texture2D texture2D, Point position, GameBoard board, 
            Texture2D tntTexture) : base(texture2D, position, board)
        {
            TextureImage = texture2D;
            _tntTexture = tntTexture;
        }

        public bool IsPlanted { get; set; }

        public void Detonate()
        {
            Tnt.IsTriggered = true;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void Plant(Point position)
        {
            if (board.IsPositionValidExceptPlayer(position))
            {
                this.Position = position;
                var tnt = new Tnt(_tntTexture, position, board);
                IsPlanted = true;
                this.Tnt = tnt;
                board.Plant(tnt);
            }
        }

        public Tnt Tnt { get; set; }
    }

    public class MedpackPowerUp : PowerUp
    {
        public MedpackPowerUp(Texture2D texture2D, Point position, GameBoard board) : base(texture2D, position, board)
        {
        }
    }

    public class FoodPowerUp : PowerUp
    {
        public FoodPowerUp(Texture2D texture2D, Point position, GameBoard board) : base(texture2D, position, board)
        {
        }
    }

    public class PowerUp : Sprite
    {
        public PowerUp(Texture2D texture2D, Point position, GameBoard board) : base(texture2D, position, board)
        {
        }

        public override Point direction
        {
            get { throw new NotImplementedException(); }
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}