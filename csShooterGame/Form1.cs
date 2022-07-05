using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csShooterGame
{
    public partial class Form1 : Form
    {
        bool isGoingUp;
        bool isGoingDown;
        bool isGoingLeft;
        bool isGoingRight;
        string facing = "up";
        double playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        int score = 0;
        bool isGameOver=false;
        const int FORM_WIDTH= 930;
        const int FORM_HEIGHT = 700;

        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (isGameOver) return;
            if(e.KeyCode == Keys.Down)
            {
                isGoingDown = isGoingLeft = isGoingRight = isGoingUp = false;
                isGoingDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }
            if (e.KeyCode == Keys.Up)
            {
                isGoingDown=isGoingLeft=isGoingRight=isGoingUp=false;
                isGoingUp = true;
                facing = "up";
                player.Image=Properties.Resources.up;
            }
            if(e.KeyCode == Keys.Left)
            {
                isGoingDown= isGoingLeft = isGoingRight=isGoingUp=false;
                isGoingLeft=true;
                facing = "left";
                player.Image= Properties.Resources.left;
            }
            if(e.KeyCode == Keys.Right)
            {
                isGoingDown=isGoingLeft=isGoingRight = isGoingUp=false;
                isGoingRight=true;
                facing="right";
                player.Image= Properties.Resources.right;
            }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (isGameOver) return;
            if(e.KeyCode == Keys.Up)
            {
                isGoingUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                isGoingDown = false;
            }
            if(e.KeyCode==Keys.Left)
            {
                isGoingLeft=false;
            }
            if (e.KeyCode == Keys.Right)
            {
                isGoingRight = false;
            }
            if (e.KeyCode == Keys.Space && ammo > 0)
            {
                ammo--;
                this.label1.Text = $"     Ammo: {ammo}";
                Shoot(facing);
                if (ammo < 5) {
                    DropAmmo();
                    
                 }
            }

        }

        private void GameEngine(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                progressBar1.Value=Convert.ToInt32((int)playerHealth);
            }
            else
            {
                player.Image = Properties.Resources.dead;
                timer1.Stop();
                isGameOver = true;
            }
            label1.Text = $"  Ammo:    {ammo}";
            label2.Text=$"Kills:    {score}";
            if (playerHealth < 20)
            {
                progressBar1.ForeColor = System.Drawing.Color.Red;
            }
            else 
            { progressBar1.ForeColor = System.Drawing.Color.Green; }
            if (isGoingLeft && player.Left > 0)
            {
                player.Left -= speed;
            }
            if(isGoingRight && player.Left + player.Width < FORM_WIDTH)
            {
                player.Left+=speed;
            }
            if(isGoingUp&player.Top > 60)
            {
                player.Top-=speed;
            }
            if (isGoingDown && player.Top + player.Height < FORM_HEIGHT)
            {
                player.Top+=speed;
            }
            newForEach:
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && x.Tag == "ammo")
                    {
                        if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                        {
                            this.Controls.Remove((PictureBox)x);//remove ammo picture
                            ((PictureBox)x).Dispose();
                            ammo += 5;
                        }
                    }
                    if (x is PictureBox && x.Tag == "bullet")
                    {
                        PictureBox bullet = (PictureBox)x;
                        if (bullet.Left < 1 || bullet.Left > FORM_WIDTH || bullet.Top < 10 || bullet.Top > FORM_HEIGHT)
                        {
                            this.Controls.Remove(bullet);
                            bullet.Dispose();
                        }
                    }
                    if (x is PictureBox && x.Tag == "zombie")
                    {
                        PictureBox zombie = (PictureBox)x;
                        if (zombie.Bounds.IntersectsWith(player.Bounds))
                        {
                            playerHealth -= 1;
                        }
                        if (zombie.Left > player.Left)
                        {
                            zombie.Left -= zombieSpeed;
                            zombie.Image = Properties.Resources.zleft;
                        }
                        if (zombie.Top > player.Top)
                        {
                            zombie.Top -= zombieSpeed;
                            zombie.Image = Properties.Resources.zup;
                        }
                        if (zombie.Left < player.Left)
                        {
                            zombie.Left += zombieSpeed;
                            zombie.Image = Properties.Resources.zright;
                        }
                        if (zombie.Top < player.Top)
                        {
                            zombie.Top += zombieSpeed;
                            zombie.Image = Properties.Resources.zdown;
                        }
                    }
                    foreach (Control j in this.Controls)
                    {//nested loop to identify bullet and zombie
                        if ((j is PictureBox && j.Tag == "bullet") && (x is PictureBox && x.Tag == "zombie"))
                        {
                            PictureBox zombie = (PictureBox)x;
                            PictureBox bullet = (PictureBox)j;
                            if (zombie.Bounds.IntersectsWith(bullet.Bounds))
                            {
                                score++;
                                this.Controls.Remove(bullet);
                                bullet.Dispose();
                                this.Controls.Remove(zombie);
                                zombie.Dispose();
                                MakeZombies();
                                this.Refresh();
                            goto newForEach;
                            }
                        }

                    }
                }
            

        }
        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = random.Next(10, FORM_WIDTH-10);
            ammo.Top = random.Next(50, FORM_HEIGHT-10);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);
            ammo.BringToFront();
           
            player.BringToFront();
        }
        private void Shoot(string direction)
        {
            Bullet bullet = new Bullet();
            bullet.direction=direction;
            bullet.left=player.Left+player.Width/2;
            bullet.top=player.Top+player.Height/2;
            bullet.MakeBullet(this);
        }
        private void MakeZombies()
        {
            PictureBox zombie=new PictureBox();
            zombie.Image = Properties.Resources.zdown;
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombie.Left = random.Next(0, FORM_WIDTH);
            zombie.Top = random.Next(0, FORM_HEIGHT);
            zombie.Tag = "zombie";
            this.Controls.Add(zombie);
            player.BringToFront();
        }

        
    }
}
