using System;
using System.Collections.Generic;
using System.Text;

namespace SchiffeVersenken
{

    enum Border
    {
        none,
        dot,
        line,
        star,
        dollar,
        sharp,
        plus,
        ddot,
        emark,
    }
    enum Align
    {
        left,
        center,
        right,
    }

    //<summary>
    //The Window class allows to create a
    //writeable area inside the console.
    //</summary>
    class Window
    {
        // Save & Restore
        private int _cleft;
        private int _ctop;
        private ConsoleColor _ccolor;

        // Position & Größe
        private int _left;
        private int _top;
        private int _width;
        private int _height;
        public int Left
        {
            get { return this._left; }
            set
            {
                this._left = value;
                this.refresh();
            }
        }
        public int Top
        {
            get { return this._top; }
            set
            {
                this._top = value;
                this.refresh();
            }
        }
        public int Width
        {
            get { return this._width; }
            set
            { 
                this._width = value;
                this.refresh();
            }
        }
        public int Height
        {
            get { return this._height; }
            set 
            { 
                this._height = value;
                this.refresh();
            }
        }

        // Farbe & Formatierung
        private ConsoleColor _color;
        public ConsoleColor Color
        {
            get { return this._color; }
            set
            {
                this._color = value;
                this.refresh();
            }
        }

        private Align _halign = Align.left;
        public Align HorizontaleAusrichtung
        {
            get { return this._halign; }
            set
            {
                int i = 0;
                for (int j = 0; j < this._zeilen.Length; j++)
                {
                    switch (this._halign)
                    {
                        case Align.left:
                            {
                                i = this._zeilen[j].Length - 1;
                                while (i >= 0 && this._zeilen[j][i] == ' ')
                                {
                                    this._zeilen[j] = this._zeilen[j].Remove(this._zeilen[j].Length - 1);
                                    i--;
                                }
                                break;
                            }
                        case Align.right:
                            {
                                while (this._zeilen[j].Length > 0 && this._zeilen[j][0] == ' ')
                                {
                                    this._zeilen[j] = this._zeilen[j].Remove(0, 1);
                                }
                                break;
                            }
                        case Align.center:
                            {
                                while (this._zeilen[j].Length > 0 && this._zeilen[j][0] == ' ')
                                {
                                    this._zeilen[j] = this._zeilen[j].Remove(0, 1);
                                }
                                i = this._zeilen[j].Length - 1;
                                while (i >= 0 && this._zeilen[j][i] == ' ')
                                {
                                    this._zeilen[j] = this._zeilen[j].Remove(this._zeilen[j].Length - 1);
                                    i--;
                                }
                                break;
                            }
                    }
                }
                this._halign = value;
                this.refresh();
            }
        }

        // Inhalt
        private String[] _zeilen;
        private int _letzte_Zeile;

        //Überschrift
        private string _title = "";
        public string Title
        {
            get { return this._title; }
            set
            {
                this._title = value;
                if (this._border != Border.none)
                {
                    this.printBorder();
                }
            }
        }
        private Align _talign = Align.left;
        public Align TitleAlign
        {
            get { return this._talign; }
            set
            {
                this._talign = value;
                if (this._border != Border.none)
                {
                    this.printBorder();
                }
            }
        }
        private ConsoleColor _tcolor;
        public ConsoleColor TitleColor
        {
            get { return this._tcolor; }
            set
            {
                this._tcolor = value;
                if (this._border != Border.none)
                {
                    this.printBorder();
                }
            }
        }

        // Umrandung
        private ConsoleColor _bcolor;
        public ConsoleColor BorderColor
        {
            get { return this._bcolor; }
            set
            {
                this._bcolor = value;
                if (this._border != Border.none)
                {
                    this.printBorder();
                }
            }
        }
        private Border _border = Border.none;
        public Border Border
        {
            get { return this._border; }
            set
            {
                if (this._border == Border.none && value != Border.none)
                {
                    this._left++;
                    this._top++;
                    this._width -= 2;
                    this._height -= 2;
                    for (int i = 0; i < this._zeilen.Length; i++)
                    {
                        this._zeilen[i] = this._zeilen[i].Substring(0, this._width);
                    }
                }
                else if (value == Border.none && this._border != Border.none)
                {
                    this._left--;
                    this._top--;
                    this._width += 2;
                    this._height += 2;
                }
                this._border = value;
                this.refresh();
            }
        }

