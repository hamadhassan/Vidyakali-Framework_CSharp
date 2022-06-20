using System;
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
        private Keys keyPressed;
        private PictureBox nextLevelBox;
        #region Load Event 
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            game.keyPressed(e.KeyCode, Consumer.Properties.Resources.idle, Consumer.Properties.Resources.runLeft, Consumer.Properties.Resources.runRight);
            keyPressed = e.KeyCode;
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
                                    level3();
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
        private int firFixTime = 20;
        private int firTime;
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            showEnergyPoint();
            string message =game.update(lblScore, pgbarPlayerLife);
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
            }
            else if (message == NextLevel.lost.ToString())
            {
                gameLoop.Enabled = false;
                MessageBox.Show("lost");
            }
            if (isLevel3 == true)
            {
                firTime++;
                if (firTime == firFixTime)
                {
                    if (keyPressed == Keys.A)
                    {
                        game.addGameObjectPictureBoxFire(Consumer.Properties.Resources.laserPlayerRight, "left", 10, ObjectType.player, -20, 40);
                        keyPressed = Keys.None;
                    }
                    if (keyPressed == Keys.D)
                    {
                        game.addGameObjectPictureBoxFire(Consumer.Properties.Resources.laserPlayerLeft, "right", 10, ObjectType.player, 0, 40);
                        keyPressed = Keys.None;
                    }
                    if (keyPressed == Keys.W)
                    {
                        game.addGameObjectPictureBoxFire(Consumer.Properties.Resources.laserPlayerUp, "up", 10, ObjectType.player, 10, -40);
                        keyPressed = Keys.None;
                    }
                    if (keyPressed == Keys.S)
                    {
                        game.addGameObjectPictureBoxFire(Consumer.Properties.Resources.laserPlayerDown, "down", 10, ObjectType.player, 10, 40);
                        keyPressed = Keys.None;
                    }
                    // clearDieObject();
                    firTime = 0;
                }
                level3InLoop();
                //isLevel3 = false;
                if (5 == random.Next(1, 5))
                {
                    level3Objects();
                }
            }
            
        }
        #endregion
        #region EnergyPoint
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
        public void clearDieObject()
        {
            for(int i = 0; i < game.GoPictureBoxList.Count; i++)
            {
                for(int j=0;j<game.GoProgressBarList.Count; j++)
                {
                    for (int k = 0; k < game.GoFirePictureBoxeList.Count; k++)
                    {
                        if (game.GoPictureBoxList[i]!=null && game.GoProgressBarList[i]!=null&& game.GoFirePictureBoxeList[i] != null)
                        {
                            if(game.GoPictureBoxList[j].Pbx.Visible == false)
                            {
                                this.Controls.Remove(game.GoPictureBoxList[i].Pbx);
                                game.GoPictureBoxList[i] = null;
                            }
                            if (game.GoProgressBarList[j].Pbar.Visible == false)
                            {
                                this.Controls.Remove(game.GoProgressBarList[i].Pbar);
                                game.GoProgressBarList[j] = null;
                            }
                            if (game.GoFirePictureBoxeList[k].Pbx.Visible == false)
                            {
                                this.Controls.Remove(game.GoFirePictureBoxeList[k].Pbx);
                                game.GoFirePictureBoxeList[k] = null;
                            }
                        }
                    }
                }
            }
            for(int i=0;i< game.GoFirePictureBoxeList.Count; i++)
            {
                if (game.GoFirePictureBoxeList[i] != null)
                {
                     if (game.GoFirePictureBoxeList[i].Pbx.Left >= this.Width)
                    {
                        this.Controls.Remove(game.GoFirePictureBoxeList[i].Pbx);
                        game.GoFirePictureBoxeList[i] = null;
                    }
                    else if (game.GoFirePictureBoxeList[i].Pbx.Left <= -25)
                    {
                        this.Controls.Remove(game.GoFirePictureBoxeList[i].Pbx);
                        game.GoFirePictureBoxeList[i] = null;
                    }
                    else if (game.GoFirePictureBoxeList[i].Pbx.Top <= 0)
                    {
                        this.Controls.Remove(game.GoFirePictureBoxeList[i].Pbx);
                        game.GoFirePictureBoxeList[i] = null;
                    }
                    else if (game.GoFirePictureBoxeList[i].Pbx.Top <= this.Width)
                    {
                        this.Controls.Remove(game.GoFirePictureBoxeList[i].Pbx);
                        game.GoFirePictureBoxeList[i] = null;
                    }
                }
               
            }
        }
        #endregion

        #region Level 1
        private void level1()
        {
            ////Level Numebr
            lblLevel.Text = "1";
            ////Background Color
            this.BackColor = Color.DarkOliveGreen;
            ////Game initiliziation
            game = new Game(5, 1, 2, 1, true);
            ////Event handlers
            customEventHandler();
            ////Boundary of the form
            Point boundary = new Point(this.Width - 75, this.Height);
            //Adding game object Picture Box
            game.addGameObjectPictureBox(ObjectType.player, Consumer.Properties.Resources.idle, random.Next(100, 300), random.Next(100, 400), new Keyboard(20, boundary, 100));
            game.addGameObjectPictureBox(ObjectType.enemyIdel, Consumer.Properties.Resources.enemyIdel, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            ////Adding game object Progress Bar
            game.addGameObjectProgressBar(ObjectType.player, 100, 40, 15, 0, 80);
            game.addGameObjectProgressBar(ObjectType.enemyIdel, 100, 40, 15, 0, 80);
            ////Chasing The player
            ChasingClass chass = new ChasingClass(ObjectType.player, ObjectType.enemyIdel, new SmartChasing(3));
            game.addChasingIntoList(chass);
            ////Adding collision
            CollisionClass playerCollisionWithEnemy = new CollisionClass(ObjectType.player, ObjectType.enemyIdel, new PLayerCollision());
            CollisionClass EnemyCollisionWithPlayer = new CollisionClass(ObjectType.enemyIdel, ObjectType.player, new EnemyCollision());
            game.addCollsionIntoList(playerCollisionWithEnemy);
            game.addCollsionIntoList(EnemyCollisionWithPlayer);
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
            game.addGameObjectPictureBox(ObjectType.player, Consumer.Properties.Resources.idle, random.Next(100, 300), random.Next(100, 400), new Keyboard(20, boundary, 100));
            level3Objects();
        }
        public void level3InLoop()
        {
          
        }
        private void level3Objects()
        {
            ////Event handlers
            customEventHandler();
            ////Boundary of the form
            Point boundary = new Point(this.Width - 75, this.Height);
            ///Adding game object Picture Box
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
