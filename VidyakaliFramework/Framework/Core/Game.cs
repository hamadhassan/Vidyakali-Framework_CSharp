using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Framework.Movement;
using Framework.Collision;
using Framework.Core;
using Framework.Chasing;
using Framework.GameScore;

namespace Framework.Core
{
    public class Game : IGame
    {
        #region Data Set and Load Event 
        private int gravity;
        private int reducePlayerHealth;
        private int reduceEnemyHealth;
        private Keys keyCodeValue;
        private float scoreIncrementValue;
        private ProgressBar life;
        private bool gameStatus;
        private string nextLevelStatus=NextLevel.level1.ToString();
        private int enemyDieCount;

        private List<GoPictureBox> goPictureBoxList;
        private List<GoProgressBar> goProgressBarList;
        private List<CollisionClass> collisionList;
        private List<GoFirePictureBox> goFirePictureBoxeList;

        private List<ObjectType> objectTypeList;
        private List<ChasingClass> chasingList;

        public event EventHandler OnPictureBoxAdded;
        public event EventHandler onProgressBarAdded;
        public event EventHandler OnPictureBoxRemoved;
        public event EventHandler onProgressBarRemoved;
        


        private GoPictureBox goPictureBox;
        private GoProgressBar goProgressBar;
        private GoFirePictureBox goFirePictureBox;

        private int pBarOffsetLeft;
        private int pBarOffsetTop;

        private int playerLeft;
        private int playerTop;

        private int wonAt;

        public List<GoPictureBox> GoPictureBoxList { get => goPictureBoxList; set => goPictureBoxList = value; }
        public List<GoProgressBar> GoProgressBarList { get => goProgressBarList; set => goProgressBarList = value; }
        public List<CollisionClass> CollisionList { get => collisionList; set => collisionList = value; }
        public List<GoFirePictureBox> GoFirePictureBoxeList { get => goFirePictureBoxeList; set => goFirePictureBoxeList = value; }

        public Game(int gravity,int reducePlayerHealth,int reduceEnemyHealth,float scoreIncrementValue,bool gameStatus)
        {
            this.gravity = gravity;
            this.reducePlayerHealth = reducePlayerHealth;
            this.reduceEnemyHealth = reduceEnemyHealth;
            this.scoreIncrementValue = scoreIncrementValue;
            this.gameStatus = gameStatus;

            GoPictureBoxList = new List<GoPictureBox>();
            CollisionList = new List<CollisionClass>();
            GoProgressBarList = new List<GoProgressBar>();
            objectTypeList = new List<ObjectType>();
            chasingList = new List<ChasingClass>();
            GoFirePictureBoxeList = new List<GoFirePictureBox>();
        }
        #endregion

        #region Events
        public void risePlayerDieEvent(PictureBox playerGameObject)
        {
            OnPictureBoxRemoved?.Invoke(playerGameObject, EventArgs.Empty);
        }
        public void riseEnemyDieEvent(PictureBox enemyGameObject)
        {
            OnPictureBoxRemoved?.Invoke(enemyGameObject, EventArgs.Empty);
        }
        public void risePlayerProgressBarDieEvent(ProgressBar playerGameObject)
        {
            onProgressBarRemoved?.Invoke(playerGameObject, EventArgs.Empty);
        }
        public void riseEnemyProgressBarDieEvent(ProgressBar playerGameObject)
        {
            onProgressBarRemoved?.Invoke(playerGameObject, EventArgs.Empty);
        }
        public void addGameObjectPictureBox(ObjectType otype, Image img, int left, int top, IMovement movement)
        {
            goPictureBox = new GoPictureBox(otype, img, left, top, movement);
            GoPictureBoxList.Add(goPictureBox);
            OnPictureBoxAdded?.Invoke(goPictureBox.Pbx, EventArgs.Empty);
        }
        public void addGameObjectPictureBoxWithoutMovement(ObjectType otype, Image img, int left, int top)
        {
            goPictureBox = new GoPictureBox(otype, img, left, top);
            GoPictureBoxList.Add(goPictureBox);
            OnPictureBoxAdded?.Invoke(goPictureBox.Pbx, EventArgs.Empty);
        }
        public void addGameObjectProgressBar(ObjectType otype, int value, int leftSize, int topSize, int offsetLeft, int offsetTop)
        {
            goProgressBar = new GoProgressBar(otype,value,leftSize,topSize);
            pBarOffsetLeft = offsetLeft;
            pBarOffsetTop = offsetTop;
            objectTypeList.Add(otype);
            GoProgressBarList.Add(goProgressBar);
            onProgressBarAdded?.Invoke(goProgressBar.Pbar, EventArgs.Empty);
        }
        public void addGameObjectPictureBoxFire(Image img, string direction, int speed, ObjectType fireFrom,int offsetLeft, int offsetTop)
        {
            for (int i = 0; i < GoPictureBoxList.Count; i++)
            {
                if (GoPictureBoxList[i].Otype == fireFrom)
                {
                    playerLeft = GoPictureBoxList[i].Pbx.Left;
                    playerTop = GoPictureBoxList[i].Pbx.Top;
                }
            }
            goFirePictureBox = new GoFirePictureBox(img, direction, speed, playerLeft+offsetLeft, playerTop+offsetTop);
            GoFirePictureBoxeList.Add(goFirePictureBox);
            OnPictureBoxAdded?.Invoke(goFirePictureBox.Pbx, EventArgs.Empty);
        }
        #endregion

