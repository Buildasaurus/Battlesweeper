using Games;
using System.Drawing;

namespace Tests
{

    public class GridUnitTest
    {
        // simple test object which implements ICopyable.
        class Foo : ICopyable<Foo>
        {
            public Foo(int id)
            {
                this.id = id;
            }

            public int id;

            public Foo Copy()
            {
                return new(id);
            }
        }

        [Fact]
        public void Constructor()
        {
            // check if the grid is able to contruct.
            Grid<Foo?> grid = new(new Size(10, 10));

            Assert.True(grid.Size.Equals(new Size(10, 10)));

            foreach (Foo? foo in grid)
                // every element should be null, as this is the default value of Foo?
                Assert.True(foo == null);
        }

        [Fact]
        public void ConstructorWithDefaultValue()
        {
            // attempt to construct a grid, with a non null default value.
            Foo fill_value = new(25);
            Grid<Foo> grid = new(new Size(20, 20), fill_value);

            Assert.True(grid.Size.Equals(new Size(20, 20)));

            foreach (Foo foo in grid)
            {
                Assert.True(foo.id == fill_value.id);
                // it is important that fill_value and foo does not share the same address,
                // as we do not want to modify the grid elements, if we modify fill_value,
                // after the Grid has been constructed.
                Assert.True(foo != fill_value);
            }
        }

        [Fact]
        public void Index1D()
        {
            // check if the 1d index works, by placing an expected value at a specific index,
            // and checking if it matches up.
            Grid<Foo?> grid = new(new Size(10, 10));

            grid[15] = new(32);

            for (int i = 0; i < grid.Size.Width * grid.Size.Height; i++)
                if (i == 15)
                    Assert.True(grid[i]!.id == 32);
                else
                    Assert.True(grid[i] == null);

            // check if the 2D index also works
            Assert.True(grid[5, 1]!.id == 32);

        }

        [Fact]
        public void Index2D()
        {
            // same as Index1D, but for an 2D index.
            Grid<Foo?> grid = new(new Size(10, 10));

            grid[5, 1] = new(32);

            for(int x = 0; x < grid.Size.Width; x++)
            {
                for(int y = 0; y < grid.Size.Height; y++)
                {
                    if (x == 5 && y == 1)
                        Assert.True(grid[x, y]!.id == 32);
                    else
                        Assert.True(grid[x, y] == null);
                }
            }

            // check if the 1D index also works
            Assert.True(grid[15]!.id == 32);
        }
    }
}