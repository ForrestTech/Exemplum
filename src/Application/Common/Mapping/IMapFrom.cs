namespace Exemplum.Application.Common.Mapping
{
    using AutoMapper;

    public interface IMapFrom<T>
    {   
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}