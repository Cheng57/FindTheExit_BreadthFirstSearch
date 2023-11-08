using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTheExit;
using DoublyLinkedLists;
using System.IO;
using System.Collections.Generic;

namespace FindTheExit
{
    /// <summary>
    /// Represents a maze.
    /// </summary>
    class Maze
    {
        private char[][] charMaze;
        private Stack<Point> stack;
        private FindTheExit2.Queue<Point> queue;

        /// <summary>
        /// Gets or sets the starting point of the maze.
        /// </summary>
        public Point StartingPoint
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the row length of the maze.
        /// </summary>
        public int RowLength
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the column length of the maze.
        /// </summary>
        public int ColumnLength
        {
            get; set;
        }

        /// <summary>
        /// Reads a specified file.
        /// </summary>
        /// <param name="fileName">The specified file.</param>
        public Maze(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(fileName);

                string line;
                int row = 0;

                //Add 'line = reader.ReadLine()' in the while condition statement so that it will read line by line each loop.
                while ((line = reader.ReadLine()) != null)
                {
                    if (row == 0)
                    {
                        string[] size = line.Split(' ');

                        RowLength = int.Parse(size[0]);
                        ColumnLength = int.Parse(size[1]);

                        charMaze = new char[RowLength][];
                        for (int i = 0; i < RowLength; i++)
                        {
                            charMaze[i] = new char[ColumnLength];
                        }
                    }

                    else if (row == 1)
                    {
                        string[] startPoint = line.Split(' ');

                        StartingPoint = new Point(int.Parse(startPoint[0]), int.Parse(startPoint[1]));
                    }
                    else
                    {
                        char[] rowChars = line.ToCharArray();

                        if (rowChars.Length != ColumnLength)
                        {
                            throw new ArgumentException("Invalid maze dimensions in the file.");
                        }

                        Array.Copy(rowChars, charMaze[row - 2], rowChars.Length);

                    }

                    row++;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the maze file: " + e.Message);
            }
        }

        /// <summary>
        /// Returns a character maze array.
        /// </summary>
        /// <returns></returns>
        public char[][] GetMaze()
        {
            return charMaze;
        }

        /// <summary>
        /// Initializes an instance of maze with the specified starting point(starting row and starting column) 
        /// and the specified maze.
        /// </summary>
        /// <param name="startingRow">The row of the specified starting point.</param>
        /// <param name="startingColumn">The column of the specified starting point.</param>
        /// <param name="existingMaze">The specified maze.</param>
        public Maze(int startingRow, int startingColumn, char[][] existingMaze)
        {
            if (startingRow < 0 || startingColumn < 0)
            {
                throw new IndexOutOfRangeException("Negative startingRow or negative startingColumn is not allowed.");
            }

            if (startingRow > existingMaze.Length || startingColumn > existingMaze[0].Length)
            {
                throw new IndexOutOfRangeException("startingRow or startingColumn is not allowed to be out of the boudary.");
            }

            //Starting point is on the Exit that is represented by the character E.
            if (existingMaze[startingRow][startingColumn] == 'E')
            {
                throw new ApplicationException("Starting point is not allowed to be on the exit.");
            }

            //Starting point is on the Wall that is represented by the character W.
            if (existingMaze[startingRow][startingColumn] == 'W')
            {
                throw new ApplicationException("Starting point is not allowed to be on the wall.");
            }

            StartingPoint = new Point(startingRow, startingColumn);

            RowLength = existingMaze.Length;
            ColumnLength = existingMaze[0].Length;

            charMaze = existingMaze;
        }

        /// <summary>
        /// Prints the maze.
        /// </summary>
        /// <returns>The string representation of the maze.</returns>
        public string PrintMaze()
        {
            string mazeOutput = "";

            for (int i = 0; i < RowLength; i++)
            {
                for (int j = 0; j < ColumnLength; j++)
                {
                    mazeOutput += charMaze[i][j];
                }

                // No '\n' added after the last iteration of the inner loop.
                if (i < RowLength - 1)
                {
                    mazeOutput += "\n";
                }
            }

            return mazeOutput;
        }

