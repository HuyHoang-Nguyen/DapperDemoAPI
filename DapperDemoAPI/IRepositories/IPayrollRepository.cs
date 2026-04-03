namespace DapperDemoAPI.IRepositories
{
    public interface IPayrollRepository
    {
        Task<int> InsertPayrollAsync(int month, int year);
    }
}