        #region Update Game by tick event 
        public string update(Label score,ProgressBar life,int wonAt)
        {
            if (gameStatus == true)
            {
                this.wonAt = wonAt;
                score.Text = Score.GamePoint.ToString();
                this.life = life;
                moveSmartly();
                detectCollsionwithPlayer();
                detectCollsionwithEnemy();
                detectCollisionOfEnergyPoint();
                removeEnergyPoint();
                detectPlayerFirCollisionwithEnemy();
                removeEnemy();
                removeEnemyHealth();
                removeBullet();
                removeObjects();
                foreach (GoPictureBox go in GoPictureBoxList)
                {
                    if(go != null)
                    {
                        if (go.Movement != null)
                        {
                            go.updateLocation(this.gravity);
                        }
                    }
                }
                for (int i = 0; i < goPictureBoxList.Count; i++)
                {//only move the progress bar 
                    for (int j = 0; j < goProgressBarList.Count; j++)
                    {
                        for (int k = 0; k < objectTypeList.Count; k++)
                        {
                            if (goPictureBoxList[j] != null && goPictureBoxList[i] != null)
                            {
                                if (goPictureBoxList[i].Movement != null)
                                {
                                    if (goPictureBoxList[i].Otype == goProgressBarList[j].Otype && objectTypeList[k] == goProgressBarList[j].Otype)
                                    {
                                        goPictureBoxList[i].updateLocation(this.gravity);
                                        goProgressBarList[j].updateLocation(goPictureBoxList[i].Pbx.Location.X + pBarOffsetLeft, goPictureBoxList[i].Pbx.Location.Y + pBarOffsetTop);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (GoFirePictureBox go in GoFirePictureBoxeList)
                {
                    if (go != null)
                    {
                        go.fire();
                    }
                }
               

            }
            return nextLevelStatus;

        }
        #endregion

        #region Movement
       
        public void keyPressed(Keys keyCode, Image idel, Image moveLeft, Image moveRigth)
        {
            keyCodeValue = keyCode;
            foreach (GoPictureBox go in GoPictureBoxList)
            {
                if (go != null)
                {
                    if (go.Movement != null)
                    {
                        if (go.Otype == ObjectType.player)
                        {
                            if (go.Movement.GetType() == typeof(Keyboard) && ObjectType.player == go.Otype)
                            {
                                Keyboard keyboardHandler = (Keyboard)go.Movement;
                                keyboardHandler.keyPressedByUser(keyCode);
                            }
                        }
                    }
                }
            }
           
        }
        #endregion

        #region  Collision
       
        private void detectCollsionwithPlayer()
        {
            for (int x = 0; x < GoPictureBoxList.Count; x++)
            {
                for (int y = 0; y < GoPictureBoxList.Count; y++)
                {
                    if (GoPictureBoxList[y].Pbx != null&& GoPictureBoxList[x].Pbx!=null)
                    {
                        if (GoPictureBoxList[x].Pbx.Bounds.IntersectsWith(GoPictureBoxList[y].Pbx.Bounds))
                        {
                            foreach (CollisionClass c in CollisionList)
                            {
                                if(c != null)
                                {
                                    for (int z = 0; z < GoProgressBarList.Count; z++)
                                    {
                                        if (GoProgressBarList[z].Pbar != null)
                                        {
                                            if (GoPictureBoxList[x].Otype == c.G1 && GoPictureBoxList[y].Otype == c.G2 && GoProgressBarList[z].Otype == c.G1 && GoProgressBarList[z].Otype == ObjectType.player)
                                            {
                                                if (GoProgressBarList[z].Pbar.Value > 0)
                                                {
                                                    GoProgressBarList[z].Pbar.Value -=reducePlayerHealth;
                                                    if (GoProgressBarList[z].Pbar.Value <= 2)
                                                    {
                                                        life.Value -= 20;
                                                        GoProgressBarList[z].Pbar.Value = 100;
                                                    }
                                                }
                                                if (life.Value <= 1)
                                                {
                                                    c.Behaviour.removePictureBoxObject(this, GoPictureBoxList[x], GoPictureBoxList[y]);
                                                    c.Behaviour.removeProgressBarObject(this, GoProgressBarList[z]);
                                                    nextLevelStatus = NextLevel.lost.ToString();
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                   
                }
            }
        }

        private void detectCollsionwithEnemy()
        {
            for (int x = 0; x < GoPictureBoxList.Count; x++)
            {
                for (int y = 0; y < GoPictureBoxList.Count; y++)
                {
                    if (GoPictureBoxList[y].Pbx != null&& GoPictureBoxList[x].Pbx != null)
                    {
                        if (GoPictureBoxList[x].Pbx.Bounds.IntersectsWith(GoPictureBoxList[y].Pbx.Bounds))
                        {
                            foreach (CollisionClass c in CollisionList)
                            {
                                for (int z = 0; z < GoProgressBarList.Count; z++)
                                {
                                    if(c!=null && GoProgressBarList[z].Pbar != null)
                                    {
                                        if (GoPictureBoxList[x].Otype == c.G1 && GoPictureBoxList[y].Otype == c.G2 && GoProgressBarList[z].Otype == c.G1 && (GoProgressBarList[z].Otype == ObjectType.enemyIdel || GoProgressBarList[z].Otype == ObjectType.enemyRun) && (keyCodeValue == Keys.Z || keyCodeValue == Keys.X))
                                        {
                                            if (GoProgressBarList[z].Pbar.Value > 0)
                                            {
                                                GoProgressBarList[z].Pbar.Value -= reduceEnemyHealth;
                                                Score.updateScore(scoreIncrementValue);
                                            }
                                            if (GoProgressBarList[z].Pbar.Value <= 1)
                                            {
                                                c.Behaviour.removePictureBoxObject(this, GoPictureBoxList[x], GoPictureBoxList[y]);
                                                c.Behaviour.removeProgressBarObject(this, GoProgressBarList[z]);
                                                goPictureBoxList[x].Pbx.Visible = false;
                                                if (c.G1 == ObjectType.enemyIdel || c.G1 == ObjectType.enemyRun || c.G2 == ObjectType.enemyIdel || c.G2 == ObjectType.enemyRun)
                                                {
                                                    if (x > goPictureBoxList.Count)
                                                    {
                                                        goPictureBoxList[x].Pbx = null;
                                                    }
                                                }
                                                GoProgressBarList[x].Pbar.Visible = false;
                                                nextLevelStatus = NextLevel.won.ToString();
                                            }

                                        }
                                    }
                                   
                                }

                            }
                        }
                    }
                }
            }
        }
        private void removeObjects()
        {
            for(int i = 0; i < goPictureBoxList.Count; i++)
            {
                if (goPictureBoxList[i].Pbx.Visible == false)
                {
                    GoPictureBoxList.RemoveAt(i);
                }
            }
            for (int i = 0; i < goProgressBarList.Count; i++)
            {
                if (goProgressBarList[i].Pbar.Visible == false)
                {
                    goProgressBarList.RemoveAt(i);
                }
            }
        }
        
        public void addCollsionIntoList(CollisionClass c)
        {
            CollisionList.Add(c);
        }
        #endregion

        #region Firing
        private void detectPlayerFirCollisionwithEnemy()
        {
            detectPlayerFirCollisionwithEnemy1();
            foreach (GoProgressBar health in GoProgressBarList)
            {
                foreach (GoPictureBox enemy in GoPictureBoxList)
                {
                    foreach (GoFirePictureBox bullet in GoFirePictureBoxeList)
                    {
                        if (enemy.Otype == ObjectType.enemyIdel || enemy.Otype == ObjectType.enemyRun)
                        {
                            if (bullet.Pbx.Bounds.IntersectsWith(enemy.Pbx.Bounds))
                            {
                                enemy.Pbx.Visible = false;
                                health.Pbar.Visible = false;
                                Score.updateScore(scoreIncrementValue);
                                enemyDieCount++;
                                if (wonAt == enemyDieCount)
                                {
                                    nextLevelStatus = NextLevel.level3.ToString();
                                    gameStatus = false;
                                }

                            }
                        }

                    }
                }
            }
        }
        private void detectPlayerFirCollisionwithEnemy1()
        {
            foreach (GoPictureBox enemy in GoPictureBoxList)
            {
                foreach (GoFirePictureBox bullet in GoFirePictureBoxeList)
                {
                    if (enemy.Otype == ObjectType.enemyIdel || enemy.Otype == ObjectType.enemyRun)
                    {
                        if (bullet.Pbx.Bounds.IntersectsWith(enemy.Pbx.Bounds))
                        {
                            enemy.Pbx.Visible = false;
                            Score.updateScore(scoreIncrementValue);
                            enemyDieCount++;
                            if (wonAt == enemyDieCount)
                            {
                                nextLevelStatus = NextLevel.level3.ToString();
                                gameStatus = false;
                            }

                        }
                    }

                }
            }
        }
    
        private void removeEnemy()
        {
            foreach (GoPictureBox enemy in GoPictureBoxList)
            {
                if (enemy.Pbx != null)
                {
                    if (enemy.Pbx.Visible == false)
                    {
                        OnPictureBoxRemoved?.Invoke(enemy.Pbx, EventArgs.Empty);
                    }
                }
               
            }
        }
        private void removeEnemyHealth()
        {
            foreach (GoProgressBar health in GoProgressBarList)
            {
                if (health.Pbar != null)
                {
                    if (health.Pbar.Visible == false)
                    {
                        onProgressBarRemoved?.Invoke(health.Pbar, EventArgs.Empty);
                    }
                }
            }
        }
        private void removeBullet()
        {
            for (int i = 0; i < GoFirePictureBoxeList.Count; i++)
            {
                if (GoFirePictureBoxeList[i].Pbx.Right <= 0)
                {//right
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }
                else if (GoFirePictureBoxeList[i].Pbx.Left <= 0)
                {//left
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }
                else if (GoFirePictureBoxeList[i].Pbx.Top <= 0)
                {//up
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }
                else if (GoFirePictureBoxeList[i].Pbx.Bottom <= 0)
                {//down
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }

            }
        }
        #endregion

        #region Chasing
        public void addChasingIntoList(ChasingClass c)
        {
            chasingList.Add(c);
        }
        private void moveSmartly()
        {
            for (int x = 0; x < GoPictureBoxList.Count; x++)
            {
                for (int y = 0; y < GoPictureBoxList.Count; y++)
                {
                    if (GoPictureBoxList[y].Pbx != null && GoPictureBoxList[x].Pbx != null)
                    {
                        foreach (ChasingClass c in chasingList)
                        {
                            if (GoPictureBoxList[x].Otype == c.G1 && GoPictureBoxList[y].Otype == c.G2)
                            {
                                c.Behaviour.performChasing(this, GoPictureBoxList[x], GoPictureBoxList[y]);
                            }
                        }
                       
                    }
                }
            }
        }
        #endregion

        #region EnergyPoint
        private void detectCollisionOfEnergyPoint()
        {
            try
            {
                for (int i = 0; i < GoPictureBoxList.Count; i++)
                {
                    for (int j = 0; j < GoPictureBoxList.Count; j++)
                    {
                        if (GoPictureBoxList[i].Pbx != null&&GoPictureBoxList[j].Pbx != null)
                        {
                            if (GoPictureBoxList[i].Otype == ObjectType.player && GoPictureBoxList[j].Otype == ObjectType.energy)
                            {
                                if (GoPictureBoxList[j].Pbx.Bounds.IntersectsWith(GoPictureBoxList[i].Pbx.Bounds))
                                {
                                    GoPictureBoxList[j].Pbx.Visible = false;
                                    if (life.Value <= 80)
                                    {
                                        life.Value += 20;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void removeEnergyPoint()
        {
            try
            {
                for (int x = 0; x < GoPictureBoxList.Count; x++)
                {
                    if (GoPictureBoxList[x].Pbx != null)
                    {
                        if (GoPictureBoxList[x].Pbx.Visible == false)
                        {
                            risePlayerDieEvent(GoPictureBoxList[x].Pbx);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