        /// <summary>
        /// Checks if the point is valid to move.
        /// </summary>
        /// <param name="row">The row of the point.</param>
        /// <param name="col">The column of the point.</param>
        /// <returns></returns>
        private bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < RowLength && col >= 0 && col < ColumnLength && charMaze[row][col] != 'W' && charMaze[row][col] != 'V';
        }

        /// <summary>
        /// Returns a stack containing the locations in the order of starting point to exit(top to bottom).
        /// Requires to run the BreadthFirstSearch() before.
        /// </summary>
        /// <returns>The stack.</returns>
        public Stack<Point> GetPathToFollow()
        {
            if (queue == null)
            {
                throw new ApplicationException("You have not yet searched the maze.");
            }

            Stack<Point> stack1 = new Stack<Point>();
            Stack<Point> stack2 = new Stack<Point>();
            Stack<Point> pathStack = new Stack<Point>();

            int stackSize = stack.Size;
            for (int i = 0; i < stackSize; i++)
            {
                Point point = stack.Pop();
                stack1.Push(point);
                stack2.Push(point);
                
            }

            int stack1Size = stack1.Size;
            for (int i = 0; i < stack1Size; i++)
            {
                Point point = stack1.Pop();
                pathStack.Push(point);
            }

            //Pushs elements back to stack
            int stack2Size = stack2.Size;
            for (int i = 0; i < stack2Size; i++)
            {
                Point point = stack2.Pop();
                
                stack.Push(point);    
            }

            return pathStack;
        }

        /// <summary>
        /// Performs a breadth first search. Returns a shortese path from the starting point to the exit.
        /// </summary>
        /// <returns>The string representation of the path and a printout of the maze after exploring.</returns>
        public string BreadthFirstSearch()
        {
            queue = new FindTheExit2.Queue<Point>();
            
            charMaze[StartingPoint.Row][StartingPoint.Column] = 'V';

            queue.Enqueue(StartingPoint);

            Point currentPoint = StartingPoint;

            stack = new Stack<Point>();

            while (!queue.IsEmpty())
            {
                currentPoint = queue.Dequeue();

                int row = currentPoint.Row;
                int col = currentPoint.Column;
                Point parentPoint = currentPoint.ParentPoint;

                char[] directions = { 'S', 'E', 'W', 'N' };
                
                foreach (char direction in directions)
                {
                    int newRow = row;
                    int newCol = col;
                    Point newParentPoint = currentPoint;

                    if (direction == 'S') newRow++;
                    else if (direction == 'E') newCol++;
                    else if (direction == 'W') newCol--;
                    else if (direction == 'N') newRow--;

                    if (IsValidMove(newRow, newCol))
                    {
                        if (charMaze[newRow][newCol] == 'E')
                        {
                            Point current = currentPoint;
                            //Pushes Point Exit First to the stack.
                            stack.Push(new Point(newRow, newCol));

                            //Pushes parentpoint to the stack.
                            while(current != null)
                            {
                                stack.Push(current);
                                current = current.ParentPoint;
                            }

                            int size = stack.Size;
                            int count = 0;

                            List<string> list = new List<string>();
                            Stack<Point> stack1 = new Stack<Point>();

                            while (count < size)
                            {
                                Point point = stack.Pop();


                                stack1.Push(point);

                                list.Add(point.ToString());

                                //Changes 'V's to '.'.
                                if (charMaze[point.Row][point.Column] != 'E')
                                {
                                    charMaze[point.Row][point.Column] = '.';
                                }

                                count++;
                            }

                            //Pushes elements back to stack.
                            int stack1Size = stack1.Size;
                            for (int i = 0; i < stack1Size; i++)
                            {
                                Point point = stack1.Pop();
                                stack.Push(point);
                            }

                            //Point Exit.
                            string exit = list[size - 1];

                            string str = null;

                            for (int i = 0; i < list.Count; i++)
                            {
                                str += list[i] + "\n";
                            }

                            return $"Path to follow from Start {StartingPoint} to Exit {exit} - {size} steps:\n{str}{PrintMaze()}";
                        }
                        charMaze[newRow][newCol] = 'V';

                        queue.Enqueue(new Point(newRow, newCol, newParentPoint));
                    }  
                }
            }
            return $"No exit found in maze!\n\n{PrintMaze()}";
        }
    }
}
