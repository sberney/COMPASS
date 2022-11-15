namespace COMPASS.Core
{
    public struct LoadedTags<TColor>
  {
    public readonly IList<ITag<TColor>> Root { get; init; }
    public readonly IList<ITag<TColor>> All { get; init; }
  }
}
