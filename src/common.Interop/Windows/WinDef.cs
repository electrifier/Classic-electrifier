/*
** 
**  electrifier
** 
**  Copyright 2018 Thorsten Jung, www.electrifier.org
**  
**  Licensed under the Apache License, Version 2.0 (the "License");
**  you may not use this file except in compliance with the License.
**  You may obtain a copy of the License at
**  
**      http://www.apache.org/licenses/LICENSE-2.0
**  
**  Unless required by applicable law or agreed to in writing, software
**  distributed under the License is distributed on an "AS IS" BASIS,
**  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
**  See the License for the specific language governing permissions and
**  limitations under the License.
**
*/

using System.Runtime.InteropServices;

namespace common.Interop
{
    public static partial class Windows
    {
        /// <summary>
        /// Helper object for RECT-structure.
        /// 
        /// Originally pulled from https://www.pinvoke.net/default.aspx/Structures/rect.html
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left, Top, Right, Bottom;

            public Rect(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public Rect(System.Drawing.Rectangle r)
                : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X {
                get { return this.Left; }
                set { this.Right -= (this.Left - value); this.Left = value; }
            }

            public int Y {
                get { return this.Top; }
                set { this.Bottom -= (this.Top - value); this.Top = value; }
            }

            public int Height {
                get { return this.Bottom - this.Top; }
                set { this.Bottom = value + this.Top; }
            }

            public int Width {
                get { return this.Right - this.Left; }
                set { this.Right = value + this.Left; }
            }

            public System.Drawing.Point Location {
                get { return new System.Drawing.Point(this.Left, this.Top); }
                set { this.X = value.X; this.Y = value.Y; }
            }

            public System.Drawing.Size Size {
                get { return new System.Drawing.Size(this.Width, this.Height); }
                set { this.Width = value.Width; this.Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(Rect r) => new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);

            public static implicit operator Rect(System.Drawing.Rectangle r) => new Rect(r);

            public static bool operator ==(Rect r1, Rect r2) => r1.Equals(r2);

            public static bool operator !=(Rect r1, Rect r2) => !r1.Equals(r2);

            public bool Equals(Rect r)
            {
                return r.Left == this.Left && r.Top == this.Top && r.Right == this.Right && r.Bottom == this.Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is Rect)
                    return this.Equals((Rect)obj);
                else if (obj is System.Drawing.Rectangle)
                    return this.Equals(new Rect((System.Drawing.Rectangle)obj));

                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture,
                    "{{Left={0},Top={1},Right={2},Bottom={3}}}", this.Left, this.Top, this.Right, this.Bottom);
            }
        }
    }
}
