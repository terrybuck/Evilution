﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.Foundation;

namespace EvilutionClass
{
    public class GenericItem
    {
        //Constructor, default name is empty string if name is not provided
        public GenericItem(string name = "")
        {
            this.Name = name;

            this.DrawBoundingRectangle = true;
        }

        ///<summary>
        ///Update the GenericItem
        ///</summary>
        ///<param name= "dt"> A delta time since the last update was called.</param>
       public virtual void Update(TimeSpan dt, GenericInput input)
        {
            float velocity = (float)(1.0 * (dt.TotalMilliseconds)/60.0);
            this.Location = new Vector2(this.Location.X + velocity, this.Location.Y + velocity);
        }

        ///<summary>
        ///Draw the GenericItem
        ///</summary>
        ///<param name= "cds"> A surface to draw on.</param>
        public virtual void Draw(CanvasDrawingSession cds)
        {
            if (null == Bitmap)
                return;

            cds.DrawImage(Bitmap, Location);

            if (DrawBoundingRectangle)
            {
                cds.DrawRectangle(BoundingRectangle, Windows.UI.Colors.Purple);
            }
        }


        public bool SetBitmapFromImageDictionary(string key)
        {
            CanvasBitmap cb = null;
            if(ImageManager.ImageDictionary.TryGetValue(key, out cb))
            {
                this.Bitmap = cb;
                this.Size = this.Bitmap.SizeInPixels;
                return true;
            }
            return false;
        }

        //Properties
        public string Name { get; set; }
        public Vector2 Location { get; set; }
        public CanvasBitmap Bitmap { get; set; }
        public BitmapSize Size { get; set; }
        public Rect BoundingRectangle { get { return new Rect(Location.X, Location.Y, Size.Width-1, Size.Height-1); } }
        public bool DrawBoundingRectangle { get; set; }



    }
}
