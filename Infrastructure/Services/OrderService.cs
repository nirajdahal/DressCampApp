using Core.Entities.Order;
using Core.Entities.Product;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {

        private readonly IBasketRepository _basketRepo;


        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }


        public async Task<Order> CreateOrderAsync(string buyerEmail, int delieveryMethodId, string basketId, Address shippingAddress)
        {
            // basket from basket repo
            var basket = await _basketRepo.GetBasketAsync(basketId);
            //get items for product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productSpec = new ProductWithTypeBrandPictureSpecification(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetEntityWithSpecs(productSpec);
                var itemOrdered = new ProductItemOrdered(productItem.Name, productItem.Id, productItem.Picture[0].PictureUrl);
                var orderItem = new OrderItem(itemOrdered, Convert.ToDecimal(productItem.Price), item.Quantity);
                items.Add(orderItem);
            }


            //get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(delieveryMethodId);
            //calculate subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            //create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
            _unitOfWork.Repository<Order>().Add(order);
            await _unitOfWork.Complete();
            //save to repo
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpecs(spec);

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);

        }
    }
}