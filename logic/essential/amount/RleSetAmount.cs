﻿using System;
using System.Collections.Generic;
using common;
using logic.core.context;
using logic.essential.path;
using Random = logic.essential.random.Random;

namespace logic.essential.amount
{
    public class RleSetAmount : Amount
    {
        private readonly IList<int[]> _list = new List<int[]>();
        private readonly int _length;
        private readonly Random _random;

        public RleSetAmount(RawNode node, IContext context)
            : base(node, context)
        {
            _random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
            var rows = node.GetNode("elements").array;

            foreach (var obj in rows)
            {
                var pair = (List<object>) obj;
                var arr = new [] { Convert.ToInt32(pair[0]), Convert.ToInt32(pair[1]) };
                _list.Add(arr);
            }

            foreach (var pair in _list)
                _length += pair[1];
        }
        public override int Number()
        {
            int pos = _random.Range(0, _length);
            int currentPos = 0;
            foreach (int[] pair in _list)
            {
                if (pos >= currentPos && pos < currentPos + pair[1])
                    return pair[0];
                currentPos += pair[1];
            }

            throw new Exception("RleSetAmount::Number() error");
        }
    }
}