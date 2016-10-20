namespace GrabCaster.Framework.EmbeddedTrigger
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    /// <summary>
    /// The nop trigger.
    /// </summary>
    [TriggerContract("{843008B6-F4E1-4A29-8082-BDC111EA0E99}", "Event Viewer Trigger", "Intercept Event Viewer Message",
        false, true, false)]
    public class EmbeddedTrigger : ITriggerType
    {
        [TriggerPropertyContract("Syncronous", "Define if the action between the trigger and the remote event needs to be syncronous")]
        public bool Syncronous { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Main data context ")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{25F85716-1154-4473-AFFE-F8F4E8AC17A9}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {

            actionTrigger(this, context);
            return null;
        }
    }
}