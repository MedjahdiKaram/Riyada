using System.Collections.Generic;
using ModelsLib;
using Riyada.ViewModel.Interaction;

namespace Riyada
{
    public class ClientView:Client
    {
        public bool IsSelected { get; set; }

        public ClientView(Client client)
        {                            
          Id =client.Id           ;
          Name           =client.Name         ;
          LastName       =client.LastName     ;
          Sex            =client.Sex          ;
          DateOfBirth    =client.DateOfBirth  ;
          IdentityId     =client.IdentityId   ;
          Phone          =client.Phone        ;
          TutorFullName  =client.TutorFullName;
          TutorPhone = client.TutorPhone;
          QrCodeImagePath = client.QrCodeImagePath;
        }
      
    }

 
}
