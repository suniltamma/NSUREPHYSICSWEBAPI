using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Xml.Linq;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace NSurePhysicsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public PackageController(ApplicationDbContext context)
        {
            _context = context;
        }




        //[HttpGet("GetAllPackages")]
        //public async Task<ActionResult<IEnumerable<PackageDto>>> GetAllPackages([FromQuery] int UserID)
        //{
        //    try
        //    {

        //        var packages = await _context.GetAllPackagesByIdAsync(UserID);

        //        // If no packages are returned, return a 404 Not Found
        //        if (packages == null || !packages.Any())
        //        {
        //            return NotFound("No Packages found.");
        //        }

        //        // Return the results with an OK status


        //        return Ok(packages);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return a 500 Internal Server Error with the exception message
        //        return StatusCode(500, $"Error fetching packages: {ex.Message}");
        //    }
        //}


        [HttpGet("GetAllPackages")]
        public async Task<ActionResult> GetAllPackages([FromQuery] int UserID)
        {
            try
            {
                // Call the method to get exams and exam subjects
                var (packages, packageSubjects) = await _context.GetAllPackagesByIdAsync(UserID);

                // Check if exams are found
                if (packages == null || !packages.Any())
                {
                    return NotFound("No Exams found.");
                }

                // Return both exams and exam subjects in the response
                var result = new
                {
                    packages = packages,
                    packageSubjects = packageSubjects
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return StatusCode(500, $"Error fetching Exams: {ex.Message}");
            }
        }





        [HttpPost("save-package")]
        public async Task<IActionResult> SavePackage([FromBody] SavePackageDto packageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var packageSubjects = new List<PackageSubjectdetailsDto>();
                foreach (var subject in packageDto.package_Subject)
                {
                    var deserializedSubject = JsonSerializer.Deserialize<PackageSubjectdetailsDto>(subject);
                    if (deserializedSubject != null)
                    {
                        packageSubjects.Add(deserializedSubject);
                    }
                }

                var packageSubjectsXml = GeneratePackageSubjectsXml(packageSubjects);

                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_SAVE_PACKAGE @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8",
                    packageDto.PackageId, packageDto.PackageName, packageDto.Package_Syllabus_Id, packageDto.CourseTypeId, packageDto.Package_No_Days,
                    packageDto.PackageCost, packageDto.UserId, packageDto.IsActive, packageSubjectsXml
                );

                return Ok(new { message = "Package saved successfully", result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }





        private string GeneratePackageSubjectsXml(List<PackageSubjectdetailsDto> PackageSubjects)
        {
            var xml = new XElement("PACKAGE_SUBJECTS",
                PackageSubjects.Select(subject => new XElement("RESULT",
                    
                    new XAttribute("SUBJECT_ID", subject.SUBJECT_ID),
                    new XAttribute("CREATED_BY", subject.CREATED_BY),
                    new XAttribute("MODIFIED_BY", subject.MODIFIED_BY),
                    new XAttribute("IS_ACTIVE", subject.IS_ACTIVE ? 1 : 0)
                ))
            );
            return xml.ToString();
        }




    }

}
