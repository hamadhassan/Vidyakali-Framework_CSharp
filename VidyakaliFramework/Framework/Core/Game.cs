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
        int gravity;
        int reducePlayerHealth;
        int reduceEnemyHealth;
        Keys keyCodeValue;
        float scoreIncrementValue;
        ProgressBar life;
        bool gameStatus;
        string nextLevelStatus=NextLevel.level1.ToString();

        List<GoPictureBox> goPictureBoxList;
        List<GoProgressBar> goProgressBarList;
        List<CollisionClass> collisionList;

        List<ObjectType> objectTypeList;
        List<ChasingClass> chasingList;

        public event EventHandler OnPictureBoxAdded;
        public event EventHandler onProgressBarAdded;
        public event EventHandler OnPictureBoxRemoved;
        public event EventHandler onProgressBarRemoved;
        


        GoPictureBox goPictureBox;
        GoProgressBar goProgressBar;

        private int pBarOffsetLeft;
        private int pBarOffsetTop;

        public List<GoPictureBox> GoPictureBoxList { get => goPictureBoxList; set => goPictureBoxList = value; }
        public List<GoProgressBar> GoProgressBarList { get => goProgressBarList; set => goProgressBarList = value; }
        public List<CollisionClass> CollisionList { get => collisionList; set => collisionList = value; }

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
        }
        #region Events
        public void riseFireCreateEvent(PictureBox box)
        {

        }
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
        public void removeAllObjects()
        {
           
        }
        #endregion

        #region Update Game by tick event 
        public string update(Label score,ProgressBar life)
        {
            if (gameStatus == true)
            {
                score.Text = Score.GamePoint.ToString();
                this.life = life;
                moveSmartly();
                detectCollsionwithPlayer();
                detectCollsionwithEnemy();
                detectCollisionOfEnergyPoint();
                removeEnergyPoint();
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
                            if (goPictureBoxList[j]!=null && goPictureBoxList[i] != null)
                            {
                                //if(goPictureBoxList[i].Movement != null)
                                //{
                                    if (goPictureBoxList[i].Otype == goProgressBarList[j].Otype && objectTypeList[k] == goProgressBarList[j].Otype)
                                    {
                                        goPictureBoxList[i].updateLocation(this.gravity);
                                        goProgressBarList[j].updateLocation(goPictureBoxList[i].Pbx.Location.X + pBarOffsetLeft, goPictureBoxList[i].Pbx.Location.Y + pBarOffsetTop);
                                    }
                                //}
                            }
                        }
                    }
                }
            }
            return nextLevelStatus;

        }
        #endregion

        #region Movement
        public void keyPressedForFire(Keys keycode)
        {
            foreach (GoPictureBox go in goPictureBoxList)
            {
                if (go != null)
                {
                    if (go.Movement != null)
                    {
                        if (go.Otype == ObjectType.bullet)
                        {
                            if (go.Movement.GetType() == typeof(Fire))
                            {
                                Fire keyboardHandler = (Fire)go.Movement;
                                keyboardHandler.keyPressedByUserForFire(keycode,go.Pbx);
                            }
                        }
                    }
                }
            }
        }
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
                                                    goProgressBarList[z].Pbar.Value -= reducePlayerHealth;
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
