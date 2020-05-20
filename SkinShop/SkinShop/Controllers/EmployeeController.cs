using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.DL.Repositories.SkinShop;
using SkinShop.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Controllers
{
    [Authorize(Roles = "manager")]
    public class EmployeeController : Controller
    {
        IEmployeeService _employeeService = new EmployeeService(new UnitOfWork("DefaultConnection"));
        IAdminService _adminService = new AdminService(new UnitOfWork("DefaultConnection"));
        MappersForDM _mappers = new MappersForDM();

        public EmployeeController() { }

        public ActionResult ConfirmOrder(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            _employeeService.ConfirmOrder(id, User.Identity.Name);
            return RedirectToAction("Orders");
        }

        public ActionResult Reject(int? id, string text)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            _employeeService.Reject(id, User.Identity.Name, text);
            return RedirectToAction("Orders");
        }

        public ActionResult DeleteOrder(int id)
        {
            if (id == 0)
                return RedirectToAction("PageNotFound", "Home");
            OperationDetails result = _employeeService.DeleteOrder(id);
            if (result.Succedeed)
                return RedirectToAction("Orders", "Client");
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult ReestablishOrder(int id)
        {
            if (id == 0)
                return RedirectToAction("PageNotFound", "Home");
            OperationDetails result = _employeeService.ReestablishOrder(id);
            if (result.Succedeed)
                return RedirectToAction("Orders", "Client");
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult Pay(int orderId)
        {
            if (orderId == 0)
                return RedirectToAction("PageNotFound", "Home");
            OperationDetails result = _employeeService.Pay(orderId);
            if (result.Succedeed)
                return RedirectToAction("Orders", "Client");
            return RedirectToAction("PageNotFound", "Home");
        }
    }
}