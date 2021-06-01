using ClassManagement.Models;
using ClassManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        readonly ClassesService classesService;
        public ClassesController(ClassesService classesService)
        {
            this.classesService = classesService;
        }

        [HttpGet("all-classes")]
        public IActionResult GetAllClasses()
        {
            return Ok(classesService.GetAllClasses());
        }

        [HttpGet("all-class-codes")]
        public async Task<IActionResult> GetAllClassCodes()
        {
            var res = await classesService.GetAllClassCodes();
            return Ok(res);
        }

        [HttpGet("class/{ClassId}")]
        public async Task<IActionResult> GetClassAsync(string ClassId)
        {
            var res = await classesService.GetClassAsync(ClassId);
            return Ok(res);
        }

        [HttpGet("class-from-day/{Day:DayOfWeek}")]
        public IActionResult GetClassesFromDay(DayOfWeek Day)
        {
            var res = classesService.GetClassesFromDay(Day);
            return Ok(res.Classes);
        }
    }
}
