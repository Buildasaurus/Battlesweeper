using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games
{
    public class Grid<T> where T : class, ICopyable<T>
    {
        public Grid(Size size, T? val = null)
        {
            m_data = new List<T>(size.Width * size.Height);
            Size = size;

            for(int i = 0; i < size.Width * size.Height; i++)
            {
                m_data.Add(val?.Copy() ?? default!);
            }
        }

        public Size Size { get; }

        public T this[int indx]
        {
            get => m_data[indx];
            set => m_data[indx] = value;
        }

        public T this[int x, int y]
        {
            get
            {
                if (x > Size.Width || y > Size.Height)
                    throw new IndexOutOfRangeException();

                return this[x + y * Size.Width];
            }
            set
            {
                if (x > Size.Width || y > Size.Height)
                    throw new IndexOutOfRangeException();

                this[x + y * Size.Width] = value;
            }
        }

        public T this[Point coord]
        {
            get => this[coord.X, coord.Y];
            set => this[coord.X, coord.Y] = value;
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return m_data.GetEnumerator();
        }

        protected List<T> m_data;
    }
}
