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
            //toggle to see rect around objects
            this.DrawBoundingRectangle = false;
        }

        ///<summary>
        ///Update the GenericItem
        ///</summary>
        ///<param name= "dt"> A delta time since the last update was called.</param>
       public virtual void Update(TimeSpan dt, GenericInput input)
        {

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetSize(uint width, uint height)
        {
            BitmapSize new_size;
            new_size.Width = width;
            new_size.Height = height;
            this.Size = new_size;
        }

        /// <summary>
        /// This should reset the GenericItem back to its default state.
        /// </summary>
        public virtual void Reset()
        {
        }


        //Properties
        public string Name { get; set; }
        public Vector2 Location { get; set; }
        public CanvasBitmap Bitmap { get; set; }
        public BitmapSize Size { get; set; }
        public Rect BoundingRectangle { get { return new Rect(Location.X, Location.Y, Size.Width-1, Size.Height-1); } }
        public bool DrawBoundingRectangle { get; set; }

        public int X
        {
            get { return (int)Location.X; }
            set
            {
                if (Location.X != value)
                {
                    Location = new Vector2(value, Location.Y);
                }
            }
        }

        public int Y
        {
            get { return (int)Location.Y; }
            set
            {
                if (Location.Y != value)
                {
                    Location = new Vector2(Location.X, value);
                }
            }
        }


    }
}
