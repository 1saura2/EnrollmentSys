// partialclass.cs
using Microsoft.AspNetCore.Mvc;


namespace ABKS_project.Models.MetaData
{
   
    [ModelMetadataType(typeof(CredentialMetaData))]
    public partial class ValidCredential : Credential
    {
      
    }  

}
