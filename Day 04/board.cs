using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace Day_04
{
    public struct BoardPosition
    {
        internal int Number;
        internal bool Marked;
    }

    public class Board
    {
        private readonly BoardPosition[,] _grid;

        public Board(BoardPosition[,] grid)
        {
            _grid = grid;
        }

        public bool IsWinner => Rows().Any(static r => r.All(static p => p.Marked)) ||
                                Columns().Any(static r => r.All(static p => p.Marked));

        private IEnumerable<IEnumerable<BoardPosition>> Rows()
        {
            for (var row = 0; row < _grid.GetLength(0); row++)
            {
                var row1 = row;
                yield return Enumerable.Range(0, _grid.GetLength(1)).Select(x => _grid[row1, x]);
            }
        }

        private IEnumerable<IEnumerable<BoardPosition>> Columns()
        {
            for (var row = 0; row < _grid.GetLength(0); row++)
            {
                var row1 = row;
                yield return Enumerable.Range(0, _grid.GetLength(1)).Select(x => _grid[x, row1]);
            }
        }

        public IEnumerable<BoardPosition> Enumerate()
        {
            for (var x = 0; x < _grid.GetLength(0); x++)
            for (var y = 0; y < _grid.GetLength(1); y++)
                yield return _grid[x, y];
        }

        public void Mark(int call)
        {
            for (var x = 0; x < _grid.GetLength(0); x++)
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                if (_grid[x, y].Number != call)
                    continue;

                _grid[x, y].Marked = true;
            }
        }
    }
}
