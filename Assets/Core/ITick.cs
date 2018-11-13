namespace Core
{
    public abstract class ITick
    {

        public int SomeValue;
        public abstract void OneShot();
        public abstract void OnDisable();

    }
}
