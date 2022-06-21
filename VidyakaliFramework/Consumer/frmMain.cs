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
        #region Load Event 

        private Game game;
        private Random random = new Random();
        private int energyTime;
        private bool isLevel1 = true;
        private bool isLevel2 = false;
        private bool isLevel3 = false;
        private Keys keyPressed;
        private PictureBox nextLevelBox;

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
        private void restart()
        {
            clearAllObjects();
            createNextLevelBox();
            nextLevelBox.Visible = false;
            isLevel1 = true;
            isLevel2 = false;
            isLevel3 = false;
            keyPressed=Keys.None;
            level1();
        }
        #endregion

        #region Tick Event
        private int firFixTime = 20;
        private int firTime;
        int counter;
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            showEnergyPoint();
            string message =game.update(lblScore, pgbarPlayerLife,5);
            if (message == NextLevel.won.ToString())
            {
                if (isLevel1==true)
                {
                    nextLevelBox.Visible = true;
                    detectCollsionwithBoxL1();
                }
                else if (isLevel2 == true)
                {
                    nextLevelBox.Visible = true;
                    detectCollsionwithBoxL2();
                }
            }
            if (message == NextLevel.lost.ToString())
            {
                gameLoop.Enabled = false;
                isLevel1 = false;
                string info = "Game Over";
                frmEnd end = new frmEnd(info);
                DialogResult result = end.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    restart();
                }
                else
                {
                    Close();
                }
            }
            if (message == NextLevel.level3.ToString())
            {
                gameLoop.Enabled = false;
                isLevel3 = false;
                string info = "You Won";
                frmEnd end = new frmEnd(info);
                DialogResult result = end.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    restart();
                }
                else
                {
                    Close();
                }
            }
            counter = random.Next(1, 500);
            if (isLevel3 == true)
            {
                if (250 == counter)
                {
                    counter = 0;
                    level3ObjectsNormal();

                }
                else if (100 == counter)
                {
                    counter = 0;
                    level3ObjectSmart();
                }
                firTime++;
                level3InLoop();
            }

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
                                if (isLevel1 == true)
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

        #region Energy Point
        private void showEnergyPoint()
        {
            try
            {
                energyTime++;
                if (60 == energyTime)
                {
                    if (pgbarPlayerLife.Value <= random.Next(0, 80))
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

        #region Remove the object 
        public void clearAllObjects()
        {
            for (int i = 0; i < game.GoPictureBoxList.Count; i++)
            {
                if (game.GoPictureBoxList[i].Otype == ObjectType.energy)
                {
                    this.Controls.Remove((game.GoPictureBoxList[i].Pbx));
                    game.GoPictureBoxList.RemoveAt(i);
                }
            }
            for (int i = 0; i < game.GoPictureBoxList.Count; i++)
            {
                if (game.GoPictureBoxList[i].Otype== ObjectType.player|| game.GoPictureBoxList[i].Otype == ObjectType.enemyIdel || game.GoPictureBoxList[i].Otype == ObjectType.enemyRun)
                {
                    this.Controls.Remove((game.GoPictureBoxList[i].Pbx));
                    game.GoPictureBoxList.RemoveAt(i);
                }
              
            }
            for (int i = 0; i < game.GoProgressBarList.Count; i++)
            {
                if (game.GoProgressBarList[i].Otype == ObjectType.player || game.GoProgressBarList[i].Otype == ObjectType.enemyIdel || game.GoProgressBarList[i].Otype == ObjectType.enemyRun)
                {
                    this.Controls.Remove((game.GoProgressBarList[i].Pbar));
                    game.GoProgressBarList.RemoveAt(i);
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
            game.addGameObjectPictureBox(ObjectType.enemyRun, Consumer.Properties.Resources.enemyRunLeft, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            ////Adding game object Progress Bar
            game.addGameObjectProgressBar(ObjectType.player, 100, 40, 15, 0, 80);
            game.addGameObjectProgressBar(ObjectType.enemyRun, 100, 40, 15, 0, 80);
            ////Chasing The player
            ChasingClass chass = new ChasingClass(ObjectType.player, ObjectType.enemyRun, new SmartChasing(8));
            game.addChasingIntoList(chass);
            ////Adding collision
            CollisionClass playerCollisionWithEnemy = new CollisionClass(ObjectType.player, ObjectType.enemyRun, new PLayerCollision());
            CollisionClass EnemyRunCollisionWithPlayer = new CollisionClass(ObjectType.enemyRun, ObjectType.player, new EnemyCollision());
            game.addCollsionIntoList(playerCollisionWithEnemy);
            game.addCollsionIntoList(EnemyRunCollisionWithPlayer);

        }
        #endregion

        #region Level 3
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
            game.addGameObjectProgressBar(ObjectType.player, 100, 40, 15, 0, 80);
            level3ObjectsNormal();
        }
        public void level3InLoop()
        {
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
        }
        
        private void level3ObjectsNormal()
        {
            ////Event handlers
            customEventHandler();
            ////Boundary of the form
            Point boundary = new Point(this.Width - 75, this.Height);
            ///Adding game object Picture Box
            game.addGameObjectPictureBox(ObjectType.enemyIdel, Consumer.Properties.Resources.enemyIdel, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            game.addGameObjectPictureBox(ObjectType.enemyRun, Consumer.Properties.Resources.enemyRunLeft, random.Next(100, 300), random.Next(100, 400), new Horizontal(0, boundary, "right", 100));
            ////Adding game object Progress Bar
           
            game.addGameObjectProgressBar(ObjectType.enemyIdel, 100, 40, 15, 0, 80);
            game.addGameObjectProgressBar(ObjectType.enemyRun, 100, 40, 15, 0, 80);
            ////Chasing The player
            ChasingClass chass = new ChasingClass(ObjectType.player, ObjectType.enemyIdel, new SmartChasing(1));
            game.addChasingIntoList(chass);
            chass = new ChasingClass(ObjectType.player, ObjectType.enemyRun, new SmartChasing(3));
            game.addChasingIntoList(chass);
            ////Adding collision
            CollisionClass playerCollisionWithEnemy = new CollisionClass(ObjectType.player, ObjectType.enemyIdel, new PLayerCollision());
            CollisionClass EnemyCollisionWithPlayer = new CollisionClass(ObjectType.enemyIdel, ObjectType.player, new EnemyCollision());
            CollisionClass EnemyRunCollisionWithPlayer = new CollisionClass(ObjectType.enemyRun, ObjectType.player, new EnemyCollision());
            game.addCollsionIntoList(playerCollisionWithEnemy);
            game.addCollsionIntoList(EnemyCollisionWithPlayer);
            game.addCollsionIntoList(EnemyRunCollisionWithPlayer);
        }
        private void level3ObjectSmart()
        {
            ////Event handlers
            customEventHandler();
            ////Boundary of the form
            Point boundary = new Point(this.Width - 75, this.Height);
            ///Adding game object Picture Box
            game.addGameObjectPictureBox(ObjectType.enemyIdel, Consumer.Properties.Resources.enemyIdel, random.Next(100, 300), random.Next(100, 400), new Horizontal(2, boundary, "right", 100));
            ////Adding game object Progress Bar
            game.addGameObjectProgressBar(ObjectType.enemyIdel, 100, 40, 15, 0, 80);
            ////Chasing The player
            ChasingClass chass = new ChasingClass(ObjectType.player, ObjectType.enemyIdel, new SmartChasing(1));
            game.addChasingIntoList(chass);
            ////Adding collision
            CollisionClass playerCollisionWithEnemy = new CollisionClass(ObjectType.player, ObjectType.enemyIdel, new PLayerCollision());
            CollisionClass EnemyCollisionWithPlayer = new CollisionClass(ObjectType.enemyIdel, ObjectType.player, new EnemyCollision());
            game.addCollsionIntoList(playerCollisionWithEnemy);
            game.addCollsionIntoList(EnemyCollisionWithPlayer);
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
