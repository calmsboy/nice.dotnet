using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nice.Dotnet.Application.IServices;
using Nice.Dotnet.Domain.Entities;

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
            return new ObjectResult(await _customInfoService.GetAllAsync());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddEntities([FromBody] List<CustomInfoModel> models)
        {
            await _customInfoService.AddAsync(models);
            return new ObjectResult(models);
        }

    }
}
