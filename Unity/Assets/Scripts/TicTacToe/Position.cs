﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo
{
    public class Position
    {
        int x;
        int y;

        public Position()
        {
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return x;
        }

        public void setX(int x)
        {
            this.x = x;
        }

        public int getY()
        {
            return y;
        }

        public void setY(int y)
        {
            this.y = y;
        }

    }
}
