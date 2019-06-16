namespace lunge.Library.Gui.Controls
{
    public class ChartNode
    {
        internal ChartNode Next;

        public float Value { get; }

        public float XOffset { get; }

        internal ChartNode(ChartNode next, float value, float xOffset)
        {
            Next = next;
            Value = value;
            XOffset = xOffset;
        }
    }
}