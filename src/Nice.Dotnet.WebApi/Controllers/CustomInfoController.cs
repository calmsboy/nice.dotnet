using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nice.Dotnet.Application.IServices;
using Nice.Dotnet.Domain.Entities;
using Serilog;

namespace Nice.Dotnet.WebApi.Controllers
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomInfoController : ControllerBase
    {
        private readonly ICustomInfoService _customInfoService;

        public CustomInfoController(ICustomInfoService customInfoService)
        {
            _customInfoService = customInfoService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("list")]
        public async Task<IActionResult> GetEntities()
        {
            Log.Information($"{HttpContext.Connection.RemoteIpAddress}:获取数据");
            return new ObjectResult(await _customInfoService.GetAllAsync());
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> ClearData()
        {
            Log.Information($"{HttpContext.Connection.RemoteIpAddress}:清除了数据");
            await _customInfoService.ClearData();
            return new ObjectResult(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddEntities([FromBody] List<CustomInfoModel> models)
        {
            Log.Information($"{HttpContext.Connection.RemoteIpAddress}:提交的数据：{JsonConvert.SerializeObject(models)}");
            var result= await _customInfoService.AddAsync(models);
            return new ObjectResult(result);
        }

    }
}
