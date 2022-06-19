﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Framework.Core;
using Framework.Collision;
using Framework.Movement;
using Framework.Chasing;
using Framework.GameScore;


namespace Consumer
{
    public partial class frmMain : Form
    {
        ////Game initiliziation
        private Game game;
        ////Event handlers
        private Random random = new Random();
        private int energyTime;
        private bool isLevel1 = true;
        private bool isLevel2 = false;
        private bool isLevel3 = false;
        private PictureBox nextLevelBox;
        #region Load Event 
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            game.keyPressed(e.KeyCode, Consumer.Properties.Resources.idle, Consumer.Properties.Resources.runLeft, Consumer.Properties.Resources.runRight);
            game.keyPressedForFire(e.KeyCode);
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            createNextLevelBox();
            nextLevelBox.Visible = false;
            level1();
        }
        #endregion

        #region Open Next level
        private void detectCollsionwithBoxL2()
        {
            try
            {
                for (int i = 0; i < game.GoPictureBoxList.Count; i++)
                {
                    if (game.GoPictureBoxList[i] != null)
                    {
                        if (game.GoPictureBoxList[i].Otype == ObjectType.player)
                        {
                            if (game.GoPictureBoxList[i].Pbx.Bounds.IntersectsWith(nextLevelBox.Bounds))
                            {
                                if (isLevel2 == true)
                                {
                                    clearAllObjects();
                                    nextLevelBox.Visible = false;
                                    isLevel3 = true;
                                    //level2();
                                    MessageBox.Show("level 3");
                                    isLevel2 = false;
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
        private void detectCollsionwithBoxL1()
        {
            try
            {
                for (int i = 0; i < game.GoPictureBoxList.Count; i++)
                {
                    if (game.GoPictureBoxList[i] != null)
                    {
                        if (game.GoPictureBoxList[i].Otype == ObjectType.player)
                        {
                            if (game.GoPictureBoxList[i].Pbx.Bounds.IntersectsWith(nextLevelBox.Bounds))
                            {
                                if(isLevel1==true)
                                {
                                    clearAllObjects();
                                    nextLevelBox.Visible = false;
                                    isLevel2 = true;
                                    level2();
                                    isLevel1 = false;
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
        private void createNextLevelBox()
        {
            try
            {
                nextLevelBox = new PictureBox();
                nextLevelBox.Image = Consumer.Properties.Resources.boxOpen;
                nextLevelBox.SizeMode = PictureBoxSizeMode.AutoSize;
                nextLevelBox.Left = random.Next(25, 300);
                nextLevelBox.Top = random.Next(100, 300);
                this.Controls.Add(nextLevelBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Tick Event
        private int count;
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            showEnergyPoint();
            string message=game.update(lblScore, pgbarPlayerLife);
            if (message == NextLevel.won.ToString())
            {
                if (isLevel1==true)
                {
                    nextLevelBox.Visible = true;
                    detectCollsionwithBoxL1();
                }
                else if (isLevel2 == true)
                {
                    if (count >= 2)
                    {
                        nextLevelBox.Visible = true;
                        detectCollsionwithBoxL2();
                        count = 0;
                    }
                    count++;
                }
                else if (isLevel3 == true)
                {
                    isLevel3 = false;
                }
            }
            else if (message == NextLevel.lost.ToString())
            {
                gameLoop.Enabled = false;
                MessageBox.Show("lost");
            }
        }
        private void showEnergyPoint()
        {
            try
            {
                energyTime++;
                if (40 == energyTime)
                {
                    if (pgbarPlayerLife.Value <= random.Next(0, 90))
                    {
                        game.addGameObjectPictureBoxWithoutMovement(ObjectType.energy, Consumer.Properties.Resources.energy, random.Next(100, 300), random.Next(100, 400));
                    }
                    energyTime = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region Remove All Object
        public void clearAllObjects()
        {
            for (int i = 0; i < game.GoPictureBoxList.Count; i++)
            {
                this.Controls.RemoveAt(i);
                game.GoPictureBoxList[i] = null;
            }
            for (int i = 0; i < game.GoProgressBarList.Count; i++)
            {
                this.Controls.RemoveAt(i);
                game.GoProgressBarList[i] = null;
            }
            for (int i = 0; i < game.CollisionList.Count; i++)
            {
                game.CollisionList[i] = null;
            }
        }
        #endregion

        #region Level 1
        private void level1()
        {
            //Level 3
            ////Level Numebr
            lblLevel.Text = "3";
            ////Background Color
            this.BackColor = Color.Silver;
            ////Game initiliziation
            game = new Game(5, 1, 2, 1, true);
            ////Event handlers
            customEventHandler();
            ////Boundary of the form
            Point boundary = new Point(this.Width - 75, this.Height);
            ///Adding game object Picture Box
            game.addGameObjectPictureBox(ObjectType.bullet, Consumer.Properties.Resources.laserPlayerLeft, 100, 300, new Fire(20, boundary, 10, 10, DirectionType.right.ToString()));
            game.addGameObjectPictureBox(ObjectType.player, Consumer.Properties.Resources.idle, random.Next(100, 300), random.Next(100, 400), new Keyboard(20, boundary, 100));









            //////Level Numebr
            //lblLevel.Text = "1";
            //////Background Color
            //this.BackColor = Color.DarkOliveGreen;
            //////Game initiliziation
            //game = new Game(5, 1, 2, 1, true);
            //////Event handlers
            //customEventHandler();
            //////Boundary of the form
            //Point boundary = new Point(this.Width - 75, this.Height);
            ////Adding game object Picture Box
            //game.addGameObjectPictureBox(ObjectType.player, Consumer.Properties.Resources.idle, random.Next(100, 300), random.Next(100, 400), new Keyboard(20, boundary, 100));
            //game.addGameObjectPictureBox(ObjectType.enemyIdel, Consumer.Properties.Resources.enemyIdel, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            //////Adding game object Progress Bar
            //game.addGameObjectProgressBar(ObjectType.player, 100, 40, 15, 0, 80);
            //game.addGameObjectProgressBar(ObjectType.enemyIdel, 100, 40, 15, 0, 80);
            //////Chasing The player
            //ChasingClass chass = new ChasingClass(ObjectType.player, ObjectType.enemyIdel, new SmartChasing(3));
            //game.addChasingIntoList(chass);
            //////Adding collision
            //CollisionClass playerCollisionWithEnemy = new CollisionClass(ObjectType.player, ObjectType.enemyIdel, new PLayerCollision());
            //CollisionClass EnemyCollisionWithPlayer = new CollisionClass(ObjectType.enemyIdel, ObjectType.player, new EnemyCollision());
            //game.addCollsionIntoList(playerCollisionWithEnemy);
            //game.addCollsionIntoList(EnemyCollisionWithPlayer);
        }
        #endregion

        #region Level 2
        private void level2()
        {
            ////Level Numebr
            lblLevel.Text = "2";
            ////Background Color
            game = new Game(5, 1, 2, 1, true);
            this.BackColor = Color.DarkSlateGray;
            ////Event handlers
            customEventHandler();
            ////Boundary of the form
            Point boundary = new Point(this.Width - 75, this.Height);
            //Adding game object Picture Box
            game.addGameObjectPictureBox(ObjectType.player, Consumer.Properties.Resources.idle, random.Next(100, 300), random.Next(100, 400), new Keyboard(20, boundary, 100));
            game.addGameObjectPictureBox(ObjectType.enemyIdel, Consumer.Properties.Resources.enemyIdel, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            game.addGameObjectPictureBox(ObjectType.enemyRun, Consumer.Properties.Resources.enemyRunLeft, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            ////Adding game object Progress Bar
            game.addGameObjectProgressBar(ObjectType.player, 100, 40, 15, 0, 80);
            game.addGameObjectProgressBar(ObjectType.enemyIdel, 100, 40, 15, 0, 80);
            game.addGameObjectProgressBar(ObjectType.enemyRun, 100, 40, 15, 0, 80);
            ////Chasing The player
            ChasingClass chass = new ChasingClass(ObjectType.player, ObjectType.enemyIdel, new SmartChasing(3));
            game.addChasingIntoList(chass);
            chass = new ChasingClass(ObjectType.player, ObjectType.enemyRun, new SmartChasing(5));
            game.addChasingIntoList(chass);
            ////Adding collision
            CollisionClass playerCollisionWithEnemy = new CollisionClass(ObjectType.player, ObjectType.enemyIdel, new PLayerCollision());
            CollisionClass EnemyCollisionWithPlayer = new CollisionClass(ObjectType.enemyIdel, ObjectType.player, new EnemyCollision());
            CollisionClass EnemyRunCollisionWithPlayer = new CollisionClass(ObjectType.enemyRun, ObjectType.player, new EnemyCollision());
            game.addCollsionIntoList(playerCollisionWithEnemy);
            game.addCollsionIntoList(EnemyCollisionWithPlayer);
            game.addCollsionIntoList(EnemyRunCollisionWithPlayer);

        }
        #endregion

        #region Level3
        public void level3()
        {

        }
        #endregion

        #region General Load Functions
        private void customEventHandler()
        {
            game.OnPictureBoxAdded += new EventHandler(addPbxIntoFormControls);
            game.OnPictureBoxRemoved += new EventHandler(removePbxFromControls);
            game.onProgressBarAdded += new EventHandler(addPbarIntoFormControls);
            game.onProgressBarRemoved += new EventHandler(removePbarFromControls);
        }
        private void addPbxIntoFormControls(object sender, EventArgs e)
        {
            this.Controls.Add((PictureBox)sender);
        }
        private void removePbxFromControls(object sender, EventArgs e)
        {
            this.Controls.Remove((PictureBox)sender);
        }
        private void addPbarIntoFormControls(object sender, EventArgs e)
        {
            this.Controls.Add((ProgressBar)sender);
        }
        private void removePbarFromControls(object sender, EventArgs e)
        {
            this.Controls.Remove((ProgressBar)sender);
        }
        #endregion

       
    }
}
