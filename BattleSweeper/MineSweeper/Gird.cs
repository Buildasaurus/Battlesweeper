using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games
{
    public class Grid<T> where T : class
    {
        public Grid(int width, int height, T? val = null)
        {
            m_data = new List<T>(width * height);
            m_width = width;

            for(int i = 0; i < width * height; i++)
            {
                m_data.Add(val);
            }

        }

        public T this[int x, int y]
        {
            get
            {
                if (x > m_width || y > m_data.Count / m_width)
                    throw new IndexOutOfRangeException();

                return m_data[x + y * m_width];
            }
            set
            {
                if (x > m_width || y > m_data.Count / m_width)
                    throw new IndexOutOfRangeException();

                m_data[x + y * m_width] = value;
            }
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return m_data.GetEnumerator();
        }

        protected List<T> m_data;
        protected int m_width;
    }
}
