namespace COMPASS.ViewModels
{
  public class TagsFiltersViewModel : ViewModelBase
  {
    public TagsFiltersViewModel() : base()
    {
      TagsTabVM = new();
      FiltersTabVM = new();
    }

    private TagsTabViewModel _tagsTabVM;
    public TagsTabViewModel TagsTabVM
    {
      get => _tagsTabVM;
      set => SetProperty(ref _tagsTabVM, value);
    }

    private FiltersTabViewModel _filtersTabVM;
    public FiltersTabViewModel FiltersTabVM
    {
      get => _filtersTabVM;
      set => SetProperty(ref _filtersTabVM, value);
    }

  }
}
