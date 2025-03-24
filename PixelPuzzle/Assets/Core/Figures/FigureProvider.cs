using System.Collections.Generic;
using UnityEngine;

namespace PixelPuzzle
{
    public class FigureProvider
    {
        private Dictionary<FigureTypes, Vector2Int[]> _figures;

        public FigureProvider()
        {
            MakeFigures();
        }

        public Vector2Int[] Get(FigureTypes type)
        {
            if (_figures.TryGetValue(type, out var f))
            {
                return f;
            }
            else
            {
                return null;
            }
        }

        public Vector2Int[] GetRandomFigure()
        {
            var rnd = Random.Range(0, _figures.Count);
            return Get((FigureTypes)rnd);
        }

        private void MakeFigures()
        {
            _figures = new(18);

            //O
            var o = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1)
            };

            //I_H
            var i_h = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(3, 0)
            };

            //I_V
            var i_v = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(0, 3)
            };

            //T_L
            var t_l = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 1)
            };

            //T_R
            var t_r = new Vector2Int[4]
            {
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),
                new Vector2Int(0, 1)
            };

            //T_U
            var t_u = new Vector2Int[4]
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(1, 0)
            };

            //T_D
            var t_d = new Vector2Int[4]
                {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(1, 1)
            };

            //Z_H
            var z_h = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(2, 1)
            };

            //Z_V
            var z_v = new Vector2Int[4]
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1)
            };

            //ZF_H
            var zf_h = new Vector2Int[4]
            {
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1)
            };

            //ZF_V
            var zf_v = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2)
            };

            //L1
            var l1 = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 0)
            };

            //L2
            var l2 = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(2, 1)
            };

            //L3
            var l3 = new Vector2Int[4]
            {
                new Vector2Int(0, 2),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2)
            };

            //L4
            var l4 = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(2, 1)
            };

            //J1
            var j1 = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2)
            };

            //J2
            var j2 = new Vector2Int[4]
            {
                new Vector2Int(2, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(2, 1)
            };

            //J3
            var j3 = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 2),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2)
            };

            //J4
            var j4 = new Vector2Int[4]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(0, 1)
            };

            _figures.Add(FigureTypes.O, o);

            _figures.Add(FigureTypes.I_H, i_h);
            _figures.Add(FigureTypes.I_V, i_v);

            _figures.Add(FigureTypes.T_L, t_l);
            _figures.Add(FigureTypes.T_R, t_r);
            _figures.Add(FigureTypes.T_U, t_u);
            _figures.Add(FigureTypes.T_D, t_d);

            _figures.Add(FigureTypes.Z_H, z_h);
            _figures.Add(FigureTypes.Z_V, z_v);
            _figures.Add(FigureTypes.ZF_H, zf_h);
            _figures.Add(FigureTypes.ZF_V, zf_v);

            _figures.Add(FigureTypes.L1, l1);
            _figures.Add(FigureTypes.L2, l2);
            _figures.Add(FigureTypes.L3, l3);
            _figures.Add(FigureTypes.L4, l4);

            _figures.Add(FigureTypes.J1, j1);
            _figures.Add(FigureTypes.J2, j2);
            _figures.Add(FigureTypes.J3, j3);
            _figures.Add(FigureTypes.J4, j4);
        }

    }
}
