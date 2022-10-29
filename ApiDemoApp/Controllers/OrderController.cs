using ApiDemoApp.Models;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderData _orderData;
        private readonly IFoodData _foodData;

        public OrderController(IOrderData orderData, IFoodData foodData)
        {
            _orderData = orderData;
            _foodData = foodData;
        }

        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(OrderModel order)
        {
            var food = await _foodData.GetFood();

            order.Total = order.Quantity * food.Where(x => order.FoodId == x.Id).First().Price;

            int id = await _orderData.CreateOrder(order);

            return Ok(new {Id = id});
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var order = await _orderData.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                var food = await _foodData.GetFood();

                var output = new
                {
                    Order = order,
                    ItemPurchased = food.Where(x => x.Id == order.FoodId).FirstOrDefault()?.Title
                };

                return Ok(output);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] OrderUpdateModel data)
        {
            await _orderData.UpdateOrderName(data.id, data.OrderName);

            return Ok();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderData.DeleteOrder(id);

            return Ok();
        }
    }
}
