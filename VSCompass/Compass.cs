using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace VSCompass
{
    public class Compass : System.Windows.Forms.Panel
    {
        private int _width = 200;
        public int width
        {
            get { return _width; }
            set
            {
                _width = value;
                this.Width = _width;
                compassView.Width = _width;
            }
        }
        private int _height = 200;
        public int height
        {
            get { return _height; }
            set
            {
                _height = value;
                this.Height = _height;
                compassView.Height = _height;
            }
        }
        public Double northPositionX { get; set; }
        public Double northPositionY { get; set; }
        public Double targetPositionX { get; set; }
        public Double targetPositionY { get; set; }
        public Double northAngle { get; set; }
        private Double tempNorthAngle;
        public Double targetAngle { get; set; }
        private Double tempTargetAngle;
        PictureBox compassView = new PictureBox();
        private Bitmap _roseBitmap;     
        public Bitmap roseBitmap
        {
            get { return _roseBitmap; }
            set
            {
                _roseBitmap = value;
                ResizeRose();
                Combine();
            }
        }
        private Bitmap _needleBitmap;
        public Bitmap needleBitmap
        {
            get { return _needleBitmap; }
            set
            {
                _needleBitmap = value;
                ResizeNeedle();
                Combine();
            }
        }
        Bitmap compass { get; set; }

        public Compass()
        {
            this.Size = new Size(200,200);
            northPositionX = _width / 2;
            northPositionY = 0;
            targetPositionX = _width / 2;
            targetPositionY = 0;
            compassView.Size = new Size(200,200);
            this.Name = "Compass";
            this.Controls.Add(compassView);
        }
        public Compass(int x, int y)
        {
            width = x;
            height = y;
            this.Size = new Size(width, height);
            compassView.Size = new Size(width ,height);
            this.Name = "Compass";
            northPositionX = _width / 2;
            northPositionY = 0;
            targetPositionX = _width / 2;
            targetPositionY = 0;
            this.Controls.Add(compassView);
        }
        public Compass(Bitmap roseBitmap, Bitmap needleBitmap, int x, int y)
        {
            width = x;
            height = y;
            this.Size = new Size(width, height);
            compassView.Size = new Size(width, height);
            this.roseBitmap = roseBitmap;
            ResizeRose();
            this.needleBitmap = needleBitmap;
            ResizeNeedle();
            this.Controls.Add(compassView);
            northPositionX = _width / 2;
            northPositionY = 0;
            targetPositionX = _width / 2;
            targetPositionY = 0;
            Combine();       
        }
        public Compass(Bitmap roseBitmap, Bitmap needleBitmap)
        {           
            this.Size = new Size(200,200);
            compassView.Size = new Size(200, 200);
            this.roseBitmap = roseBitmap;
            ResizeRose();
            this.needleBitmap = needleBitmap;
            ResizeNeedle();
            northPositionX = _width / 2;
            northPositionY = 0;
            targetPositionX = _width / 2;
            targetPositionY = 0;
            this.Controls.Add(compassView);
            Combine();
        }        
        void ResizeNeedle()
        {
            if (_needleBitmap != null)
            {
                try
                {
                    _needleBitmap = new Bitmap(_needleBitmap, new Size(this.width,this.height));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
        void ResizeRose()
        {
            if (_roseBitmap != null)
            {
                try
                {
                    _roseBitmap = new Bitmap(_roseBitmap, new Size(width,height));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        void Combine()
        {
            if(width != 0 && height != 0)
            {
                compass = new Bitmap(width, height);
            }  
            
            if (_roseBitmap != null && _needleBitmap != null)
            {
                ResizeRose();
                ResizeNeedle();
                using (Graphics g = Graphics.FromImage(compass))
                {
                    g.DrawImage(_roseBitmap, Point.Empty);
                    g.DrawImage(_needleBitmap, Point.Empty);                   
                }
                compassView.Image = compass;
            }           
        }

        public void RotateNeedle()
        {
            if (_needleBitmap != null)
            {
                double centerX = width / 2;
                double centerY = height / 2;
                tempNorthAngle = northAngle;
                northAngle = Math.Atan2(northPositionY - centerY, northPositionX - centerX) * (180 / Math.PI) + 90;
                if (tempNorthAngle != northAngle) {
                    float rotation;
                    if (northAngle >= 0 && tempNorthAngle >= 0)
                    {
                        rotation = (float)(northAngle - tempNorthAngle);
                    }
                    else if (northAngle < 0 && tempNorthAngle >= 0)
                    {
                        rotation = (float)-(Math.Abs(northAngle) +Math.Abs(tempNorthAngle));
                    }
                    else if(northAngle < 0 && tempNorthAngle < 0)
                    {
                        rotation = (float)-(Math.Abs(northAngle) -Math.Abs(tempNorthAngle));
                    }
                    else
                    {
                        rotation = (float)(Math.Abs(northAngle) +Math.Abs(tempNorthAngle));
                    }
                    Bitmap bp = new Bitmap(width, height);                    
                    using (Graphics g = Graphics.FromImage(bp))
                    {
                        g.TranslateTransform((float)bp.Width / 2, (float)bp.Height / 2);                        
                        g.RotateTransform(rotation);
                        g.TranslateTransform(-(float)bp.Width / 2, -(float)bp.Height / 2);
                        g.DrawImage(needleBitmap, new Point((bp.Width - needleBitmap.Width) / 2, (bp.Height - needleBitmap.Height) / 2));
                    }
                    _needleBitmap = bp;
                }
            }            
            Combine();
        }
        public void RotateRose()
        {
            if (_roseBitmap != null)
            {
                double centerX = width / 2;
                double centerY = height / 2;                
                tempTargetAngle = targetAngle;
                targetAngle = Math.Atan2(targetPositionY - centerY, targetPositionX - centerX) * (180 / Math.PI) + 90;
                if (tempTargetAngle != targetAngle)
                {
                    float rotation;
                    if (targetAngle >= 0 && tempTargetAngle >= 0)
                    {
                        rotation = (float)(targetAngle - tempTargetAngle);
                    }
                    else if (targetAngle < 0 && tempTargetAngle >= 0)
                    {
                        rotation = (float)-(Math.Abs(targetAngle) + Math.Abs(tempTargetAngle));
                    }
                    else if (targetAngle < 0 && tempTargetAngle < 0)
                    {
                        rotation = (float)-(Math.Abs(targetAngle) - Math.Abs(tempTargetAngle));
                    }
                    else
                    {
                        rotation = (float)(Math.Abs(targetAngle) + Math.Abs(tempTargetAngle));
                    }
                    Bitmap bp = new Bitmap(width, height);
                    using (Graphics g = Graphics.FromImage(bp))
                    {
                        g.TranslateTransform((float)bp.Width / 2, (float)bp.Height / 2);
                        g.RotateTransform(rotation);
                        g.TranslateTransform(-(float)bp.Width / 2, -(float)bp.Height / 2);
                        g.DrawImage(_roseBitmap, new Point((bp.Width - roseBitmap.Width) / 2, (bp.Height - roseBitmap.Height) / 2));
                    }
                    _roseBitmap = bp;
                }
                Combine();
            }
        }        
        public Double CalculateAngle()
        {
            double centerX = width / 2;
            double centerY = height / 2;
            double needleAngle = Math.Atan2(northPositionY - centerY, northPositionX - centerX) * (180/Math.PI)+90;
            double roseAngle = Math.Atan2(targetPositionY - centerY, targetPositionX - centerX) * (180 / Math.PI)+90;
            Double result = Math.Abs(needleAngle) + Math.Abs(roseAngle);
            return result;
        }
    }
}
