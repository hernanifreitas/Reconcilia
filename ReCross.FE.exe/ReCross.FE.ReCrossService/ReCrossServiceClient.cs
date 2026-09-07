using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ReCross.FE.ReCrossService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ReCrossServiceClient : ClientBase<IReCrossService>, IReCrossService
{
	public ReCrossServiceClient()
	{
	}

	public ReCrossServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ReCrossServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ReCrossServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ReCrossServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Customer GetCustomerByID(string customerID)
	{
		return base.Channel.GetCustomerByID(customerID);
	}
}
