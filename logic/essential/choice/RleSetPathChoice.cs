using System;
using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.throughEvent;
using Assets.logic.essential.path;
using Random = Assets.logic.essential.random.Random;

namespace Assets.logic.essential.choice
{
    public class RleSetPathChoice : PathChoice
    {
        readonly List<object[]> list = new List<object[]>();
        private readonly int length;

        public RleSetPathChoice(RawNode node, IContext context)
            : base(node, context)
        {
            var rows = node.GetNode("elements").array;
            foreach (var obj in rows)
            {
                var l = (List<object>)obj;
                var arr = new [] { l[0], Convert.ToInt32(l[1]) };
                list.Add(arr);
            }

            foreach (var pair in list)
                length += (int)pair[1];
        }

        public override ModelsPath GetPath()
        {
            var rnd = randomPath.GetSelf<Random>();
            int pos = rnd.Range(0, length);
            int currentPos = 0;
            foreach (object[] pair in list)
            {
                if (pos >= currentPos && pos < currentPos + (int)pair[1])
                    return PathUtil.ModelsPath(GetContext(), (string)pair[0], rnd);
                currentPos += (int)pair[1];
            }

            throw new Exception("nothing selected");
        }
    }
}