        private void moveUp(int n)
        {
            if (n >= this._height)
            {
                for (int i = 0; i < this._zeilen.Length - 1; i++)
                {
                    this._zeilen[i] = "";
                }
            }
            else
            {
                for (int i = 0; i < this._zeilen.Length - n; i++)
                {
                    this._zeilen[i] = this._zeilen[i + n];
                }
                for (int i = this._height - n; i < this._zeilen.Length; i++)
                {
                    this._zeilen[i] = "";
                }
            }
            if (this._letzte_Zeile > n)
            {
                this._letzte_Zeile -= n;
            }
            else
            {
                this._letzte_Zeile = -1;
            }
        }
        private void printZeile(int n)
        {
            n--;
            // Leerzeichen auffüllen
            while (this._zeilen[n].Length < this._width)
            {
                switch (this._halign)
                {
                    case Align.left:
                        {
                            this._zeilen[n] += " ";
                            break;
                        }
                    case Align.right:
                        {
                            this._zeilen[n] = " " + this._zeilen[n];
                            break;
                        }
                    case Align.center:
                        {
                            if (this._zeilen[n].Length % 2 == 0)
                            {
                                this._zeilen[n] += " ";
                            }
                            else
                            {
                                this._zeilen[n] = " " + this._zeilen[n];
                            }
                            break;
                        }
                }
            }
            this.saveConsole();
            // verändern
            Console.SetCursorPosition(this._left, this._top + n);
            Console.ForegroundColor = this._color;
            // schreiben
            Console.Write(this._zeilen[n]);
            this.restoreConsole();
        }
        private void printBorderSign()
        {
            switch (this._border)
            {
                case Border.dot:
                    {
                        Console.Write(".");
                        break;
                    }
                case Border.star:
                    {
                        Console.Write("*");
                        break;
                    }
                case Border.line:
                    {
                        Console.Write("_");
                        break;
                    }
                case Border.sharp:
                    {
                        Console.Write("#");
                        break;
                    }
                case Border.dollar:
                    {
                        Console.Write("$");
                        break;
                    }
                case Border.plus:
                    {
                        Console.Write("+");
                        break;
                    }
                case Border.ddot:
                    {
                        Console.Write(":");
                        break;
                    }
                case Border.emark:
                    {
                        Console.Write("!");
                        break;
                    }
                default:
                    {
                        Console.Write("");
                        break;
                    }
            }
        }
        private void printBorder()
        {
            this.saveConsole();
            Console.ForegroundColor = this._bcolor;
            Console.SetCursorPosition(this._left - 1, this._top - 1);
            // oben
            for (int i = 0; i < this._width + 2; i++)
            {
                this.printBorderSign();
            }
            // überschrift
            if (this._title.Length <= this._width)
            {
                ConsoleColor bkp = Console.ForegroundColor;
                Console.ForegroundColor = this._tcolor;
                switch (this._talign)
                {
                    case Align.left:
                        {
                            Console.SetCursorPosition(this._left, this._top - 1);
                            Console.Write(this._title);
                            break;
                        }
                    case Align.right:
                        {
                            Console.SetCursorPosition(this._left + this._width - this._title.Length, this._top - 1);
                            Console.Write(this._title);
                            break;
                        }
                    case Align.center:
                        {
                            Console.SetCursorPosition(this._left + ((this._width - this._title.Length) / 2), this._top - 1);
                            Console.Write(this._title);
                            break;
                        }
                }
                Console.ForegroundColor = bkp;
            }
            // unten
            Console.SetCursorPosition(this._left - 1, this._top + this._height);
            for (int i = 0; i < this._width + 2; i++)
            {
                this.printBorderSign();
            }
            // links und rechts
            for (int i = 0; i < this._height; i++)
            {
                // links
                Console.SetCursorPosition(this._left - 1, this._top + i);
                this.printBorderSign();
                // rechts
                Console.SetCursorPosition(this._left + this._width, this._top + i);
                this.printBorderSign();
            }
            this.restoreConsole();
        }
        private void saveConsole()
        {
            this._cleft = Console.CursorLeft;
            this._ctop = Console.CursorTop;
            this._ccolor = Console.ForegroundColor;
        }
        private void restoreConsole()
        {
            Console.CursorLeft = this._cleft;
            Console.CursorTop = this._ctop;
            Console.ForegroundColor = this._ccolor;
        }

        public Window(string title, int l, int t, int w, int h, ConsoleColor c)
        {
            this._left = l;
            this._top = t;
            this._width = w;
            this._height = h;
            this._color = c;
            this._bcolor = c;
            this._tcolor = c;
            this._title = title;
            this._zeilen = new string[h];
            for (int i = 0; i < h; i++)
            {
                this._zeilen[i] = " ";
            }
            this._letzte_Zeile = -1;
            this.refresh();
        }
        public Window(string title, int l, int t, int w, int h)
        {
            this._left = l;
            this._top = t;
            this._width = w;
            this._height = h;
            this._color = Console.ForegroundColor;
            this._bcolor = Console.ForegroundColor;
            this._tcolor = Console.ForegroundColor;
            this._title = title;
            this._zeilen = new string[h];
            for (int i = 0; i < h; i++)
            {
                this._zeilen[i] = " ";
            }
            this._letzte_Zeile = -1;
            this.refresh();
        }
        public Window(int w, int h)
        {
            this._left = Console.CursorLeft;
            this._top = Console.CursorTop;
            this._width = w;
            this._height = h;
            this._color = Console.ForegroundColor;
            this._bcolor = Console.ForegroundColor;
            this._tcolor = Console.ForegroundColor;
            this._zeilen = new string[h];
            for (int i = 0; i < h; i++)
            {
                this._zeilen[i] = " ";
            }
            this._letzte_Zeile = -1;
            this.refresh();
        }

        public void WriteLine(string s)
        {
            bool moved = false;
            while (this._letzte_Zeile >= this._height - 1)
            {
                this.moveUp(1);
                moved = true;
            }
            if (s.Length <= this._width)
            {
                this._zeilen[++this._letzte_Zeile] = s;
            }
            else
            {
                this._zeilen[++this._letzte_Zeile] = s.Substring(0, this._width);
                this.WriteLine(s.Substring(this._width));
            }
            if (moved)
            {
                this.refreshText();
            }
            else
            {
                this.printZeile(this._letzte_Zeile + 1);
            }
        }

        public void refresh()
        {
            if (this._border != Border.none)
            {
                this.printBorder();
            }
            for (int i = 1; i <= this._height; i++)
            {
                this.printZeile(i);
            }
        }
        public void refreshText()
        {
            for (int i = 1; i <= this._height; i++)
            {
                this.printZeile(i);
            }
        }
        public void refreshBorder()
        {
            if (this._border != Border.none)
            {
                this.printBorder();
            }
        }

        public void clear()
        {
            this.moveUp(this._height);
            this.refresh();
        }

        public void focus()
        {
            while (this._letzte_Zeile >= this._height - 1)
            {
                this.moveUp(1);
            }
            Console.SetCursorPosition(this._left, this._top + this._letzte_Zeile + 1);
        }
    }
}
