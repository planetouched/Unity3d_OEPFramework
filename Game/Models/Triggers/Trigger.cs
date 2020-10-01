using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;
using Game.Models.Triggers.Associations;
using Game.Models.Triggers.Associations._Base;

namespace Game.Models.Triggers
{
    public class Trigger : ReferenceModelBase<TriggerCategories, TriggerDescription>
    {
        private const string EnabledKey = "enabled";
        private const string ActivatedKey = "activated";
        private const string CompletedKey = "completed";
        private const string AcceptedKey = "accepted";
        private const string AssociationKey = "assoc";

        private const string AcceptedRewardResultKey = "accepted-reward-result";

        public bool enabled { get; private set; }
        public bool activated { get; private set; }
        public bool completed { get; private set; }
        public bool accepted { get; private set; }

        public IRewardResult acceptedRewardResult { get; private set; }
        
        public IAssociation association { get; private set; }

        public Trigger(RawNode initNode, TriggerCategories categories, TriggerDescription description, IContext context) : base(initNode, categories, description, context)
        {
            enabled = initNode.GetBool(EnabledKey);
            activated = initNode.GetBool(ActivatedKey);
            completed = initNode.GetBool(CompletedKey);
            accepted = initNode.GetBool(AcceptedKey);
            acceptedRewardResult = description.acceptable && completed && !accepted ? FactoryManager.Build<RewardResult>(initNode.GetNode(AcceptedRewardResultKey), context) : null;
        }

        public override void Initialization()
        {
            var assocDescriptionNode = description.GetNode().GetNode(AssociationKey);
            var assocInitNode = initNode.GetNode(AssociationKey);

            association = (IAssociation)FactoryManager.Build(
                typeof(Association), 
                assocDescriptionNode.GetString("type"), 
                assocInitNode, 
                assocDescriptionNode, 
                categories.association, 
                GetContext());
            
            AddChild(association);
            
            association.Initialization();
            
            if (activated)
            {
                AttachAssoc();
                OnActivate();
            }
        }

        public void Enable()
        {
            if (enabled) return;
            enabled = true;
            Call(categories.enabled);
        }

        public void Disable()
        {
            if (!enabled) return;
            enabled = false;
            Call(categories.disabled);
        }

        public void Activate()
        {
            if (activated || !enabled || !description.price.Check()) return;
            
            description.price.Pay();
            
            activated = true;
            completed = false;

            association.Activate();
            AttachAssoc();
            OnActivate();
            Call(categories.activated);
        }

        public void Deactivate()
        {
            if (!activated) return;
            activated = false;
            
            DetachAssoc();
            OnDeactivate();
            Call(categories.deactivated);
        }

        public void Complete()
        {
            if (completed) return;

            Deactivate();
            Disable();
            completed = true;

            if (description.acceptable)
            {
                acceptedRewardResult = description.acceptedReward.Calculate();
            }

            Call(categories.completed, new TriggerHandlerArgs {rewardResult = description.reward.Award()});
        }

        public void Accept()
        {
            if (!completed || accepted) return;

            TriggerHandlerArgs args = null;

            if (acceptedRewardResult != null)
            {
                args = new TriggerHandlerArgs {rewardResult = description.acceptedReward.Award(acceptedRewardResult)};
                acceptedRewardResult = null;
            }

            accepted = true;
            Call(categories.accepted, args);
        }

        public override object Serialize()
        {
            var dict = SerializeUtil.Dict().SetArgs(CompletedKey, completed, ActivatedKey, activated, EnabledKey, enabled, AssociationKey, association.Serialize());

            if (description.acceptable)
            {
                dict.Add(AcceptedKey, accepted);
            }

            if (acceptedRewardResult != null)
            {
                dict.Add(AcceptedRewardResultKey, acceptedRewardResult.Serialize());
            }

            return dict;
        }

        private void AttachAssoc()
        {
            association.Connect();
            association.Attach(association.categories.complete, AssocComplete);
        }

        private void DetachAssoc()
        {
            association.Disconnect();
            association.Detach(association.categories.complete, AssocComplete);
        }
        
        private void AssocComplete(CoreParams cp, object args)
        {
            Complete();
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }
    }
}