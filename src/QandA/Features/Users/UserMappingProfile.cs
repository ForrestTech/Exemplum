using AutoMapper;

namespace QandA.Features.Users
{
	public class UserMappingProfile : Profile
	{
		public UserMappingProfile()
		{
			CreateMap<User, UserDTO>();
		}
	}
}