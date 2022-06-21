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

            goPictureBoxList = new List<GoPictureBox>();
            collisionList = new List<CollisionClass>();
            goProgressBarList = new List<GoProgressBar>();
            objectTypeList = new List<ObjectType>();
            chasingList = new List<ChasingClass>();
            goFirePictureBoxeList = new List<GoFirePictureBox>();
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
            goPictureBoxList.Add(goPictureBox);
            OnPictureBoxAdded?.Invoke(goPictureBox.Pbx, EventArgs.Empty);
        }
        public void addGameObjectPictureBoxWithoutMovement(ObjectType otype, Image img, int left, int top)
        {
            goPictureBox = new GoPictureBox(otype, img, left, top);
            goPictureBoxList.Add(goPictureBox);
            OnPictureBoxAdded?.Invoke(goPictureBox.Pbx, EventArgs.Empty);
        }
        public void addGameObjectProgressBar(ObjectType otype, int value, int leftSize, int topSize, int offsetLeft, int offsetTop)
        {
            goProgressBar = new GoProgressBar(otype,value,leftSize,topSize);
            pBarOffsetLeft = offsetLeft;
            pBarOffsetTop = offsetTop;
            objectTypeList.Add(otype);
            goProgressBarList.Add(goProgressBar);
            onProgressBarAdded?.Invoke(goProgressBar.Pbar, EventArgs.Empty);
        }
        public void addGameObjectPictureBoxFire(Image img, string direction, int speed, ObjectType fireFrom,int offsetLeft, int offsetTop)
        {
            for (int i = 0; i < goPictureBoxList.Count; i++)
            {
                if (goPictureBoxList[i].Otype == fireFrom)
                {
                    playerLeft = goPictureBoxList[i].Pbx.Left;
                    playerTop = goPictureBoxList[i].Pbx.Top;
                }
            }
            goFirePictureBox = new GoFirePictureBox(img, direction, speed, playerLeft+offsetLeft, playerTop+offsetTop);
            goFirePictureBoxeList.Add(goFirePictureBox);
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
                foreach (GoPictureBox go in goPictureBoxList)
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
                foreach (GoFirePictureBox go in goFirePictureBoxeList)
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
            foreach (GoPictureBox go in goPictureBoxList)
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
                                //if (keyCode != Keys.Left && keyCode != Keys.Right)
                                //{
                                //    if (ObjectType.player == go.Otype)
                                //    {
                                //        goPictureBox.updateImage(idel);
                                //    }
                                //}
                                //if (keyCode == Keys.Left)
                                //{
                                //    if (ObjectType.player == go.Otype)
                                //    {
                                //        goPictureBox.updateImage(moveLeft);
                                //        keyCode = Keys.Clear;
                                //    }
                                //}
                                //else if (keyCode == Keys.Right)
                                //{
                                //    if (ObjectType.player == go.Otype)
                                //    {
                                //        goPictureBox.updateImage(moveRigth);
                                //        keyCode = Keys.Clear;
                                //    }

                                //}

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
            for (int x = 0; x < goPictureBoxList.Count; x++)
            {
                for (int y = 0; y < goPictureBoxList.Count; y++)
                {
                    if (goPictureBoxList[y] != null&& goPictureBoxList[x]!=null)
                    {
                        if (goPictureBoxList[x].Pbx.Bounds.IntersectsWith(goPictureBoxList[y].Pbx.Bounds))
                        {
                            foreach (CollisionClass c in collisionList)
                            {
                                if(c != null)
                                {
                                    for (int z = 0; z < goProgressBarList.Count; z++)
                                    {
                                        if (goProgressBarList[z] != null)
                                        {
                                            if (goPictureBoxList[x].Otype == c.G1 && goPictureBoxList[y].Otype == c.G2 && goProgressBarList[z].Otype == c.G1 && goProgressBarList[z].Otype == ObjectType.player)
                                            {
                                                if (goProgressBarList[z].Pbar.Value > 0)
                                                {
                                                    goProgressBarList[z].Pbar.Value -=reducePlayerHealth;
                                                    if (goProgressBarList[z].Pbar.Value <= 2)
                                                    {
                                                        life.Value -= 20;
                                                        goProgressBarList[z].Pbar.Value = 100;
                                                    }
                                                }
                                                if (life.Value <= 1)
                                                {
                                                    c.Behaviour.removePictureBoxObject(this, goPictureBoxList[x], goPictureBoxList[y]);
                                                    c.Behaviour.removeProgressBarObject(this, goProgressBarList[z]);
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
            for (int x = 0; x < goPictureBoxList.Count; x++)
            {
                for (int y = 0; y < goPictureBoxList.Count; y++)
                {
                    if (goPictureBoxList[y] != null&& goPictureBoxList[x] != null)
                    {
                        if (goPictureBoxList[x].Pbx.Bounds.IntersectsWith(goPictureBoxList[y].Pbx.Bounds))
                        {
                            foreach (CollisionClass c in collisionList)
                            {
                                for (int z = 0; z < goProgressBarList.Count; z++)
                                {
                                    if(c!=null && goProgressBarList[z] != null)
                                    {
                                        if (goPictureBoxList[x].Otype == c.G1 && goPictureBoxList[y].Otype == c.G2 && goProgressBarList[z].Otype == c.G1 && (goProgressBarList[z].Otype == ObjectType.enemyIdel || goProgressBarList[z].Otype == ObjectType.enemyRun) && (keyCodeValue == Keys.Z || keyCodeValue == Keys.X))
                                        {
                                            if (goProgressBarList[z].Pbar.Value > 0)
                                            {
                                                goProgressBarList[z].Pbar.Value -= reduceEnemyHealth;
                                                Score.updateScore(scoreIncrementValue);
                                            }
                                            if (goProgressBarList[z].Pbar.Value <= 1)
                                            {
                                                c.Behaviour.removePictureBoxObject(this, goPictureBoxList[x], goPictureBoxList[y]);
                                                c.Behaviour.removeProgressBarObject(this, goProgressBarList[z]);
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

        
        public void addCollsionIntoList(CollisionClass c)
        {
            collisionList.Add(c);
        }
        #endregion

        #region Firing
        private void detectPlayerFirCollisionwithEnemy()
        {
            foreach(GoProgressBar health in goProgressBarList)
            {
                foreach (GoPictureBox enemy in goPictureBoxList)
                {
                    foreach (GoFirePictureBox bullet in goFirePictureBoxeList)
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
        private void removeEnemy()
        {
            foreach (GoPictureBox enemy in goPictureBoxList)
            {
                if (enemy.Pbx.Visible == false)
                {
                    OnPictureBoxRemoved?.Invoke(enemy.Pbx, EventArgs.Empty);
                }
            }
        }
        private void removeEnemyHealth()
        {
            foreach (GoProgressBar health in goProgressBarList)
            {
                if (health.Pbar.Visible == false)
                {
                    onProgressBarRemoved?.Invoke(health.Pbar, EventArgs.Empty);
                }
            }
        }
        private void removeBullet()
        {
            for (int i = 0; i < goFirePictureBoxeList.Count; i++)
            {
                if (goFirePictureBoxeList[i].Pbx.Right <= 0)
                {//right
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }
                else if (goFirePictureBoxeList[i].Pbx.Left <= 0)
                {//left
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }
                else if (goFirePictureBoxeList[i].Pbx.Top <= 0)
                {//up
                    OnPictureBoxRemoved?.Invoke(GoFirePictureBoxeList[i].Pbx, EventArgs.Empty);
                    GoFirePictureBoxeList.RemoveAt(i);
                }
                else if (goFirePictureBoxeList[i].Pbx.Bottom <= 0)
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
            for (int x = 0; x < goPictureBoxList.Count; x++)
            {
                for (int y = 0; y < goPictureBoxList.Count; y++)
                {
                    if (goPictureBoxList[y] != null && goPictureBoxList[x]!=null)
                    {
                        foreach (ChasingClass c in chasingList)
                        {
                            if (goPictureBoxList[x].Otype == c.G1 && goPictureBoxList[y].Otype == c.G2)
                            {
                                c.Behaviour.performChasing(this, goPictureBoxList[x], goPictureBoxList[y]);

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
                for (int i = 0; i < goPictureBoxList.Count; i++)
                {
                    for (int j = 0; j < goPictureBoxList.Count; j++)
                    {
                        if (goPictureBoxList[i] != null&&goPictureBoxList[j] != null)
                        {
                            if (goPictureBoxList[i].Otype == ObjectType.player && goPictureBoxList[j].Otype == ObjectType.energy)
                            {
                                if (goPictureBoxList[j].Pbx.Bounds.IntersectsWith(goPictureBoxList[i].Pbx.Bounds))
                                {
                                    goPictureBoxList[j].Pbx.Visible = false;
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
                for (int x = 0; x < goPictureBoxList.Count; x++)
                {
                    if (goPictureBoxList[x] != null)
                    {
                        if (goPictureBoxList[x].Pbx.Visible == false)
                        {
                            risePlayerDieEvent(goPictureBoxList[x].Pbx);
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
