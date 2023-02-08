using BookShop.BLL.BasketViewModels;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace BookShop.BLL.Services
{
	public class LayoutService
	{
		private readonly BookDbContext _bookDbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public LayoutService(BookDbContext bookDbContext, IHttpContextAccessor httpContextAccessor)
		{
			_bookDbContext = bookDbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public List<Setting> GetSettings()
		{
			List<Setting> settings = _bookDbContext.Settings.ToList();
			return settings;
		}

		public BasketVM GetBasket()
		{

			string basketStr = _httpContextAccessor.HttpContext.Request.Cookies["Basket"];
			if (!string.IsNullOrEmpty(basketStr))
			{
				BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
				LayoutBasketViewModel layoutBasketViewModel = new LayoutBasketViewModel();
				foreach (BasketCookieItemVM cookie in basket.BasketCookieItemVMs)
				{
					Book existed = _bookDbContext.Books.FirstOrDefault(b => b.Id == cookie.Id);
					if (existed == null)
					{
                        basket.BasketCookieItemVMs.Remove(cookie);
                    }
				
				}
				return basket;
			}
			return null; 
		}
	}
}
