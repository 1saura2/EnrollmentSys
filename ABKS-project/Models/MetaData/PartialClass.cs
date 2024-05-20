// partialclass.cs
using Microsoft.AspNetCore.Mvc;


namespace ABKS_project.Models.MetaData
{
   
    [ModelMetadataType(typeof(CredentialMetaData))]
    public partial class ValidCredential : Credential
    {
      
    }  
/*    
    [ModelMetadataType(typeof(CartDetailMetaData))]
    public partial class ValidCartDetail : CartDetail
    {
      
    }


    [ModelMetadataType(typeof(OrderMetaData))]
    public partial class ValidOrder : Order
    {

    }

    [ModelMetadataType(typeof(OrderDetailMetaData))]
    public partial class ValidOrderDetail : OrderDetail
    {

    }


    [ModelMetadataType(typeof(OrderStatusMetaData))]
    public partial class ValidOrderStatus : OrderStatus
    {

    }

    [ModelMetadataType(typeof(ProductMetaData))]
    public partial class ValidProduct : Product
    {

    }

    [ModelMetadataType(typeof(ProductCategoryMetaData))]
    public partial class ValidProductCategory : ProductCategory
    {

    }

    [ModelMetadataType(typeof(ShoppingCartMetaData))]
    public partial class ValidShoppingCart : ShoppingCart
    {

    }
    
    [ModelMetadataType(typeof(StockMetaData))]
    public partial class ValidStock : Stock
    {

    }*/
}
