using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Basement.BLFramework.Essential.Reward.Result
{
    public interface IRewardResult : ISerialize, IHasContext
    {
        string type { get; }
    }
}