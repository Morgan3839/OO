/***************************
 * Author: Ksenia Dorozhko *
 ***************************/

using System;

class SquareMatrix : ICloneable, IComparable {
  private const int minSize = 1;
  private const int maxSize = 10;

  private const int randomMin = 0;
  private const int randomMax = 10;

  private const int matrix1x1 = 1;
  private const int matrix2x2 = 2;

  private const int firstIndex = 0;
  private const int secondIndex = 1;

  private const int positiveSign = 1;
  private const int negativeSign = -1;
  
  private int[][] _elements;
  private int _size;

  public SquareMatrix(int size) {
    if (size < minSize || size > maxSize) {
      throw new InvalidMatrixSizeException("Matrix size must be from 1 to 10");
    }
    
    _size = size;
    _elements = new int[_size][];
    Random randomGenerator;
    randomGenerator = new Random();
    
    for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
      _elements[rowIndex] = new int[_size];
      for (int columnIndex = 0; columnIndex < _size; ++columnIndex) {
        _elements[rowIndex][columnIndex] = randomGenerator.Next(randomMin, randomMax);
      }
    }
  }

  public int this[int rowIndex, int columnIndex] {
    get { 
      return _elements[rowIndex][columnIndex]; 
    }
    set { 
      _elements[rowIndex][columnIndex] = value;
    }
  }

  public int Size {
    get { 
      return _size;
    }
  }

  public static SquareMatrix operator +(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
    if (leftMatrix._size != rightMatrix._size) {
      throw new MatrixOperationException("Matrices must be of same size for addition");
    }
    SquareMatrix resultMatrix;
    resultMatrix = new SquareMatrix(leftMatrix._size);
    for (int rowIndex = 0; rowIndex < leftMatrix._size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < leftMatrix._size; ++columnIndex) {
        resultMatrix[rowIndex, columnIndex] = leftMatrix[rowIndex, columnIndex] + rightMatrix[rowIndex, columnIndex];
      }
    }
    return resultMatrix;
  }

  public static SquareMatrix operator *(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
    if (leftMatrix._size != rightMatrix._size) {
      throw new MatrixOperationException("Matrices must be of same size for multiplication");
    }
    
    SquareMatrix resultMatrix;
    resultMatrix = new SquareMatrix(leftMatrix._size);
    
    for (int rowIndex = 0; rowIndex < leftMatrix._size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < leftMatrix._size; ++columnIndex) {
        int sumOfProducts;
        sumOfProducts = 0;
        for (int innerIndex = 0; innerIndex < leftMatrix._size; ++innerIndex) {
          sumOfProducts += leftMatrix[rowIndex, innerIndex] * rightMatrix[innerIndex, columnIndex];
        }
        resultMatrix[rowIndex, columnIndex] = sumOfProducts;
      }
    }
    return resultMatrix;
  }

  public override string ToString() {
    string matrixString;
    matrixString = "";
    for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < _size; ++columnIndex) {
        matrixString += _elements[rowIndex][columnIndex] + "\t";
      }
      matrixString += "\n";
    }
    return matrixString;
  }

  public int CompareTo(object other) {
    if (other is SquareMatrix matrix) {
      return this.Determinant().CompareTo(matrix.Determinant());
    }
    return -1;
  }

  public override bool Equals(object obj) {
    if (!(obj is SquareMatrix otherMatrix)) {
      return false;
    }
    if (otherMatrix._size != _size) {
      return false;
    }
    for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < _size; ++columnIndex) {
        if (_elements[rowIndex][columnIndex] != otherMatrix[rowIndex, columnIndex]) {
          return false;
        }
      }
    }
    return true;
  }

  public override int GetHashCode() {
    return Determinant();
  }

  public int Determinant() {
    if (_size == matrix1x1) {
      return _elements[firstIndex][firstIndex];
    }
    if (_size == matrix2x2)
      return _elements[firstIndex][firstIndex] * _elements[secondIndex][secondIndex] - _elements[firstIndex][secondIndex] * _elements[secondIndex][firstIndex];
    int det;
    det = 0;
    for (int columnIndex = 0; columnIndex < _size; ++columnIndex) {
      int sign;
      if (columnIndex % matrix2x2 == firstIndex) {
        sign = positiveSign;
      } else {
        sign = negativeSign;
      }
      det += sign * _elements[firstIndex][columnIndex] * Minor(firstIndex, columnIndex).Determinant();
    }
    return det;
  }

  private SquareMatrix Minor(int rowIndex, int columnIndex) {
    SquareMatrix minorMatrix;
    minorMatrix = new SquareMatrix(_size - 1);
    int minorRowIndex;
    minorRowIndex = firstIndex;
    for (int currentRowIndex = 0; currentRowIndex < _size; ++currentRowIndex) {
      if (currentRowIndex == rowIndex) {
        continue;
      }
      int minorColumnIndex;
      minorColumnIndex = 0;
      for (int currentColumnIndex = 0; currentColumnIndex < _size; ++currentColumnIndex) {
        if (currentColumnIndex == columnIndex) {
          continue;
        }
        minorMatrix[minorRowIndex, minorColumnIndex] = _elements[currentRowIndex][currentColumnIndex];
        ++minorColumnIndex;
      }
      ++minorRowIndex;
    }
    return minorMatrix;
  }

  public object Clone() {
    SquareMatrix cloneMatrix;
    cloneMatrix = new SquareMatrix(_size);
    for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < _size; ++columnIndex) {
        cloneMatrix[rowIndex, columnIndex] = _elements[rowIndex][columnIndex];
      }
    }
    return cloneMatrix;
  }
}

public class InvalidMatrixSizeException : Exception {
  public InvalidMatrixSizeException(string message) : base(message) { }
}

public class MatrixOperationException : Exception {
  public MatrixOperationException(string message) : base(message) { }
}

class MatrixCalculator {
  public void Run() {
    try {
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
    catch (Exception ex) {
      Console.WriteLine("Error: " + ex.Message);
    }
  }
}

class Program {
  static void Main() {
    MatrixCalculator calculator;
    calculator = new MatrixCalculator();
    calculator.Run();
  }
}