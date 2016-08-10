using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touha.Infrastructure.UtilsTests
{
    public class XmlOrder
    {
        #region private Fields
        private Guid _orderID;

        private DateTime _orderDate;

        private XmlProduct _product;

        private int _quantity;
        #endregion

        #region Constructors
        public XmlOrder()
        {
            this._orderID = new Guid();
            this._orderDate = DateTime.MinValue;
            this._quantity = int.MinValue;
        }

        public XmlOrder(Guid id, DateTime orderDate, XmlProduct product, int quantity)
        {
            this._orderID = id;
            this._orderDate = orderDate;
            this._product = product;
            this._quantity = quantity;
        }
        #endregion

        #region Properties
        public Guid OrderID
        {
            get { return this._orderID; }
            set { this._orderID = value; }
        }

        public DateTime OrderDate
        {
            get { return this._orderDate; }
            set { this._orderDate = value; }
        }

        public XmlProduct Product
        {
            get { return this._product; }
            set { this._product = value; }
        }

        public int Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ID: {0}\nDate:{1}\nProduct:\n\tID:{2}\n\tName:{3}\n\tProducing Area:{4}\n\tPrice:{5}\nQuantity:{6}",
                this._orderID, this._orderDate, this._product.ProductID, this._product.ProductName, this._product.ProducingArea, this._product.UnitPrice, this._quantity);
        }
    }
}
