using System.Collections.Generic;
using game.models.triggers._base;

namespace game.models.triggers.util
{
    static class TriggerUtil
    {
        public static Trigger[] Decomposite(Trigger trigger, List<Trigger> triggers = null)
        {
            if (triggers == null)
                triggers = new List<Trigger>();

            var ct = trigger as CompositeTrigger;

            if (ct != null)
            {
                foreach (var r in ct.triggers)
                    Decomposite(r.Value, triggers);
            }
            else
                triggers.Add(trigger);

            return triggers.ToArray();
        }
    }
}
