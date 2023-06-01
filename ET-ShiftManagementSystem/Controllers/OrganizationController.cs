using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SuperAdmin,Admin,User,SystemAdmin")]
    [EnableCors("CorePolicy")]
    public class OrganizationController : ControllerBase
    {
        private readonly IorganizationServices _services;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly IUserRepository userRepository;

        public OrganizationController(IorganizationServices services, IMapper mapper , IEmailSender emailSender , IUserRepository userRepository)
        {
            _services = services;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// organization details 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ViewData")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "SuperAdmin,SystemAdmin")]

        public async Task<ActionResult<IEnumerable<Entities.Organization>>> Get()
        {
            try
            {
                var Organization =  _services.GetOrganizationData();

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
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
           

        }

        /// <summary>
        /// organization details 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("Details")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "SuperAdmin,SystemAdmin")]

        public async Task<ActionResult<IEnumerable<Entities.Organization>>> GetDetails()
        {
            try
            {
                var Organization =  _services.GetOrganizationData();

                if (Organization == null)
                {
                    return NotFound();
                }

                var OrganizationRequest = new List<OrganizationDetailsRequest>();

                Organization.ToList().ForEach(Organization =>
                {
                    var organizationRequest = new OrganizationDetailsRequest()
                    {
                        TenentID = Organization.TenentID,
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
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
            

        }

        /// <summary>
        /// View Perticular organization by Giving Tenant Id s
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> GetOrganizationById(Guid id)
        {
            try
            {
                var Organization = await _services.GetOrgByID(id);

                if (Organization != null)
                {
                    var organizationDTO = mapper.Map<Models.organizationModels.Organization>(Organization);

                    return Ok(organizationDTO);
                }
                return NotFound();
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }


        /// <summary>
        /// Add organization by superadmin / system admin 
        /// </summary>
        /// <param name="addOrganization"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> AddOrganization(AddOrganizationRequest addOrganization)
        {
            try
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
                    Password = Organization.Password,
                };
                //request DTO to Domine Model
                var user = new ShiftMgtDbContext.Entities.User()
                {
                    username = Organization.Adminfullname,
                    FirstName = Organization.Adminfullname,
                    //LastName = " ",
                    Email = Organization.Adminemailaddress,
                    //password = "HelloWorld26#",
                    //ContactNumber = "",
                    // AlternateContactNumber = "",
                    TenentID = Organization.TenentID,

                };
                // pass details to repository 

                user = await userRepository.RegisterORGAdminAsync(user);

                //convert back to DTO
                var UserDTO = new Models.UserModel.UserDto
                {
                    id = user.id,
                    username = user.username,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    LastName = user.LastName,
                    password = user.password,
                    IsActive = user.IsActive,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,
                    TenateID = user.TenentID,
                };

                //var resetUrl = Url.Action("LoginAync", "Auth", new { username = user.username, password = user.password }, Request.Scheme);


                await emailSender.SendEmailAsync(Organization.Adminemailaddress, $"Welcome To ShiftManagementSystem",
                    $" Your username is {Organization.Adminfullname} your emai id : {Organization.Adminemailaddress},\n Please reset your password by <h1><a href='{"http://localhost:3000/resetpassword"}'>click here</a></ h1> \n Please login by <h1><a href='{"http://localhost:3000"}'>click here</a></h1>.");

                //await emailSender.SendEmailAsync(Organization.Adminemailaddress, "Reset your password",
                // $"Please reset your password by <a href='{"http://localhost:3000/resetpassword"}'>clicking here</a>.");

                return CreatedAtAction(nameof(GetOrganizationById), new { Id = OrganizationDTO.TenentID }, OrganizationDTO);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


           /* { user.FirstName + " " + user.LastName}
            added to the project . use this credential to login UserId :{ user.username}
            password is :{ user.password}*/
        }

        /// <summary>
        /// Update Organization 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateOrganization"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> updateOrganization([FromRoute] Guid id , [FromBody]Models.organizationModels.UpdateOrganizationRequest updateOrganization)
        {
            try
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
                    Pincode = updateOrganization.Pincode,
                };

                organization = await _services.UpdateOrganization(id, organization);

                //if (organization == null)
                //{
                //    return NotFound();
                //}

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
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
            
        }

        /// <summary>
        /// Delete Organization 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> Deleteorg(Guid id)
        {
            try
            {
                var delete = await _services.DeleteOrganization(id);

                if (delete == null)
                {
                    return NotFound();
                }

                return Ok(delete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
