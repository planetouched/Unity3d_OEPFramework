using System.Collections.Generic;
using Assets.game.model.triggers._base;

namespace Assets.game.model.triggers.util
{
    static class TriggerUtil
    {
        public static Trigger[] Decomposite(Trigger tr, List<Trigger> triggers = null)
        {
            if (triggers == null)
                triggers = new List<Trigger>();

            if (tr is CompositeTrigger cr)
            {
                foreach (var r in cr.triggers)
                    Decomposite(r.Value, triggers);
            }
            else
                triggers.Add(tr);

            return triggers.ToArray();
        }
    }
}
