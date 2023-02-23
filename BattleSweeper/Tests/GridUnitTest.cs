using Games;
using System.Drawing;

namespace Tests
{

    public class GridUnitTest
    {
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
            Grid<Foo?> grid = new(new Size(10, 10));

            Assert.True(grid.Size.Equals(new Size(10, 10)));

            foreach (Foo? foo in grid)
                Assert.True(foo == null);
        }

        [Fact]
        public void ConstructorWithDefaultValue()
        {
            Foo fill_value = new(25);
            Grid<Foo> grid = new(new Size(20, 20), fill_value);

            Assert.True(grid.Size.Equals(new Size(20, 20)));

            foreach (Foo foo in grid)
            {
                Assert.True(foo.id == fill_value.id);
                Assert.True(foo != fill_value);
            }
        }

        [Fact]
        public void Index1D()
        {
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