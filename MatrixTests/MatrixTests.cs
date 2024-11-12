using MathLibrary;

namespace MatrixTests;

public class MatrixTests
{
    [SetUp]
    public void Setup() { }
    
    [Test]
    public void CorrectRowNumberInEmptyMatrixTest()
    {
        var matrixA = new Matrix<int>(null);
        Assert.That(matrixA.GetMatrix(), Is.Empty);
    }
    
    [Test]
    public void CorrectRowNumberInMatrixTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 0, 1, 2 }, { 3, 4, 5 } });
        Assert.That(matrixA.Rows, Is.EqualTo(2));
    }

    [Test]
    public void CorrectColumnNumberInMatrixTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });
        Assert.That(matrixA.Columns, Is.EqualTo(3));
    }

    [Test]
    public void CorrectIndexerInMatrixTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 54, 1, 99 }, { 34, 24, 57 } });
        Assert.That(matrixA[0, 2], Is.EqualTo(99));
    }

    [Test]
    public void CorrectAdditionOf2MatricesTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 0, 1, 2 } });
        var matrixB = new Matrix<int>(new[,] { { 1, 2, 3 } });
        Matrix<int> result = matrixA + matrixB;
        Assert.That(result.GetMatrix(), Is.EqualTo(new[,] { { 1, 3, 5 } }));
    }

    [Test]
    public void CorrectMultiplicationOf2MatricesTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 1, 5 } });
        var matrixB = new Matrix<int>(new[,] { { 7, 2 }, { 1, 0 } });
        Matrix<int> result = matrixA * matrixB;
        Assert.That(result.GetMatrix(), Is.EqualTo(new[,] { { 12, 2 } }));
    }

    [Test]
    public void ArgumentExceptionInAdditionWhenOneMatrixIsEmptyTest()
    {
        var matrixA = new Matrix<int>(null);
        var matrixB = new Matrix<int>(new[,] { { 7, 2 }, { 1, 0 } });

        Assert.Throws<ArgumentException>(Add);

        void Add()
        {
            Matrix<int> matrix = matrixA + matrixB;
        }
    }
    
    [Test]
    public void ArgumentExceptionInMultiplicationWhenOneMatrixIsEmptyTest()
    {
        var matrixA = new Matrix<int>(null);
        var matrixB = new Matrix<int>(new[,] { { 7, 2 }, { 1, 0 } });

        Assert.Throws<ArgumentException>(Mult);

        void Mult()
        {
            Matrix<int> matrix = matrixA * matrixB;
        }
    }

    [Test]
    public void ArgumentExceptionInAdditionWhenSizeAreNotCorrectTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 1, 10 } });
        var matrixB = new Matrix<int>(new[,] { { 7, 2 }, { 1, 0 } });

        Assert.Throws<ArgumentException>(Add);

        void Add()
        {
            Matrix<int> matrix = matrixA + matrixB;
        }
    }

    [Test]
    public void ArgumentExceptionInMultiplicationWhenSizeAreNotCorrectTest()
    {
        var matrixA = new Matrix<int>(new[,] { { 1, 10 }, { 1, 0 } });
        var matrixB = new Matrix<int>(new[,] { { 7, 2 } });

        Assert.Throws<ArgumentException>(Mult);

        void Mult()
        {
            Matrix<int> matrix = matrixA * matrixB;
        }
    }
}