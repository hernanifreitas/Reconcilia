using System.CodeDom.Compiler;
using System.ServiceModel;

namespace ReCross.FE.ReCrossService;

[ServiceContract(ConfigurationName = "ReCrossService.IReCrossService")]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IReCrossService
{
	[OperationContract(Action = "http://tempuri.org/IReCrossService/GetCustomerByID", ReplyAction = "http://tempuri.org/IReCrossService/GetCustomerByIDResponse")]
	Customer GetCustomerByID(string customerID);
}
