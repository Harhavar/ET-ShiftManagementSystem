namespace ET_ShiftManagementSystem.Models.organizationModels
{
    public class AddOrganizationRequest
    {
        public string OrganizationName { get; set; }
        public byte[]? OrganizationLogo { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Country { get; set; }
        public string? HouseBuildingNumber { get; set; }
        public string StreetLandmark { get; set; }
        public string CityTown { get; set; }
        public string StateProvince { get; set; }
        public string Pincode { get; set; }
        public string Adminfullname { get; set; }
        public string Adminemailaddress { get; set; }
    }
}
