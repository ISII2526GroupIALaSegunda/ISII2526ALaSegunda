namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{
    public class ReturnSummaryDTO
    {
        public ReturnSummaryDTO(
            
            
            
 
            string customerName,
            string customerSurname,
            string customerAddress,
            string customerPhoneNumber,
            List<ReturnedProductInfoDTO> returnedProducts
        )
        {
       

            CustomerName = customerName;
            CustomerSurname = customerSurname;
            CustomerAddress = customerAddress;
            CustomerPhoneNumber = customerPhoneNumber;

            ReturnedProducts = returnedProducts;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PaymentMethod { get; set; }
        public string Reason { get; set; }
        public int? Rating { get; set; }
        public double MoneyToReturn { get; set; }

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public List<ReturnedProductInfoDTO> ReturnedProducts { get; set; }
    }
}
