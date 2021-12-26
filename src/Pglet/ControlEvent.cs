namespace Pglet
{
    public class ControlEvent : Event
    {
        public Control Control { get; set; }
        public Page Page { get; set; }

        public override string ToString()
        {
            return $"{this.Target} {this.Name} {this.Data}";
        }
    }
}
