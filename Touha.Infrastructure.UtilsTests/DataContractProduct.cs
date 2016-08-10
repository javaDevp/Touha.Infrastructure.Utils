using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Touha.Infrastructure.UtilsTests
{
    [DataContract(Name="product", Namespace="ohohohoh")]
    class DataContractProduct
    {
        #region private Fields
        private Guid _productID;

        private string _productName;

        private string _producingArea;

        private double _unitPrice;
        #endregion

        #region Constructors
        public DataContractProduct()
        {
            Console.WriteLine("the constructor of DataContractProduct has been invocated");
        }

        public DataContractProduct(Guid id, string name, string producingArea, double price)
        {
            this._productID = id;
            this._productName = name;
            this._producingArea = producingArea;
            this._unitPrice = price;
        }
        #endregion

        #region Properties
        [DataMember(Name="id", Order=1)]
        public Guid ProductID
        {
            get { return this._productID; }
            set { this._productID = value; }
        }

        [DataMember(Name = "name", Order = 2)]
        public string ProductName
        {
            get { return this._productName; }
            set { this._productName = value; }
        }

        [DataMember(Name = "producingArea", Order = 3)]
        public string ProducingArea
        {
            get { return this._producingArea; }
            set { this._producingArea = value; }
        }

        [DataMember(Name = "price", Order = 4)]
        public double UnitPrice
        {
            get { return this._unitPrice; }
            set { this._unitPrice = value; }
        }
        #endregion
    }
}
