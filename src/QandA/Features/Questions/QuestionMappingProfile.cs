using AutoMapper;

namespace QandA.Features.Questions
{
	public class QuestionMappingProfile : Profile
	{
		public QuestionMappingProfile()
		{
			CreateMap<Question, QuestionDTO>()
				.ForMember(destination => destination.QuestionerUsername, member => member.MapFrom(source => source.Questioner.Username));
		}
	}
}