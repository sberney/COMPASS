namespace COMPASS.Core.Tags
{
    public interface IHasMutableChildren<T>
  {
    void AddChild(T child);
    void RemoveChild(T child);
    void ClearChildren();
  }
}