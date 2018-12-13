namespace lunge.Library.Screens
{
    public interface IScreenManager
    {
        T FindScreen<T>() where T : Screen;
    }
}