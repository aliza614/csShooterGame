using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace csShooterGame
{
    public class Bullet
    {
        public string direction;
        public int bulletSpeed = 20;
        PictureBox bullet=new PictureBox();
        Timer tm=new Timer();

        public int left;
        public int top;

        public void MakeBullet(Form form)
        {
            bullet.BackColor=System.Drawing.Color.White;
            bullet.Size = new Size(5, 5);
            bullet.Tag = "bullet";
            bullet.Left= left;
            bullet.Top= top;
            bullet.BringToFront();
            form.Controls.Add(bullet);
            tm.Interval = bulletSpeed;
            tm.Tick += new EventHandler(tm_Tick);
            tm.Start();
        }
        public void tm_Tick(object sender, EventArgs e)
        {
            if (direction == "left") bullet.Left -= bulletSpeed;
            if (direction == "right") bullet.Left += bulletSpeed;
            if(direction=="up")       bullet.Top -= bulletSpeed;
            if(direction=="down")     bullet.Top+=bulletSpeed;
            //if the bullet goes off of the screen remove
            if(bullet.Left<16||bullet.Left>860||bullet.Top<10||bullet.Top>616)
            {
                /*tm.Stop();
                tm.Dispose();
                bullet.Dispose();
                tm= null;
                bullet= null;*/
                if (bullet.Left<16&& direction=="left") bullet.Left=860;
                if (bullet.Left > 860 && direction == "right") bullet.Left = 16;
                if (bullet.Top < 10 && direction == "up") bullet.Top = 616;
                if (bullet.Top > 616 && direction == "down") bullet.Top = 10;
            }
            
        }

    }
}
