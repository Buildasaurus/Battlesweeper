using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games
{
    /// <summary>
    /// 
    /// Dataclass representing a static 2D grid.
    /// 
    /// </summary>
    /// <typeparam name="T">
    /// datatype to contain in the grid.
    /// Must implement ICopyable,
    /// in order to prevent the constructor from filling the grid with references to the same object.
    /// </typeparam>
    public class Grid<T> where T : class?, ICopyable<T>
    {
        /// <summary>
        /// 
        /// construct a Grid with the passed size
        /// optionally, fill all values with a default value.
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="val"> value to fill grid with. If null, fill with default value of T </param>
        public Grid(Size size, T? val = null)
        {
            m_data = new List<T>(size.Width * size.Height);
            Size = size;

            for(int i = 0; i < size.Width * size.Height; i++)
                m_data.Add(val?.Copy() ?? default!);
        }

        public Size Size { get; }

        /// <summary>
        /// acces the 2D grid, as a 1D array, with size Width * Height.
        /// </summary>
        public T this[int indx]
        {
            get => m_data[indx];
            set => m_data[indx] = value;
        }

        /// <summary>
        /// access the element stored in the 2D matrix, at the position (x, y)
        /// </summary>
        public T this[int x, int y]
        {
            get
            {
                if (!inBounds(new(x, y)))
                    throw new IndexOutOfRangeException();

                return this[x + y * Size.Width];
            }
            set
            {
                if (!inBounds(new(x, y)))
                    throw new IndexOutOfRangeException();

                this[x + y * Size.Width] = value;
            }
        }

        /// <summary>
        /// access the element stored in the 2D matrix, at the position (coord.X, coord.Y)
        /// </summary>
        public T this[Point coord]
        {
            get => this[coord.X, coord.Y];
            set => this[coord.X, coord.Y] = value;
        }

        /// <summary>
        /// check whether the passed coordinate is a valid point, inside the grid.
        /// </summary>
        public bool inBounds(Point coord) =>
            coord.X >= 0 && coord.X < Size.Width && coord.Y >= 0 && coord.Y < Size.Height;

        public List<T>.Enumerator GetEnumerator()
        {
            return m_data.GetEnumerator();
        }

        protected List<T> m_data;
    }
}
