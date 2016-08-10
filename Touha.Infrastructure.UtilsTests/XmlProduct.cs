using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touha.Infrastructure.UtilsTests
{
    public class XmlProduct
    {
        #region private Fields
        private Guid _productID;

        private string _productName;

        private string _producingArea;

        private double _unitPrice;
        #endregion

        #region Constructor
        public XmlProduct()
        {
            Console.WriteLine("the constructor of XmlProduct has been invocated");
        }

        public XmlProduct(Guid id, string name, string producingArea, double price)
        {
            this._productID = id;
            this._productName = name;
            this._producingArea = producingArea;
            this._unitPrice = price;
        }
        #endregion

        #region Properties
        public Guid ProductID
        {
            get { return this._productID; }
            set { this._productID = value; }
        }

        public string ProductName
        {
            get { return this._productName; }
            set { this._productName = value; }
        }

        public string ProducingArea
        {
            get { return this._producingArea; }
            set { this._producingArea = value; }
        }

        public double UnitPrice
        {
            get { return this._unitPrice; }
            set { this._unitPrice = value; }
        }
        #endregion
    }
}
