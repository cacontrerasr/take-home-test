namespace Fundo.Applications.WebApi.Data
{
    public static class LoanDbInitializer
    {
        public static void Initialize(LoanDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
