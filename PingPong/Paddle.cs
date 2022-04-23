using Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PingPong
{
    public class Paddle
    {
        public PictureBox pBox = new PictureBox();
        public bool isRacketMovingUp;
        public bool isRacketMovingDown;
        public int Score = 0;
        public readonly PaddleSide Side;
        public Form form;

        public Point Position
        {
            get
            {
                return pBox.Location;
            }
            set
            {
                pBox.Location = value;
            }
        }

        public Paddle(Image rImage, PaddleSide side, Form2 form)
        {
            Side = side;

            pBox.Size = new Size(18, 130);
            pBox.Image = rImage;
            if (Side == PaddleSide.Left)
                pBox.Location = new Point(0, (form.Size.Height - 200) / 2);
            else if (Side == PaddleSide.Right)
                pBox.Location = new Point(form.Size.Width - pBox.Size.Width * 2, (form.Size.Height - 200) / 2);
            else
                throw new Exception("Side is not `Left` or `Right`");
            Score = 0;
            form.Controls.Add(pBox);
            this.form = form;
        }
    }
}
