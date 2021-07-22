using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class BasketAppService : ApplicationService, IBasketAppService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly IEmailService emailService;
        private readonly EmailSettings emailSettings;
        private readonly string htmlContainerFolder;
        private readonly IWebshopFurnitureUnitRepository webshopFurnitureUnitRepository;
        private readonly IWebshopOrderRepository webshopOrderRepository;

        public BasketAppService(IApplicationServiceDependencyAggregate aggregate,
            IBasketRepository basketRepository,
            IEmailDataRepository emailDataRepository,
            IEmailService emailService,
            IOptions<EmailSettings> emailSettings,
            IHostingEnvironment hostingEnvironment,
            IWebshopFurnitureUnitRepository webshopFurnitureUnitRepository,
            IWebshopOrderRepository webshopOrderRepository)
            : base(aggregate)
        {
            this.basketRepository = basketRepository;
            this.emailDataRepository = emailDataRepository;
            this.emailService = emailService;
            this.emailSettings = emailSettings.Value;
            htmlContainerFolder = Path.Combine(hostingEnvironment.ContentRootPath, "AppData");
            this.webshopFurnitureUnitRepository = webshopFurnitureUnitRepository;
            this.webshopOrderRepository = webshopOrderRepository;
        }

        public async Task<int> CreateBasketAsync(BasketCreateDto basketCreateDto)
        {
            //TODO: validate customerid, if there is no such element in db, throw badRequest
            var newBasket = basketCreateDto.CreateModelObject();
            await basketRepository.InsertAsync(newBasket);
            foreach (var orderedFurnitureUnit in basketCreateDto.OrderedFurnitureUnits)
            {
                var newOrderedFurnitureUnit = orderedFurnitureUnit.CreateModelObject();
                newOrderedFurnitureUnit.BasketId = newBasket.Id;

                var wfu = await webshopFurnitureUnitRepository.SingleIncludingAsync(ent => ent.FurnitureUnitId == orderedFurnitureUnit.FurnitureUnitId, ent => ent.Price.Currency);
                var wfuPrice = wfu.Price;
                newOrderedFurnitureUnit.UnitPrice = wfuPrice;
                newOrderedFurnitureUnit.FurnitureUnit = wfu.FurnitureUnit;
                newBasket.AddOrderedFurnitureUnit(newOrderedFurnitureUnit);

                double value = newBasket.SubTotal == null ? value = 0 : value = newBasket.SubTotal.Value;
                newBasket.SubTotal = SetSubTotal(wfuPrice, newOrderedFurnitureUnit.Quantity, value);
            }

            await unitOfWork.SaveChangesAsync();
            return newBasket.Id;
        }

        public async Task DeleteBasketItemAsync(int basketId, Guid furnitureUnitId)
        {
            var basket = await basketRepository.SingleIncludingAsync(ent => ent.Id == basketId, ent => ent.OrderedFurnitureUnits);
            var orderedFurnitureUnit = basket.OrderedFurnitureUnits.Single(ent => ent.FurnitureUnitId == furnitureUnitId);
            basket.RemoveOrderedFurnitureUnit(orderedFurnitureUnit);

            var wfu = await webshopFurnitureUnitRepository.SingleIncludingAsync(ent => ent.FurnitureUnitId == furnitureUnitId, ent => ent.Price.Currency);
            basket.SetSubTotalAfterDeleteItem(wfu.Price, orderedFurnitureUnit);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<BasketDetailsDto> GetBasketDetailsAsync(int id)
        {
            if (await basketRepository.GetBasketAsync(id) == null) return null;

            var basketDeatils = new BasketDetailsDto(await basketRepository.GetBasketAsync(id));
            foreach (var ofu in basketDeatils.OrderedFurnitureUnits)
            {
                var wfu = await webshopFurnitureUnitRepository.SingleIncludingAsync(ent => ent.FurnitureUnitId == ofu.FurnitureUnitId, ent => ent.Price, ent => ent.FurnitureUnit.Image, ent => ent.FurnitureUnit.Category);
                ofu.WebshopFurnitureUnitListDto = new WebshopFurnitureUnitByWebshopListDto(wfu);
            }

            return basketDeatils;
        }

        public async Task PurchaseBasketAsync(int id, BasketPurchaseDto basketPurchaseDto)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            basketPurchaseDto.UpdateModelObject(basket);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ConfirmPurchase);

            string contents = System.IO.File.ReadAllText(Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.FileName));

            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(contents, basketPurchaseDto.Name, basketPurchaseDto.Name, basketPurchaseDto.Name, basketPurchaseDto.Name);
            await emailService.SendEmailAsync(
                user: basket.Customer.User,
                subject: emailSettings.ResetSubject,
                builder.ToMessageBody(),
                emailData.Id);


            var webshopOrder = new WebshopOrder("WebshopOrder", basketPurchaseDto.CustomerId, basketPurchaseDto.DelieveryAddress.CreateModelObject()) { BasketId = basket.Id };
            await webshopOrderRepository.InsertAsync(webshopOrder);
            webshopOrder.AddOrderedFurnitureUnits(basket.OrderedFurnitureUnits.ToList());
            webshopOrder.AddService(new WebshopOrderService() { ServiceId = basket.ServiceId, WebshopOrderId = webshopOrder.Id });
            basket.CustomerId = null;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBasketAsync(int id, BasketUpdateDto basketUpdateDto, bool isUpdate)
        {
            var basket = await basketRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.OrderedFurnitureUnits, ent => ent.SubTotal);
            var differenceQuantity = 0;
            double value = 0;
            foreach (var orderedFurnitureUnit in basketUpdateDto.OrderedFurnitureUnits)
            {
                var wfu = await webshopFurnitureUnitRepository.SingleIncludingAsync(ent => ent.FurnitureUnitId == orderedFurnitureUnit.FurnitureUnitId, ent => ent.Price.Currency);

                if (basket.OrderedFurnitureUnits.Any(ent => ent.FurnitureUnitId == orderedFurnitureUnit.FurnitureUnitId))
                {
                    var updatedOrderedFurnitureUnit = basket.OrderedFurnitureUnits.Single(ent => ent.FurnitureUnitId == orderedFurnitureUnit.FurnitureUnitId);
                    if (isUpdate)
                    {
                        updatedOrderedFurnitureUnit.Quantity += orderedFurnitureUnit.Quantity;
                        value = basket.SubTotal == null ? 0 : basket.SubTotal.Value;
                        basket.SubTotal = SetSubTotal(wfu.Price, orderedFurnitureUnit.Quantity, value);
                    }
                    else
                    {
                        if (basketUpdateDto.OrderedFurnitureUnits.Count() == 1 && updatedOrderedFurnitureUnit.Quantity != orderedFurnitureUnit.Quantity)
                        {
                            basket.SubTotal = null;
                            basket.SubTotal = SetSubTotal(wfu.Price, orderedFurnitureUnit.Quantity, value);
                            updatedOrderedFurnitureUnit.Quantity = orderedFurnitureUnit.Quantity;
                        }
                        if (updatedOrderedFurnitureUnit.Quantity != orderedFurnitureUnit.Quantity)
                        {
                            differenceQuantity = orderedFurnitureUnit.Quantity - updatedOrderedFurnitureUnit.Quantity;
                            updatedOrderedFurnitureUnit.Quantity = orderedFurnitureUnit.Quantity;
                            value = basket.SubTotal == null ? 0 : basket.SubTotal.Value;
                            basket.SubTotal = SetSubTotal(wfu.Price, differenceQuantity, value);
                        }
                    }
                }
                else
                {
                    var newOrderedFurnitureUnit = new OrderedFurnitureUnit(orderedFurnitureUnit.FurnitureUnitId, orderedFurnitureUnit.Quantity) { BasketId = basket.Id };
                    var wfuPrice = wfu.Price;
                    newOrderedFurnitureUnit.UnitPrice = wfuPrice;
                    newOrderedFurnitureUnit.FurnitureUnit = wfu.FurnitureUnit;
                    basket.AddOrderedFurnitureUnit(newOrderedFurnitureUnit);

                    value = basket.SubTotal == null ? value = 0 : value = basket.SubTotal.Value;
                    basket.SubTotal = SetSubTotal(wfuPrice, newOrderedFurnitureUnit.Quantity, value);
                }
            }

            basketUpdateDto.UpdateModelObject(basket);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> SynchronizeBasketsAsync(int basketId, int otherBasketId)
        {
            var firstBasket = await basketRepository.SingleOrDefaultIncludingAsync(ent => ent.Id == basketId, ent => ent.OrderedFurnitureUnits);
            var secondBasket = await basketRepository.SingleOrDefaultIncludingAsync(ent => ent.Id == otherBasketId, ent => ent.OrderedFurnitureUnits);
            if (firstBasket == null)
            {
                return secondBasket != null ? secondBasket.Id : 0;
            }
            if (secondBasket == null)
            {
                return firstBasket.Id;
            }
            if (DateTime.Compare(firstBasket.CreationTime, secondBasket.CreationTime) < 0)
            {
                foreach (var ofu in secondBasket.OrderedFurnitureUnits)
                {
                    firstBasket.AddOrderedFurnitureUnit(ofu);
                }
                await basketRepository.DeleteAsync(secondBasket.Id);
            }
            await unitOfWork.SaveChangesAsync();
            return firstBasket.Id;
        }

        private Price SetSubTotal(Price wfuPrice, int quantity, double value)
        {
            var currency = wfuPrice.CurrencyId;
            value += wfuPrice.Value * quantity;
            return new Price(value, currency);
        }
    }
}