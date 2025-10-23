namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForDetailDTO
    {
        public PurchaseForDetailDTO(int id, decimal totalPrice, DateTime date, string street, string city, string postalCode, string nameSurname, string state, string paymentMethod, string customerUserName, IList<PurchaseItemDTO> items)
        {
            Id = id;
            TotalPrice = totalPrice;
            Date = date;
            Street = street;
            City = city;
            PostalCode = postalCode;
            NameSurname = nameSurname;
            State = state;
            PaymentMethod = paymentMethod;
            CustomerUserName = customerUserName;
            Items = items;
        }

        public int Id { get; set; }
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }

        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string NameSurname { get; set; } = default!;

        public string State { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;
        public string CustomerUserName { get; set; } = default!;
        public IList<PurchaseItemDTO> Items { get; set; } = new List<PurchaseItemDTO>();

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForDetailDTO dTO &&
                   Id == dTO.Id &&
                   TotalPrice == dTO.TotalPrice &&
                   Date == dTO.Date &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   PostalCode == dTO.PostalCode &&
                   NameSurname == dTO.NameSurname &&
                   State == dTO.State &&
                   PaymentMethod == dTO.PaymentMethod &&
                   CustomerUserName == dTO.CustomerUserName &&
                   EqualityComparer<IList<PurchaseItemDTO>>.Default.Equals(Items, dTO.Items);
        }
    }
}
