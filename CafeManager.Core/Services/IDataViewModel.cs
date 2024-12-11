namespace CafeManager.Core.Services
{
    public interface IDataViewModel
    {
        Task LoadData(CancellationToken token = default);
    }
}