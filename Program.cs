/***************************
 * Author: Ksenia Dorozhko *
 ***************************/

using System;

// Class "Square Matrix"
class SquareMatrix : ICloneable, IComparable
{
    private int[][] _elements;
    private int _size;

    public SquareMatrix(int size)
    {
        if (size < 1 || size > 10)
        {
            throw new InvalidMatrixSizeException("Matrix size must be from 1 to 10");
        }
        _size = size;
        _elements = new int[_size][];
        Random randomGenerator;
        randomGenerator = new Random();
        for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
        {
            _elements[rowIndex] = new int[_size];
            for (int columnIndex = 0; columnIndex < _size; ++columnIndex)
            {
                _elements[rowIndex][columnIndex] = randomGenerator.Next(0, 10);
            }
        }
    }

    public int this[int rowIndex, int columnIndex]
    {
        get { return _elements[rowIndex][columnIndex]; }
        set { _elements[rowIndex][columnIndex] = value; }
    }

    public int Size
    {
        get { return _size; }
    }

    // Operator Overloading
    public static SquareMatrix operator +(SquareMatrix leftMatrix, SquareMatrix rightMatrix)
    {
        if (leftMatrix._size != rightMatrix._size)
        {
            throw new MatrixOperationException("Matrices must be of same size for addition");
        }
        SquareMatrix resultMatrix;
        resultMatrix = new SquareMatrix(leftMatrix._size);
        for (int rowIndex = 0; rowIndex < leftMatrix._size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < leftMatrix._size; ++columnIndex)
            {
                resultMatrix[rowIndex, columnIndex] = leftMatrix[rowIndex, columnIndex] + rightMatrix[rowIndex, columnIndex];
            }
        }
        return resultMatrix;
    }

    public static SquareMatrix operator *(SquareMatrix leftMatrix, SquareMatrix rightMatrix)
    {
        if (leftMatrix._size != rightMatrix._size)
        {
            throw new MatrixOperationException("Matrices must be of same size for multiplication");
        }
        SquareMatrix resultMatrix;
        resultMatrix = new SquareMatrix(leftMatrix._size);
        for (int rowIndex = 0; rowIndex < leftMatrix._size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < leftMatrix._size; ++columnIndex)
            {
                int sumOfProducts;
                sumOfProducts = 0;
                for (int innerIndex = 0; innerIndex < leftMatrix._size; ++innerIndex)
                {
                    sumOfProducts += leftMatrix[rowIndex, innerIndex] * rightMatrix[innerIndex, columnIndex];
                }
                resultMatrix[rowIndex, columnIndex] = sumOfProducts;
            }
        }
        return resultMatrix;
    }

    // Methods ToString, CompareTo, Equals, GetHashCode
    public override string ToString()
    {
        string matrixString;
        matrixString = "";
        for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < _size; ++columnIndex)
            {
                matrixString += _elements[rowIndex][columnIndex] + "\t";
            }
            matrixString += "\n";
        }
        return matrixString;
    }

    public int CompareTo(object other)
    {
        if (other is SquareMatrix matrix)
        {
            return this.Determinant().CompareTo(matrix.Determinant());
        }
        return -1;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is SquareMatrix otherMatrix)) return false;
        if (otherMatrix._size != _size) return false;
        for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < _size; ++columnIndex)
            {
                if (_elements[rowIndex][columnIndex] != otherMatrix[rowIndex, columnIndex]) return false;
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        return Determinant();
    }

    // Determinant
    public int Determinant()
    {
        if (_size == 1) return _elements[0][0];
        if (_size == 2) return _elements[0][0] * _elements[1][1] - _elements[0][1] * _elements[1][0];
        int det;
        det = 0;
        for (int columnIndex = 0; columnIndex < _size; ++columnIndex)
        {
            int sign;
            sign = (columnIndex % 2 == 0) ? 1 : -1;
            det += sign * _elements[0][columnIndex] * Minor(0, columnIndex).Determinant();
        }
        return det;
    }

    private SquareMatrix Minor(int rowIndex, int columnIndex)
    {
        SquareMatrix minorMatrix;
        minorMatrix = new SquareMatrix(_size - 1);
        int minorRowIndex;
        minorRowIndex = 0;
        for (int currentRowIndex = 0; currentRowIndex < _size; ++currentRowIndex)
        {
            if (currentRowIndex == rowIndex) continue;
            int minorColumnIndex;
            minorColumnIndex = 0;
            for (int currentColumnIndex = 0; currentColumnIndex < _size; ++currentColumnIndex)
            {
                if (currentColumnIndex == columnIndex) continue;
                minorMatrix[minorRowIndex, minorColumnIndex] = _elements[currentRowIndex][currentColumnIndex];
                ++minorColumnIndex;
            }
            ++minorRowIndex;
        }
        return minorMatrix;
    }

    // Deep Copy Prototype
    public object Clone()
    {
        SquareMatrix cloneMatrix;
        cloneMatrix = new SquareMatrix(_size);
        for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < _size; ++columnIndex)
            {
                cloneMatrix[rowIndex, columnIndex] = _elements[rowIndex][columnIndex];
            }
        }
        return cloneMatrix;
    }
}

// Custom Exceptions
public class InvalidMatrixSizeException : Exception
{
    public InvalidMatrixSizeException(string message) : base(message) { }
}

public class MatrixOperationException : Exception
{
    public MatrixOperationException(string message) : base(message) { }
}

// Test application "Matrix Calculator"
class MatrixCalculator
{
    public void Run()
    {
        try
        {
            Console.Write("Enter matrix size (max 10x10): ");
            int matrixSize;
            matrixSize = int.Parse(Console.ReadLine());

            SquareMatrix matrixA;
            matrixA = new SquareMatrix(matrixSize);
            SquareMatrix matrixB;
            matrixB = new SquareMatrix(matrixSize);

            Console.WriteLine("Matrix A:");
            Console.WriteLine(matrixA);

            Console.WriteLine("Matrix B:");
            Console.WriteLine(matrixB);

            SquareMatrix sumMatrix;
            sumMatrix = matrixA + matrixB;
            Console.WriteLine("A + B:");
            Console.WriteLine(sumMatrix);

            SquareMatrix productMatrix;
            productMatrix = matrixA * matrixB;
            Console.WriteLine("A * B:");
            Console.WriteLine(productMatrix);

            Console.WriteLine("A > B: " + (matrixA.CompareTo(matrixB) > 0));
            Console.WriteLine("A == B: " + matrixA.Equals(matrixB));

            SquareMatrix clonedMatrix;
            clonedMatrix = (SquareMatrix)matrixA.Clone();
            Console.WriteLine("\nCloned matrix A:");
            Console.WriteLine(clonedMatrix);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}

class Program
{
    static void Main()
    {
        MatrixCalculator calculator;
        calculator = new MatrixCalculator();
        calculator.Run();
    }
}