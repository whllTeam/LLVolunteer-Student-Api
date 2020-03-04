using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManager.Core.Entities.ViewModel;
using SchoolManager.Core.Interfaces;

namespace SchoolManager.API.Controllers
{
    [Route("api/SchoolUser")]
    [ApiController]
    public class SchoolUserController : ControllerBase
    {
        public SchoolUserController(ISchoolInfoService service)
        {
            SchoolInfoService = service;
        }
        public ISchoolInfoService SchoolInfoService { get; }
        [HttpPost]
        public async Task<IActionResult> AuthorizeUserInfo([FromBody] UserLoginRequest request)
        {
            var data = await SchoolInfoService.StudentLogin(request);
            return Ok(data);
        }
        [HttpGet("image")]
        public async Task<IActionResult> GetValidateCode()
        {
            var model = await SchoolInfoService.GetValidateImage();
            // 自定义  header  需要  在 cors  中 添加
            if (model.Data == null)
            {
                return BadRequest("内部错误");
            }
            Response.Headers.Add("PreUrl", model.Data?.PreUrl);
            Response.Headers.Add("LoginUrl", model.Data?.LoginUrl);
            Response.Headers.Add("LoginViewState", model.Data?.LoginViewState);
            return File(model.Data?.ImageStream, "image/jpeg");
        }
        [HttpGet("base64")]
        public async Task<IActionResult> GetValidateCodeWithBase()
        {
            var model = await SchoolInfoService.GetValidateImageBase64();
            // 自定义  header  需要  在 cors  中 添加
            if (model.Data == null)
            {
                return BadRequest("内部错误");
            }
            Response.Headers.Add("PreUrl", model.Data.PreUrl);
            Response.Headers.Add("LoginUrl", model.Data.LoginUrl);
            Response.Headers.Add("LoginViewState", model.Data.LoginViewState);
            return Ok(model.Data.ImageBase64);
        }
        [Route("userInfo/{uid}")]
        public async Task<IActionResult> GetUserInfo([FromRoute]string uid,[FromQuery] string userName)
        {
            var model = await SchoolInfoService.GetLocalUserInfo(uid, userName);
            return Ok(model);
        }
    }
}