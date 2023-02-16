using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin,User,SystemAdmin")]
    [EnableCors("CorePolicy")]
    public class OrganizationController : ControllerBase
    {
        private readonly IorganizationServices _services;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;

        public OrganizationController(IorganizationServices services, IMapper mapper , IEmailSender emailSender)
        {
            _services = services;
            this.mapper = mapper;
            this.emailSender = emailSender;
        }

        [HttpGet]
        [Route("ViewData")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Entities.Organization>>> Get()
        {
            var Organization = await _services.GetOrganizationData();

            if (Organization == null)
            {
                return NotFound();
            }

            var OrganizationRequest = new List<organizationdataRequest>();

            Organization.ToList().ForEach(Organization =>
            {
                var organizationRequest = new organizationdataRequest()
                {
                    OrganizationName = Organization.OrganizationName,
                    EmailAddress = Organization.EmailAddress,
                    PhoneNumber = Organization.PhoneNumber,
                    CreatedDate = Organization.CreatedDate,
                };
                OrganizationRequest.Add(organizationRequest);

            });

            return Ok(OrganizationRequest);

        }


        [HttpGet]
        [Route("Details")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Entities.Organization>>> GetDetails()
        {
            var Organization = await _services.GetOrganizationData();

            if (Organization == null)
            {
                return NotFound();
            }

            var OrganizationRequest = new List<OrganizationDetailsRequest>();

            Organization.ToList().ForEach(Organization =>
            {
                var organizationRequest = new OrganizationDetailsRequest()
                {
                    OrganizationName = Organization.OrganizationName,
                    EmailAddress = Organization.EmailAddress,
                    PhoneNumber = Organization.PhoneNumber,
                    CreatedDate = Organization.CreatedDate,
                    HouseBuildingNumber = Organization.HouseBuildingNumber,
                    CityTown = Organization.CityTown,
                    LastModifiedDate = Organization.LastModifiedDate,
                    Adminemailaddress = Organization.Adminemailaddress,
                    StateProvince = Organization.StateProvince,
                    StreetLandmark = Organization.StreetLandmark,
                    Country = Organization.Country,
                    Adminfullname = Organization.Adminfullname,
                    OrganizationLogo = Organization.OrganizationLogo,
                    Pincode = Organization.Pincode,
                };
                OrganizationRequest.Add(organizationRequest);

            });

            return Ok(OrganizationRequest);

        }
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> GetOrganizationById(Guid id)
        {
            var Organization = await _services.GetOrgByID(id);

            if (Organization == null)
            {
                return NotFound();
            }

            var organizationDTO = mapper.Map<Models.organizationModels.Organization>(Organization);

            return Ok(organizationDTO);
        }
        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> AddOrganization(AddOrganizationRequest addOrganization)
        {

            var Organization = new Entities.Organization()
            {

                OrganizationName = addOrganization.OrganizationName,
                EmailAddress = addOrganization.EmailAddress,
                PhoneNumber = addOrganization.PhoneNumber,
                Adminemailaddress = addOrganization.Adminemailaddress,
                Adminfullname = addOrganization.Adminfullname,
                OrganizationLogo = addOrganization.OrganizationLogo,
                Pincode = addOrganization.Pincode,
                CityTown = addOrganization.CityTown,
                Country = addOrganization.Country,
                StateProvince = addOrganization.StateProvince,
                StreetLandmark = addOrganization.StreetLandmark,
                HouseBuildingNumber = addOrganization.HouseBuildingNumber,

            };

            Organization = await _services.AddOrgnization(Organization);

            var OrganizationDTO = new Models.organizationModels.Organization()
            {
                TenentID = Organization.TenentID,
                OrganizationName = Organization.OrganizationName,
                EmailAddress = Organization.EmailAddress,
                PhoneNumber = Organization.PhoneNumber,
                Adminemailaddress = Organization.Adminemailaddress,
                Adminfullname = Organization.Adminfullname,
                OrganizationLogo = Organization.OrganizationLogo,
                Pincode = Organization.Pincode,
                CityTown = Organization.CityTown,
                StateProvince = Organization.StateProvince,
                StreetLandmark = Organization.StreetLandmark,
                Country = Organization.Country,
                CreatedDate = Organization.CreatedDate,
                HouseBuildingNumber = Organization.HouseBuildingNumber,
                LastModifiedDate = Organization.LastModifiedDate,
                Password= Organization.Password,
            };

             await emailSender.SendEmailAsync(Organization.Adminemailaddress, "Reset your password",
                $"Please reset your password by <a href='{"http://localhost:3000/ResetPaswword"}'>clicking here</a>.");

            return CreatedAtAction(nameof(GetOrganizationById), new { Id = OrganizationDTO.TenentID }, OrganizationDTO);


        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> updateOrganization([FromRoute] Guid id , [FromBody]Models.organizationModels.UpdateOrganizationRequest updateOrganization)
        {
            var organization = new Entities.Organization()
            {
                OrganizationLogo = updateOrganization.OrganizationLogo,
                OrganizationName = updateOrganization.OrganizationName,
                Adminemailaddress = updateOrganization.Adminemailaddress,
                StateProvince = updateOrganization.StateProvince,
                StreetLandmark = updateOrganization.StreetLandmark,
                Country = updateOrganization.Country,
                Adminfullname = updateOrganization.Adminfullname,
                CityTown = updateOrganization.CityTown,
                EmailAddress = updateOrganization.EmailAddress,
                HouseBuildingNumber = updateOrganization.HouseBuildingNumber,
                PhoneNumber = updateOrganization.PhoneNumber,
                Pincode= updateOrganization.Pincode,
            };

            organization = await _services.UpdateOrganization(id, organization);

            if (organization == null)
            {
                return NotFound();
            }

            var updateOrg = new Models.organizationModels.Organization()
            {
                TenentID = organization.TenentID,
                OrganizationName = organization.OrganizationName,
                EmailAddress = organization.EmailAddress,
                Country = organization.Country,
                LastModifiedDate = organization.LastModifiedDate,
                Adminemailaddress = organization.Adminemailaddress,
                Adminfullname = organization.Adminfullname,
                CityTown = organization.CityTown,
                HouseBuildingNumber = organization.HouseBuildingNumber,
                PhoneNumber = organization.PhoneNumber,
                Pincode = organization.Pincode,
                OrganizationLogo = organization.OrganizationLogo,
                StateProvince = organization.StateProvince,
                StreetLandmark = organization.StreetLandmark,
            };

            return Ok(updateOrg);
        }

        [HttpDelete]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Deleteorg(Guid id)
        {
            var delete = await _services.DeleteOrganization(id);

            if(delete == null)
            {
                return NotFound();
            }

            return Ok(delete);
        }
    }
}
