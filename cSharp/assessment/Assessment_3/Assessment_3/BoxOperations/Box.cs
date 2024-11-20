using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_3.BoxOperations
{
    internal class Box
    {
        private readonly float _length;
        private readonly float _width;

        public Box(float length, float width)
        {
            _length = length;
            _width = width;
        }

        public static Box operator +(Box box1, Box box2)
        {
            Box box3 = new Box(box1._length + box2._length, box1._width + box2._width);
            return box3;
        }

        public void Display()
        {
            Console.WriteLine($"Length of box: {_length}");
            Console.WriteLine($"Width of box: {_width}");
        }
    }
}
