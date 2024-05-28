using Grpc.Core;
using Nice.Dotnet.gRPC.Protos;

namespace Nice.Dotnet.gRPC;

public class NiceCurrentService : NiceService.NiceServiceBase
{
  private readonly ILogger _logger;
  public NiceCurrentService(ILogger<NiceCurrentService> logger)
  {
    _logger = logger;
  }

  public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
  {
    var response = new HelloReply() { Message = $"Hello {request.Name},{DateTime.Now:yyyy/MM/dd HH:mm:ss}" };
    _logger.LogInformation(response.ToString());
    await Task.Delay(1);
    return response;
  }
}
