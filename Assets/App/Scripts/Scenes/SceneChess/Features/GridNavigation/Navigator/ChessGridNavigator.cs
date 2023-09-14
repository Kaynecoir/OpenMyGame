using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            List<Vector2Int> way = GetMinWay(unit, from, to, grid, null, 20);
            return way;
        }

        List<Vector2Int> GetMinWay(ChessUnitType unit, Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid, List<Vector2Int> havedWay = null, int depth = 1, int max_depth = 10)
        {
            if (havedWay == null)
            {
                havedWay = new List<Vector2Int>() { startPos };
            }
            List<Vector2Int> ableMoves = GetAllMoves(unit, startPos, chessGrid);

            bool _findTarget = false;
            foreach (Vector2Int p in havedWay)
            {
                if (p == targetPos)
                {
                    _findTarget = true;
                    break;
                }
            }
            if (_findTarget)
            {
                return havedWay;
            }
            else if (depth == 1 || havedWay.Count > max_depth)
            {
                return null;
            }
            else
            {
                int lengthWay = max_depth;
                List<Vector2Int> minWay = null;
                foreach (Vector2Int m in ableMoves)
                {
                    bool _continue = false;
                    foreach (Vector2Int p in havedWay)
                    {
                        if (p == m)
                        {
                            _continue = true;
                            break;
                        }
                    }
                    if (_continue) continue;

                    chessGrid.Move(startPos, m);
                    havedWay.Add(m);

                    List<Vector2Int> way = GetMinWay(unit, m, targetPos, chessGrid, havedWay, depth - 1, lengthWay);
                    if (way != null && way.Count < lengthWay)
                    {
                        minWay = new List<Vector2Int>(way);
                        lengthWay = minWay.Count;
                    }

                    havedWay.Remove(m);
                    chessGrid.Move(m, startPos);
                }
                return minWay;
            }
        }

        List<Vector2Int> GetAllMoves(ChessUnitType figureType, Vector2Int from, ChessGrid chessGrid)
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            Func<Vector2Int, Vector2Int, ChessGrid, bool> AbleMove = (figureType) switch
            {
                ChessUnitType.Pon => PawnAbleMove,  // I hope Pon it's Pawn
                ChessUnitType.Rook => RookAbleMove,
                ChessUnitType.Knight => KnightAbleMove,
                ChessUnitType.Bishop => BishopAbleMove,
                ChessUnitType.Queen => QueenAbleMove,
                ChessUnitType.King => KingAbleMove,
                _ => throw new NotImplementedException("It isn't figure."),
            };
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (AbleMove(from, new Vector2Int(x, y), chessGrid))
                    {
                        moves.Add(new Vector2Int(x, y));
                    }
                }
            }

            return moves;
        }

        public bool PawnAbleMove(Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid)
        {
            bool moveStatus = false;

            if (targetPos == startPos) return moveStatus;
            if (Math.Abs(targetPos.y - startPos.y) > 2 || Math.Abs(targetPos.y - startPos.y) == 0 || (targetPos.y - startPos.y > 0) != (chessGrid.Get(startPos).PieceModel.Color == ChessUnitColor.White)) return moveStatus;

            if (Math.Abs(targetPos.y - startPos.y) == 1)
            {
                if (targetPos.x == startPos.x && chessGrid.Get(targetPos) == null) moveStatus = true;
            }

            return moveStatus;
        }
        public bool RookAbleMove(Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid)
        {
            bool moveStatus = false;

            if (targetPos == startPos) return moveStatus;
            if (Math.Abs(targetPos.x - startPos.x) > 0 && Math.Abs(targetPos.y - startPos.y) > 0) return moveStatus;
            Vector2Int direction = (targetPos - startPos) / Math.Max(Math.Abs((targetPos - startPos).x), Math.Abs((targetPos - startPos).y));
            Vector2Int nowPos = startPos + direction;
            while (nowPos != targetPos)
            {
                if (chessGrid.Get(nowPos) != null) return moveStatus;
                nowPos += direction;
            }
            if (chessGrid.Get(targetPos) == null) moveStatus = true;

            return moveStatus;
        }
        public bool KnightAbleMove(Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid)
        {
            bool moveStatus = false;

            if (targetPos == startPos) return moveStatus;
            if (Math.Abs(targetPos.x - startPos.x) > 2 || Math.Abs(targetPos.y - startPos.y) > 2) return moveStatus;

            if (Math.Abs((targetPos - startPos).x) == 2 && Math.Abs((targetPos - startPos).y) == 1)
            {
                if (chessGrid.Get(targetPos) == null) moveStatus = true;
                if (chessGrid.Get(targetPos) != null) return false;
            }
            if (Math.Abs((targetPos - startPos).y) == 2 && Math.Abs((targetPos - startPos).x) == 1)
            {
                if (chessGrid.Get(targetPos) == null) moveStatus = true;
                if (chessGrid.Get(targetPos) != null) return false;
            }

            return moveStatus;
        }
        public bool BishopAbleMove(Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid)
        {
            bool moveStatus = false;

            if (targetPos == startPos) return moveStatus;
            if (Math.Abs(targetPos.x - startPos.x) != Math.Abs(targetPos.y - startPos.y)) return moveStatus;
            Vector2Int direction = (targetPos - startPos) / Math.Abs((targetPos - startPos).x);
            Vector2Int nowPos = startPos + direction;
            while (nowPos != targetPos)
            {
                if (chessGrid.Get(nowPos) != null) return moveStatus;
                nowPos += direction;
            }
            if (chessGrid.Get(targetPos) == null) moveStatus = true;
            if (chessGrid.Get(targetPos) != null) moveStatus = false;

            return moveStatus;
        }
        public bool QueenAbleMove(Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid)
        {
            bool moveStatus = false;

            if (targetPos == startPos) return moveStatus;
            if ((Math.Abs(targetPos.x - startPos.x) != Math.Abs(targetPos.y - startPos.y)) && (Math.Abs(targetPos.x - startPos.x) > 0 && Math.Abs(targetPos.y - startPos.y) > 0)) return moveStatus;

            Vector2Int direction = (targetPos - startPos) / Math.Max(Math.Abs((targetPos - startPos).x), Math.Abs((targetPos - startPos).y));
            Vector2Int nowPos = startPos + direction;
            while (nowPos != targetPos)
            {
                if (chessGrid.Get(nowPos) != null) return moveStatus;
                nowPos += direction;
            }
            if (chessGrid.Get(targetPos) == null) moveStatus = true;
            if (chessGrid.Get(targetPos) != null) moveStatus = false;

            return moveStatus;
        }
        public bool KingAbleMove(Vector2Int startPos, Vector2Int targetPos, ChessGrid chessGrid)
        {
            bool moveStatus = false;

            if (targetPos == startPos) return moveStatus;

            if (Math.Abs(targetPos.x - startPos.x) > 1 || Math.Abs(targetPos.y - startPos.y) > 1) return moveStatus;

            if (chessGrid.Get(targetPos) == null) moveStatus = true;
            if (chessGrid.Get(targetPos) != null) moveStatus = false;

            return moveStatus;
        }
    }
}