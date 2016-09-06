using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Data.EntityModel;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using ComputerShop.Models;

namespace ComputerShop.DAO
{
    public class DeliveryDAO
    {
        private ComputerShopDBEntities _entities = new ComputerShopDBEntities();

        public IEnumerable<delivery> getDelivery(int id)
        {
            int idOrder = (from c in _entities.order where c.userorder_id == id select c.order_id).FirstOrDefault();
            return (from c in _entities.delivery where c.orderdelivery_id == idOrder select c);
        }

        public int getDeliveryForCreate(int id)
        {
            return (from c in _entities.order where c.userorder_id == id select c.order_id).FirstOrDefault();
        }
        public bool deleteDelivery(int id)
        {
            int idDel= (from c in _entities.delivery where c.orderdelivery_id == id select c.delivery_id).FirstOrDefault();
            try
            {
                delivery delivery = _entities.delivery.Find(idDel);
                _entities.delivery.Remove(delivery);
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool confirmToDelivery(int id)
        {
            int idDel = (from c in _entities.delivery where c.orderdelivery_id == id select c.delivery_id).FirstOrDefault();
            try
            {
                delivery delivery = _entities.delivery.Find(idDel);
                delivery.delivery_status = "отправлен";
                _entities.Entry(delivery).State = EntityState.Modified;
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
       
    }
}