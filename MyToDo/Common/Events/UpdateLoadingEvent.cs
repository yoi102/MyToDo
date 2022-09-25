using Prism.Events;

namespace MyToDo.Common.Events
{
    public class UpdateModel
    {
        public bool IsOpen { get; set; }
    }

    public class UpdateLoadingEvent : PubSubEvent<UpdateModel>
    {
    }
}