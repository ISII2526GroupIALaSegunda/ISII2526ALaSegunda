using AppForSEII2526.Web.API;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.Web
{
    public class DeleveryAssignmentStateContainer
    {
        public DeliveryAssignmentForCreateDTO DeliveryAssgnment { get; private set; } = new DeliveryAssignmentForCreateDTO { PurchaseDeliveries = new List<PurchaseDeliveryDTO>() };
        
        public decimal TotalPrice 
        { 
            get 
            {
                return Convert.ToDecimal(DeliveryAssgnment.PurchaseDeliveries.Sum(pd => pd.TotalPrice));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddPurchaseOrderToDeliveryAssignment(PurchaseOrderForDeliveryDTO purchaseOrder)
        {
            //before adding a movie we checked whether it has been already added
            if (!DeliveryAssgnment.PurchaseDeliveries.Any(pd => pd.PurchaseOrderId == purchaseOrder.Id))
                //we add it if it is not in the list
                DeliveryAssgnment.PurchaseDeliveries.Add(new PurchaseDeliveryDTO()
                {
                    Date = purchaseOrder.Date,
                    Street = purchaseOrder.Street,
                    City = purchaseOrder.City,
                    PostalCode = purchaseOrder.PostalCode,
                    TotalPrice = purchaseOrder.TotalPrice,
                    PurchaseOrderId = purchaseOrder.Id,

                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemovePurchaseDeliveryToDelivery(PurchaseDeliveryDTO purchaseDelivery)
        {
            DeliveryAssgnment.PurchaseDeliveries.Remove(purchaseDelivery);

        }

        //we eliminate all the movies from the list
        public void ClearDeliveryList()
        {
            DeliveryAssgnment.PurchaseDeliveries.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void RentalProcessed()
        {
            //we have finished the rental process so we create a new object without data
            DeliveryAssgnment = new DeliveryAssignmentForCreateDTO()
            {
                PurchaseDeliveries = new List<PurchaseDeliveryDTO>()
            };
        }
    }
}
    }
}
