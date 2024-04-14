using AutoMapper;
using DataExporter.Dtos;
using DataExporter.Model;

namespace DataExporter.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Policy, ReadPolicyDto>();
            CreateMap<ReadPolicyDto, Policy>();

            CreateMap<Policy, CreatePolicyDto>();
            CreateMap<CreatePolicyDto, Policy>();

            CreateMap<Note, NoteDto>();
            CreateMap<NoteDto, Note>();

            CreateMap<Policy, ExportDto>().ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Select(x => x.Text)));
        }
    }
}
