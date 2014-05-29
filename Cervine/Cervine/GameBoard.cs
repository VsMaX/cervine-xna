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
    /// <summary>
    /// GameBoard class that represents board for one game with all sprites and objects neccesary to run it
    /// </summary>
    [Serializable]
    public class GameBoard
    {
        /// <summary>
        /// Array of fields representing board
        /// </summary>
        public Field[,] Fields { get; set; }
        /// <summary>
        /// Width of board in cell count
        /// </summary>
        public int SizeY { get; private set; }
        /// <summary>
        /// Height of board in call count
        /// </summary>
        public int SizeX { get; private set; }
        /// <summary>
        /// Width of one cell in pixels
        /// </summary>
        public float FrameWidth { get; set; }
        /// <summary>
        /// Height of one cell in pixels
        /// </summary>
        public float FrameHeight { get; set; }
        /// <summary>
        /// Height of top menu bar in pixels
        /// </summary>
        public float MenuBarHeight { get; set; }
        /// <summary>
        /// List of all sprites that are drawn on board except bombs
        /// </summary>
        public List<Sprite> Drawables { get; set; }
        /// <summary>
        /// Font used in game
        /// </summary>
        public SpriteFont font;
        /// <summary>
        /// List of bombs that are currently on board
        /// </summary>
        public List<Bomb> Bombs { get; set; }
        /// <summary>
        /// Level of difficulty of the game, changes in time
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Time since last monster was spawned on board
        /// </summary>
        public double TimeSinceLastRespawn { get; set; }
        /// <summary>
        /// Time between two monster respawns
        /// </summary>
        public double TimeToRespawn { get; set; }
        /// <summary>
        /// Game time measured for score purposes
        /// </summary>
        private double _gameTime;
        /// <summary>
        /// Time to next levelup
        /// </summary>
        public int LevelUpTime { get; set; }
        /// <summary>
        /// Current player
        /// </summary>
        public UserControlledSprite Player
        {
            get { return Drawables.FirstOrDefault(x => x is UserControlledSprite) as UserControlledSprite; }
        }
        /// <summary>
        /// Constructor for GameBoard
        /// </summary>
        /// <param name="boardSize">Size of board</param>
        /// <param name="frameWidth">Width of single board cell in pixels</param>
        /// <param name="frameHeight">Width of single board cell in pixels</param>
        /// <param name="menuBarHeight">Height of top menu bar in pixels</param>
        /// <param name="spriteManager">Instance of SpriteManager class running game</param>
        /// <param name="font">Font used in game</param>
        /// <param name="bombFire">Bombfire texture</param>
        /// <param name="normalEnemyTexture">Normal enemy texture</param>
        /// <param name="hunterEnemyTexture">Hunter enemy texture</param>
        /// <param name="shooterEnemyTexture">Shooter enemy texture</param>
        /// <param name="tankEnemyTexture">Tank enemy texture</param>
        /// <param name="foodTexture">Food texture</param>
        /// <param name="medpackTexture">Medpack texture</param>
        /// <param name="tntDetonatorTexture">Tnt detonator texture</param>
        /// <param name="chargingTexture">Charging powerup texture</param>
        /// <param name="tntTexture">Tnt texture</param>
        /// <param name="bombTexture2D">Bomb texture</param>
        /// <param name="debrisTexture1">Debris texture when 1 in slot</param>
        /// <param name="debrisTexture2">Debris texture when 2 in slot</param>
        /// <param name="debrisTexture3">Debris texture when 3 in slot</param>
        /// <param name="debrisTexture4">Debris texture when 4 in slow</param>
        /// <param name="destroyableWallTexture">Destroyable wall texture</param>
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
            Level = 1;
            TimeToRespawn = 60000;
            TimeToPowerUp = 5000;
            TimeToDestroyableWall = 7000;
            LevelUpTime = 60000;

            AddObject(new NormalEnemySprite(_normalEnemyTexture,
                new Point(19, 14), this));

            AddObject(new HunterEnemySprite(_hunterEnemyTexture,
                new Point(5, 0), this));
        }
        /// <summary>
        /// Adds sprite to game and puts it on field on board
        /// </summary>
        /// <param name="sprite"></param>
        public void AddObject(Sprite sprite)
        {
            var spritePosition = sprite.GetPosition();
            if (IsPositionValid(spritePosition))
            {
                Drawables.Add(sprite);
                Fields[spritePosition.X, spritePosition.Y].Sprite = sprite;
            }
        }
        /// <summary>
        /// Removes sprite from game entirely
        /// </summary>
        /// <param name="sprite"></param>
        public void RemoveObject(Sprite sprite)
        {
            var spritePosition = sprite.GetPosition();
            var spriteOnBoard = Fields[spritePosition.X, spritePosition.Y].Sprite;
            Fields[spritePosition.X, spritePosition.Y].Sprite = null;
            if(spriteOnBoard == null || !Drawables.Remove(spriteOnBoard))
                throw new Exception("No such sprite on board");
        }
        /// <summary>
        /// Checks if position is empty on map
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns>True if position is empty, false otherwise</returns>
        public bool IsPositionEmpty(Point newPosition)
        {
            return Fields[newPosition.X, newPosition.Y].Sprite == null &&
                Fields[newPosition.X, newPosition.Y].Bomb == null;
        }
        /// <summary>
        /// Checks if position is within board width and height and if position is taken by another sprite
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns>True if position is on board and the field is empty, false otheriwse</returns>
        public bool IsPositionValid(Point newPosition)
        {
            return IsPositionOnBoard(newPosition) && IsPositionEmpty(newPosition);
        }
        /// <summary>
        /// Checks if position is within board width and height
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns>True if position is within board bounds, false otherwise</returns>
        private bool IsPositionOnBoard(Point newPosition)
        {
            return newPosition.X >= 0 && newPosition.X < SizeX && newPosition.Y >= 0 && newPosition.Y < SizeY;
        }
        /// <summary>
        /// Checks if position is valid and is taken by another sprite. 
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Returns false if there is no sprite or there is player, true otherwise</returns>
        public bool IsPositionValidExceptPlayer(Point position)
        {
            if (!IsPositionOnBoard(position))
                return false;
            return IsPositionEmpty(position) || Fields[position.X, position.Y].Sprite == Player;
        }
        /// <summary>
        /// Changes position of a sprite on board array
        /// </summary>
        /// <param name="oldPosition">Old position of sprite</param>
        /// <param name="sprite">Sprite with Position parameter set to new position</param>
        public void ChangePosition(Point oldPosition, Sprite sprite)
        {
            Fields[oldPosition.X, oldPosition.Y].Sprite = null;
            Fields[sprite.Position.X, sprite.Position.Y].Sprite = sprite;
        }
        /// <summary>
        /// Draws all objects on device screen
        /// </summary>
        /// <param name="gameTime">GameTime class updated every draw</param>
        /// <param name="spriteBatch">SpriteBatch used to draw on screen</param>
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
        /// <summary>
        /// Adjusts position to fit board width and height
        /// </summary>
        /// <param name="position">Position to fit</param>
        /// <returns>New position that is within board width and heigh</returns>
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
        /// <summary>
        /// Updates all sprites positions and calculates next moves. 
        /// Also takes care of bombs timeout and player powerups timeout.
        /// Generates and puts on board new enemies and new powerups.
        /// </summary>
        /// <param name="gameTime">GameTime used to track time in game</param>
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
            if (TimeSinceLastLevel >= LevelUpTime)
            {
                Level++;
                LevelUpTime = LevelUpTime*2;
                TimeSinceLastLevel = 0;
            }
            else
            {
                TimeSinceLastLevel += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            _gameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        /// <summary>
        /// Time since last levelup has occured
        /// </summary>
        public double TimeSinceLastLevel { get; set; }
        /// <summary>
        /// Generates DestroyableWallSprite that is ready to be placed on board.
        /// </summary>
        /// <returns></returns>
        private Sprite GenerateDestroyableWall()
        {
            var position = GenerateNewPosition();
            while (!IsPositionValid(position) && !IsPositionEmpty(position) && Math.Abs(position.X - Player.Position.X) < 3 && Math.Abs(position.Y - Player.Position.Y) < 3)
                position = GenerateNewPosition();

            var wall = new DestroyableWallSprite(_destroyableWallTexture, position, this);
            return wall;
        }
        /// <summary>
        /// Time between two DestroyableWallSprite objects are generated on map
        /// </summary>
        public double TimeToDestroyableWall { get; set; }
        /// <summary>
        /// Time since last DestroyableWallSprite was generated on map
        /// </summary>
        public double TimeSinceLastDestroyableWall { get; set; }
        /// <summary>
        /// Generates new powerup that is ready to be placed on board
        /// </summary>
        /// <returns>Random powerup</returns>
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
        /// <summary>
        /// Time since last powerup has been placed on board
        /// </summary>
        public double TimeSinceLastPowerUp { get; set; }
        /// <summary>
        /// Time between two powerups can show on board
        /// </summary>
        public double TimeToPowerUp { get; set; }
        /// <summary>
        /// Generates random enemy according to current level
        /// </summary>
        /// <param name="level">Current level of game</param>
        /// <returns>Random enemy ready to be placed on map</returns>
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
        /// <summary>
        /// Generates new random position that is within board bounds
        /// </summary>
        /// <returns></returns>
        private Point GenerateNewPosition()
        {
            Random r = new Random();
            int x = r.Next(SizeX);
            int y = r.Next(SizeY);
            return new Point(x,y);
        }
        /// <summary>
        /// Gets sprite that is placed on board
        /// </summary>
        /// <param name="position">Position of sprite to get</param>
        /// <returns>Sprite on asked position, null if there is no such sprite</returns>
        public Sprite GetSprite(Point position)
        {
            return Fields[position.X, position.Y].Sprite;
        }
        /// <summary>
        /// Finishes current game and records its high scores to high scores menu.
        /// </summary>
        public void GameOver()
        {
            _spriteManager.GameOver((int)(_gameTime/1000));
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
        /// <summary>
        /// Returns positions that are used by Astar algorithm to move sprite
        /// </summary>
        /// <param name="position">Current sprite position for which astar will be calculated</param>
        /// <returns>List of fields that sprite can move on</returns>
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
        /// <summary>
        /// Plants new bomb on map
        /// </summary>
        /// <param name="bomb"></param>
        public void Plant(Bomb bomb)
        {
            if (Bombs.FirstOrDefault(x => x.Position.X == bomb.Position.X && x.Position.Y == bomb.Position.Y) == null)
            {
                Fields[bomb.Position.X, bomb.Position.Y].Bomb = bomb;
                Bombs.Add(bomb);
            }
        }
    }
}