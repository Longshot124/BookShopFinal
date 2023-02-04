using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		//public BasketVM
	}
}
