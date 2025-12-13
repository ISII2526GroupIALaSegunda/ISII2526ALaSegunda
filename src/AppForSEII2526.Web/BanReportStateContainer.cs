using AppForSEII2526.Web.API;


namespace AppForSEII2526.Web
{
    public class BanReportStateContainer
    {
        public BanReportForCreateDTO BanReport { get; private set; } = new BanReportForCreateDTO()
        {
            Customers = new List<ReportCustomerForCreateDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddCustomer(string customerId, string message)
        {
            if (!BanReport.Customers.Any(c => c.CustomerId == customerId))
            {
                BanReport.Customers.Add(new ReportCustomerForCreateDTO
                {
                    CustomerId = customerId,
                    Message = message
                });

                NotifyStateChanged();
            }
        }

        public void RemoveCustomer(ReportCustomerForCreateDTO customer)
        {
            BanReport.Customers.Remove(customer);
            NotifyStateChanged();
        }

        public void ClearCustomers()
        {
            BanReport.Customers.Clear();
            NotifyStateChanged();
        }

        public void BanReportProcessed()
        {
            BanReport = new BanReportForCreateDTO()
            {
                Customers = new List<ReportCustomerForCreateDTO>()
            };

            NotifyStateChanged();
        }
    }
}