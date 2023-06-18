using AutoMapper;
using WaveActionApi.Models;
using WaveActionApi.Dtos.Access;
using WaveActionApi.Dtos.Author;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Threads;
using BC = BCrypt.Net.BCrypt;

namespace WaveActionApi.Services;

public class ObjectMapperFactory
{
    private readonly MapperConfiguration _config;

    public ObjectMapperFactory()
    {
        _config = new MapperConfiguration(cfg =>
        {
            // Access Dtos
            cfg.CreateMap<SignupProfileDto, ProfileModel>();
            cfg.CreateMap<SignupDto, AuthorModel>()
                .ForMember(dest => dest.PasswordHash, o => o.MapFrom(a => BC.HashPassword(a.Password)));

            // Author Dtos
            cfg.CreateMap<AuthorProfileDto, ProfileModel>();
            cfg.CreateMap<ProfileModel, AuthorProfileDto>();
            cfg.CreateMap<ProfileModel, AuthorShortProfileDto>();
            cfg.CreateMap<AuthorModel, AuthorDto>();
            cfg.CreateMap<AuthorModel, AuthorShortDto>();

            // Threads Dtos
            cfg.CreateMap<ThreadModel, ThreadDto>();
            cfg.CreateMap<ThreadModel, ThreadShortDto>();
            cfg.CreateMap<ThreadCreateDto, ThreadModel>();
            
            // Posts Dtos
            cfg.CreateMap<PostModel, PostDto>()
                .ForMember(
                    dest => dest.TagList,
                    o => o.MapFrom(p => p.Tags.Split(',', StringSplitOptions.None).ToList()));
            cfg.CreateMap<PostModel, PostShortDto>()
                .ForMember(dest => dest.TagsList,
                    o => o.MapFrom(p => p.Tags.Split(',', StringSplitOptions.None).ToList()));
            cfg.CreateMap<PostCreateDto, PostModel>()
                .ForMember(dest => dest.Tags, o => o.MapFrom(p => string.Join(",", p.TagList!.ToArray())));
        });
    }

    public IMapper CreateMapper() => _config.CreateMapper();
}