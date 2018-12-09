using System;
using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.essential.path;
using Random = Assets.logic.essential.random.Random;

namespace Assets.logic.essential.amount
{
    public class RleSetAmount : Amount
    {
        private readonly IList<int[]> list = new List<int[]>();
        private readonly int length;
        private readonly Random random;

        public RleSetAmount(RawNode node, IContext context)
            : base(node, context)
        {
            random = PathUtil.ModelsPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
            var rows = node.GetNode("elements").array;

            foreach (var obj in rows)
            {
                var pair = (List<object>) obj;
                var arr = new [] { Convert.ToInt32(pair[0]), Convert.ToInt32(pair[1]) };
                list.Add(arr);
            }

            foreach (var pair in list)
                length += pair[1];
        }
        public override int Number()
        {
            int pos = random.Range(0, length);
            int currentPos = 0;
            foreach (int[] pair in list)
            {
                if (pos >= currentPos && pos < currentPos + pair[1])
                    return pair[0];
                currentPos += pair[1];
            }

            throw new Exception("RleSetAmount::Number() error");
        }
    }
}