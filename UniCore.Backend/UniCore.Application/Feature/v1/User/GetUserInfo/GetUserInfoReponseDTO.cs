using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.User.GetUserInfo
{
    public class GetUserInfoResponseDTO 
    {
		public string FirstName { get; set;}="";
		public string LastName { get; set;}="";
		public string FullName { get; set;}="";
		public string PhoneNumber { get; set;}="";
		public string AvatarUrl { get; set;}= "";
		public string Gender { get; set;}="";
		public string BirthDate { get; set;}="";
		public string Address { get; set;}="";
		public string Bio { get; set; }="";
    }
}
